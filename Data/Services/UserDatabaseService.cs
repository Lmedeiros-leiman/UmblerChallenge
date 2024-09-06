
namespace UmbraChallenge.Data.Services {
    using System.Collections.ObjectModel;
    using System.Text.Json;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.CodeAnalysis;
    using Microsoft.EntityFrameworkCore;
    using UmbraChallenge.Components.Account.Pages.Manage;
    using UmbraChallenge.Components.Shared;
    using UmbraChallenge.Data.Models;
    using UmbraChallenge.Models;

    interface IUserDatabaseService {
        public Task<ApplicationUser?> GetCurrentAuthUser();
        // returns the current logged in user, or null if not logged in.
        // preferably used on all routes that require authentication.
        public Task<ICollection<UserTransferKey>> GetUserTransferKeys(ApplicationUser user);
        public Task<ICollection<Transaction>> GetUserTransactions(ApplicationUser user, bool includeCancelled = false);
        public Task<double> GetUserBalance(ApplicationUser user, bool calculateCancelled = false);

        //
        // Trasactions related
        //
        public Task<PageAlert> MakeTransaction(ApplicationUser user, MakeTransaction.InputTransaction input);
        public Task<PageAlert> CancelTransaction(ApplicationUser user, Transaction targetTransaction);


        //
        // UserTransferKeys related
        //
        public Task<PageAlert> AddNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input);
        public Task<UserTransferKey?> AddAndGetNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input);
        public Task<PageAlert> RemoveUserTransferKey(ApplicationUser user, UserTransferKey targetKey);
        public Task<PageAlert> ToggleTransferKey(ApplicationUser user, UserTransferKey targetKey, bool newStatus);

        public Task<UserTransferKey> FindUserTransferKey(ApplicationUser user, PossibleTransferKeys keyType);
        public Task<List<UserTransferKey>> FetchSimilarKeysValues(string KeyValue, ApplicationUser user);
        // toggles the transfer key activity, the key is still registered, just unusable.




    }

    public class UserDatabaseService(ApplicationDbContext dbContext, AuthenticationStateProvider authStatusProvider, ILogger<UserDatabaseService> logger) : IUserDatabaseService {
        public readonly ApplicationDbContext _dbContext = dbContext;
        public readonly AuthenticationStateProvider _authStatus = authStatusProvider;



        public readonly ILogger<UserDatabaseService> _logger = logger;

        public async Task<ApplicationUser?> GetCurrentAuthUser() {
            var user = (await _authStatus.GetAuthenticationStateAsync()).User;

            if (user.Identity != null && user.Identity?.IsAuthenticated == true) {

                string? userName = user.Identity.Name;
                if (!string.IsNullOrEmpty(userName)) {
                    return await _dbContext.Users
                                .Where(u => u.UserName.Equals(userName) || u.Email.Equals(userName))
                                .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync to avoid exceptions if no match is found
                }
            }
            return null;
        }


        // returns the current logged in user, or null if not logged in.
        // preferably used on all routes that require authentication.
        public async Task<ICollection<UserTransferKey>> GetUserTransferKeys(ApplicationUser user) {
            return await _dbContext.UserTransferKeys.Where(k => k.User.Id == user.Id).ToListAsync();
        }
        public async Task<ICollection<Transaction>> GetUserTransactions(ApplicationUser user, bool includeCancelled = false) {
            // unfortunatelly if we call an Application user the app resolves into an infinite loop. 
            // so we have to MANUALLY include what we need.

            var transactions = await _dbContext.Transactions
                .Where(t =>
                    (t.Receiver.UserId == user.Id || t.Sender.Id == user.Id) &&
                    (t.Status != TransactionStatus.Cancelled || includeCancelled == true) )
                .Select(t => new {
                    t.TransactionId,
                    t.TransferAmmount,
                    t.Timestamp,
                    Sender = new {
                        t.Sender.Id,
                        t.Sender.UserName,
                        t.Sender.Email,
                    },
                    Receiver = new {
                        t.Receiver.KeyId,
                        t.Receiver.UserId,
                        t.Receiver.KeyValue,
                        t.Receiver.KeyType,
                        t.Receiver.IsActive,
                        t.Receiver.CreationTimeStamp,
                        User = new {
                            t.Receiver.User.Id,
                            t.Receiver.User.Email,
                            t.Receiver.User.UserName,
                        }
                    },
                    t.Status,
                })
                .AsNoTracking()
                .ToListAsync();

                ICollection<Transaction> builtTransactionsResult = [];
                foreach( var t in transactions) {
                    Transaction transactionModel = new() {
                        Status = t.Status, 
                        Timestamp = t.Timestamp,
                        TransactionId = t.TransactionId, 
                        TransferAmmount = t.TransferAmmount,
                        Receiver = new() {
                            User = new() {
                                Id = t.Receiver.User.Id,
                                Email = t.Receiver.User.Email,
                                UserName = t.Receiver.User.UserName,
                            },
                            UserId = t.Receiver.UserId,
                            KeyValue = t.Receiver.KeyValue,
                            KeyType = t.Receiver.KeyType,
                            KeyId = t.Receiver.KeyId,
                            IsActive = t.Receiver.IsActive,
                            CreationTimeStamp = t.Receiver.CreationTimeStamp
                            },
                        Sender = new () {
                            Id = t.Sender.Id,
                            Email = t.Sender.Email,
                            UserName = t.Sender.UserName,
                        }
                    };
                    builtTransactionsResult.Add(transactionModel);
                }
                return builtTransactionsResult.OrderByDescending(t => t.Timestamp).ToList(); ;
        }
        //
        // Trasactions related
        //
        public async Task<double> GetUserBalance(ApplicationUser user, bool calculateCancelled = false) {
            var userTransactions = await this.GetUserTransactions(user);

            // if the sender is the user AND the receiver is also the user, its a deposit.
            // if the sender is the user and the receiver is not, its a payment/transaction.
            //
            double paymentsTotal = 0.00d;

            for (int i = 0; i < userTransactions.Count; i++) {
                Transaction currentTransaction = userTransactions.ElementAt(i);

                if (currentTransaction.Status == TransactionStatus.Cancelled && calculateCancelled == false) { continue;}

                if (currentTransaction.Sender.Id == user.Id) {
                    if (currentTransaction.Receiver.User.Id == user.Id) {
                        // its a deposit.
                        paymentsTotal += currentTransaction.TransferAmmount;
                        continue;
                    } else {
                        // its a Transaction to another user
                        paymentsTotal -= currentTransaction.TransferAmmount;
                        continue;
                    }
                } else {
                    if (currentTransaction.Receiver.User.Id == user.Id) {
                        // its a Transaction to this user
                        paymentsTotal += currentTransaction.TransferAmmount;
                        continue;
                    }
                }
            }

            return paymentsTotal;
        }
        public async Task<PageAlert> MakeTransaction(ApplicationUser user, MakeTransaction.InputTransaction input) {
            _logger.LogInformation($"user ID: {user.Id} trying to make a new transaction.");


            if (input.Ammount <= 0) {
                return new PageAlert("We do not allow transfers of zero or negative amounts.", AlertType.Danger);
            }

            if (input.IsDeposit) {
                // deposits will automatically deposit into the email key of the user.
                // if there isnt one, create it. BUT set to inactive if the user did not validate it.
                //
                var userEmailKey = await this.FindUserTransferKey(user, PossibleTransferKeys.Email);
                if (userEmailKey == null) {
                    var newUserEmailKey = await this.AddAndGetNewUserTransferKey(user, new() { KeyType = PossibleTransferKeys.Email, KeyValue = user.Email});
                    if (newUserEmailKey != null) {
                        userEmailKey = newUserEmailKey;
                    }
                }
                // ignore the warning, there is no way for userEmailKey to be null here.
                Transaction newDeposit = new() { Sender = user, Receiver = userEmailKey, TransferAmmount = input.Ammount };

                while (await _dbContext.Transactions.FindAsync(newDeposit.TransactionId) != null) {
                    newDeposit.TransactionId = Guid.NewGuid().ToString();
                }

                _dbContext.Add(newDeposit);
                await _dbContext.SaveChangesAsync();

                return new("Sucessfully deposited into you account.", AlertType.Success);
            }
            else {
                var databaseReceivingKey = await _dbContext.UserTransferKeys.FindAsync(input.ReceiverKey.KeyId);

                if (databaseReceivingKey == null) { return new("Invalid key.", AlertType.Warning); }
                if (databaseReceivingKey.User.Id == user.Id) { return new("You can't transfer money to yourself.", AlertType.Warning); }
                if (databaseReceivingKey.IsActive == false) { return new("Key is not active.", AlertType.Warning); }

                double userBalance = await this.GetUserBalance(user);
                if (userBalance < input.Ammount) { return new("Insufficient funds.", AlertType.Warning); }

                Transaction newTransaction = new() { Sender = user, Receiver = databaseReceivingKey, TransferAmmount = input.Ammount };
                while (await _dbContext.Transactions.FindAsync(newTransaction.TransactionId) != null) {
                    newTransaction.TransactionId = Guid.NewGuid().ToString();
                }
                _dbContext.Add(newTransaction);
                await _dbContext.SaveChangesAsync();
                return new("Transaction sucessfull.", AlertType.Success);
            }

        }
        
        public async Task<PageAlert> CancelTransaction(ApplicationUser user, Transaction targetTransaction) {
            if ( string.IsNullOrEmpty(targetTransaction.TransactionId) ) { return new("Transaction ID is not valid.", AlertType.Danger);}
            
            var databaseTransaction = await _dbContext.Transactions.FindAsync(targetTransaction.TransactionId);
            Console.WriteLine(JsonSerializer.Serialize(databaseTransaction));
            if (databaseTransaction == null) { return new("Transaction not found.", AlertType.Danger); }
            
            //if (databaseTransaction.Sender.Id != user.Id) { return new("You can only cancel your own transactions.", AlertType.Danger); }
            
            databaseTransaction.Status = TransactionStatus.Cancelled;
            await _dbContext.SaveChangesAsync();
            
            return new PageAlert( "Transaction cancelled sucessfully." ,AlertType.Success);
        }
        
        //
        // UserTransferKeys related
        //
        public async Task<UserTransferKey?> AddAndGetNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input) {
            // checks if the user already has an key of this type.
            if (await _dbContext.UserTransferKeys.AnyAsync(k => k.KeyType == Input.KeyType && k.User.Id == user.Id)) {
                return null;
            }

            try {

                var newKey = new UserTransferKey() { UserId = user.Id, KeyValue = Input.KeyValue, KeyType = Input.KeyType, User = user };

                
                // makes sure the key ID is unique.
                while (await _dbContext.UserTransferKeys.FindAsync(newKey.KeyId) != null) {
                    newKey.KeyId = Guid.NewGuid().ToString();
                }

                _dbContext.UserTransferKeys.Add(newKey);
                _dbContext.SaveChanges();

                _logger.LogInformation($"New key added for user ID: {user.Id}");
                return newKey;
            }
            catch (System.Exception error) {
                return null;
            }
        }
        public async Task<PageAlert> AddNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input) {
            try {
                if (await AddAndGetNewUserTransferKey(user, Input) != null) {
                    return new PageAlert("New key added.", AlertType.Success);
                }
                else {
                    return new PageAlert($"Key not added, Already Exists or user already has one for {Input.KeyType} type.", AlertType.Warning);
                }

            }
            catch (Exception error) {
                _logger.LogCritical($"Error adding new userKey for user ID {user.Id}: {error.Message} at {DateTime.UtcNow}");
                return new PageAlert($"Key not added, crucial error. {error.Message}", AlertType.Danger);
            }

        }
        public async Task<PageAlert> RemoveUserTransferKey(ApplicationUser user, UserTransferKey targetKey) {
            var databaseKey = await _dbContext.UserTransferKeys.FindAsync(targetKey.KeyId);

            if (databaseKey == null) {
                return new PageAlert("Key not found or invalid.", AlertType.Danger);
            }

            if (databaseKey.User.Id != user.Id) {
                return new PageAlert("Key does not belog to this account.", AlertType.Danger);
            }

            _dbContext.UserTransferKeys.Remove(databaseKey);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"New key added for user ID: {user.Id}");
            return new PageAlert("Key removed.", AlertType.Success);
        }
        public async Task<PageAlert> ToggleTransferKey(ApplicationUser user, UserTransferKey targetKey, bool newStatus) {

            var databaseKey = await _dbContext.UserTransferKeys.FindAsync(targetKey.KeyId);

            if (databaseKey == null) {
                return new PageAlert("Key not found or invalid.", AlertType.Danger);
            }

            if (databaseKey.User.Id != user.Id) {
                return new PageAlert("Key does not belog to this account.", AlertType.Danger);
            }

            

            databaseKey.IsActive = newStatus;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Key set to {databaseKey.IsActive.ToString()} for user ID: {user.Id}");
            return new PageAlert("Key updated.", AlertType.Success);
        }

        public async Task<List<UserTransferKey>> FetchSimilarKeysValues(string KeyValue, ApplicationUser loggedUser) {
            if (string.IsNullOrEmpty(KeyValue)) { return []; }

            return await _dbContext.UserTransferKeys.Where(k => k.KeyValue.Contains(KeyValue) && k.User.Id != loggedUser.Id).Include(k => k.User).ToListAsync();
        }
        public async Task<UserTransferKey> FindUserTransferKey(ApplicationUser user, PossibleTransferKeys keyType) {
            return await _dbContext.UserTransferKeys.FirstOrDefaultAsync(k => k.KeyType == keyType && k.User.Id == user.Id);
        }
    }
}