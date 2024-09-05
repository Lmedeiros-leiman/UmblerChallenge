
namespace UmbraChallenge.Data.Services {
    using System.Collections.ObjectModel;
    using Microsoft.AspNetCore.Components.Authorization;
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
        public Task<ICollection<Transaction>> GetUserTransactions(ApplicationUser user);
        public Task<double> GetUserBalance(ApplicationUser user);
        // returns 

        //
        // Trasactions related
        //
        public Task<PageAlert> MakeTransaction(ApplicationUser user, MakeTransaction.InputTransaction input);


        //
        // UserTransferKeys related
        //
        public Task<PageAlert> AddNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input);
        public Task<PageAlert> RemoveUserTransferKey(ApplicationUser user, UserTransferKey targetKey);
        public Task<PageAlert> ToggleTransferKey(ApplicationUser user, UserTransferKey targetKey, bool newStatus);
        public Task<List<UserTransferKey>> FetchSimilarKeysValues(string KeyValue, ApplicationUser user);
        // toggles the transfer key activity, the key is still registered, just unusable.




    }

    public class UserDatabaseService(ApplicationDbContext dbContext, AuthenticationStateProvider authStatusProvider,ILogger<UserDatabaseService> logger) : IUserDatabaseService {
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
        public async Task<ICollection<Transaction>> GetUserTransactions(ApplicationUser user) {
            return await _dbContext.Transactions.Where(t => t.Receiver.User.Id == user.Id || t.Sender.Id == user.Id).ToListAsync();
        }
        // returns 

        //
        // Trasactions related
        //
        public async Task<double> GetUserBalance(ApplicationUser user) {
            var userTransactions = await this.GetUserTransactions(user);
            var paymentsTotal = userTransactions.Sum(t => t.Sender.Id == user.Id && t.Receiver.User.Id == user.Id ? -t.TransferAmmount : t.TransferAmmount);

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
                    Transaction newTransaction = new() {Sender = user, , TransferAmmount = input.Ammount  };
                    
                    while ( await _dbContext.Transactions.FindAsync(newTransaction.TransactionId) != null) {
                        newTransaction.TransactionId = Guid.NewGuid().ToString();
                    }



                    return new ("Sucessfully deposited into you account.",AlertType.Success);
            }

            var databaseReceivingKey = await _dbContext.UserTransferKeys.FindAsync(input.ReceiverKey.KeyId);
            if (databaseReceivingKey == null) {
                return new PageAlert("Invalid key.", AlertType.Danger);
            }

            if (databaseReceivingKey.User.Id == user.Id) {
                return new PageAlert("You can't transfer money to yourself.", AlertType.Warning);
            }

            if (databaseReceivingKey.IsActive == false) {
                return new PageAlert("Key is not active.", AlertType.Warning);
            }

            var userBalance = await this.GetUserBalance(user);
            if (userBalance < input.Ammount) {
                return new PageAlert("Insufficient funds.", AlertType.Danger);
            }

            Transaction newTransaction = new() {Sender = user, Receiver = databaseReceivingKey, TransferAmmount = input.Ammount  };
            while ( await _dbContext.Transactions.FindAsync(newTransaction.TransactionId) != null) {
                newTransaction.TransactionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation($"New Transaction added between user ID: {user.Id} || \n to key {input.ReceiverKey.KeyId}");
            return new PageAlert("Transaction sucessfull.", AlertType.Success);
        }
        //
        // UserTransferKeys related
        //
        public async Task<PageAlert> AddNewUserTransferKey(ApplicationUser user, AddTransferKey.InputKeyModel Input) {
            // checks if the user already has an key of this type.
            if (await _dbContext.UserTransferKeys.AnyAsync(k => k.KeyType == Input.KeyType && k.User.Id == user.Id)) {
                return new PageAlert("User already has a key of this type.", AlertType.Warning);
            }
            
            try {
                
                var newKey = new UserTransferKey() { UserId = user.Id ,KeyValue = Input.KeyValue, KeyType = Input.KeyType, User = user };
                
                // user can only use keys when the email is confirmed.
                if (user.EmailConfirmed == false) {
                    newKey.IsActive = false;
                }
                // makes sure the key ID is unique.
                while (await _dbContext.UserTransferKeys.FindAsync(newKey.KeyId) != null) {
                    newKey.KeyId = Guid.NewGuid().ToString();
                }

                _dbContext.UserTransferKeys.Add(newKey);
                _dbContext.SaveChanges();

                _logger.LogInformation($"New key added for user ID: {user.Id}");
                return new PageAlert("New key added.", AlertType.Success);
            }
            catch (System.Exception error) {
                return new PageAlert(error.Message, AlertType.Danger);
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

            return await _dbContext.UserTransferKeys.Where( k => k.KeyValue.Contains(KeyValue) && k.User.Id != loggedUser.Id ).Include(k => k.User).ToListAsync();
        }
    }
}