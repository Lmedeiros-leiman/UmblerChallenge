using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmbraChallenge.Data.Tables;

namespace UmbraChallenge.Data.Services
{
    public interface ITransactionService {
        // auxiliary services
        public Task<bool> TransactionExits(string transactionId);



        // CRUD (also known as essenctial services)
        public Task<Transaction> CreateTransactionAsync(UserTransferKey senderKey, UserTransferKey receiverKey, decimal transferAmmount);


        
    }
    public class TransactionService(ApplicationDbContext databaseContext) : ITransactionService
    {
        private readonly ApplicationDbContext _context = databaseContext;
        public async Task<bool> TransactionExits(string transactionId) {
            return await _context.Transactions.FindAsync(transactionId) != null;
        }

        public async Task<Transaction> CreateTransactionAsync(UserTransferKey senderKey, UserTransferKey receiverKey, decimal transferAmmount) {
            Transaction temporaryTransaction = new Transaction(senderKey,receiverKey,transferAmmount);

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