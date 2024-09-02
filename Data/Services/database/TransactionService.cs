using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UmbraChallenge.Data.Models;

namespace UmbraChallenge.Data.Services
{
    public interface ITransactionService {
        // auxiliary services
        public Task<bool> TransactionExits(string transactionId);



        // CRUD (also known as essenctial services)
        public Task<Transaction> CreateTransactionAsync( ApplicationUser senderUser, UserTransferKey receiverKey, double transferAmmount);


        
    }
    public class TransactionService(ApplicationDbContext databaseContext) : ITransactionService
    {
        private readonly ApplicationDbContext _context = databaseContext;
        public async Task<bool> TransactionExits(string transactionId) {
            return await _context.Set<Transaction>().FindAsync(transactionId) != null;
        }

        public async Task<Transaction> CreateTransactionAsync(ApplicationUser senderUser, UserTransferKey receiverKey, double transferAmmount) {
            Transaction temporaryTransaction = new() {
                TransactionId = Guid.NewGuid().ToString(),
                Sender = senderUser,
                Receiver = receiverKey,
                TransferAmmount = transferAmmount
            };

            while (await TransactionExits(temporaryTransaction.TransactionId)) {
                temporaryTransaction.TransactionId = Guid.NewGuid().ToString();
            }

            var receiverUser = await _context.Users.FirstAsync(u => u.UserKeysList.Any(k => k.KeyId == receiverKey.KeyId ));

            senderUser.UserTransactions.Add(temporaryTransaction);
            receiverUser.UserTransactions.Add(temporaryTransaction);

            await _context.SaveChangesAsync();

            // in case we may need to use it.
            return temporaryTransaction;
        }
        
    }
}