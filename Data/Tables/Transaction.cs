using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UmbraChallenge.Data.Services;

namespace UmbraChallenge.Data.Tables
{
    // This table may be the most important one in the database.
    // It holds all interactions users have with eachothers
    // BUT it also includes transactions the users have with the platform itself.
    // it also handles money transfers and that by itself is scary enough.
    //
    [Table("Transactions")]
    public class Transaction()
    {
        protected readonly static TransactionService _service =
        [Key]
        private string TransactionId {  get; set;} = GenerateRandomId();




        private static string GenerateRandomId() {
            
            string newlyCreatedId = Guid.NewGuid().ToString();
            while ( TransactionSe ) {
                
            }
            Guid newId = 
            
            return newId.ToString();
        }
    }
}