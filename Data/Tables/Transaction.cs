using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UmbraChallenge.Data.Services;
using UmbraChallenge.Data.Models;

namespace UmbraChallenge.Data.Models
{
    // This table may be the most important one in the database.
    // It holds all interactions users have with eachothers
    // BUT it also includes transactions the users have with the platform itself.
    // it also handles money transfers and that by itself is scary enough.
    //
    [Table("Transactions")]
    public class Transaction {
        [Key]
        public required string TransactionId {  get; set;}
        public required ApplicationUser Sender {get; set;}
        public required UserTransferKey Receiver {get; set;}
        public required double TransferAmmount {get; set;}
        
        
        public readonly DateTime Timestamp = DateTime.UtcNow;
        
        public Transaction? CancelationTransation {get; set;} // refers to a transaction that cancels this one.

    }
}