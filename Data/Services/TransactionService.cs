using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace UmbraChallenge.Data.Services
{
    public interface ITransactionService {
        public Task<Transaction?> getTransactionById(string transactionID);
    }
    public class TransactionService(ApplicationDbContext databaseContext) : ITransactionService
    {
        private readonly ApplicationDbContext _context = databaseContext;


        public async Task<Transaction?> getTransactionByIdA(string transactionID) {
            return _context.
        }
    }
}