using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return await _context.Transactions.FindAsync(transactionId) != null;
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

            await _context.Transactions.AddAsync(temporaryTransaction);
            await _context.SaveChangesAsync();

            // in case we may need to use it.
            return temporaryTransaction;
        }
        
    }
}