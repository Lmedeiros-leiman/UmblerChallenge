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
    public enum TransactionStatus{
        Valid,

        Cancelled,
    }
    [Table("Transactions")]
    public class Transaction {
        [Key]
        public string TransactionId {  get; set;} = Guid.NewGuid().ToString();
        public required ApplicationUser Sender {get; set;}
        public required UserTransferKey Receiver {get; set;}
        public required double TransferAmmount {get; set;}
        
        [DataType(DataType.DateTime)]
        public  DateTime Timestamp {get; set;} = DateTime.UtcNow;
        
        public TransactionStatus Status {get; set;} = TransactionStatus.Valid;

    }
}