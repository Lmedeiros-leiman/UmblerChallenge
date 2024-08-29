using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UmbraChallenge.Data.Tables;

namespace UmbraChallenge.Data.Services
{
    public interface ITransactionService {
        public Task<Transaction?> getTransactionById(string transactionID);
    }
    public class TransactionService(ApplicationDbContext databaseContext) : ITransactionService
    {
        private readonly ApplicationDbContext _context = databaseContext;


        public async Task<Transaction?> getTransactionByIdA(string transactionID) {
            return await _context.Transactions.FindAsync(transactionID);
        }
    }
}