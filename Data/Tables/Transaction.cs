using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UmbraChallenge.Data.Services;
using UmbraChallenge.Data.Tables;

namespace UmbraChallenge.Data.Tables
{
    // This table may be the most important one in the database.
    // It holds all interactions users have with eachothers
    // BUT it also includes transactions the users have with the platform itself.
    // it also handles money transfers and that by itself is scary enough.
    //
    [Table("Transactions")]
    public class Transaction {
        [Key]
        public  string TransactionId {  get; set;}
        public  UserTransferKey Sender {get; set;}
        public  UserTransferKey Receiver {get; set;}
        public  decimal TransferAmmount {get; set;}

        public Transaction( UserTransferKey senderKey, UserTransferKey receiverKey, decimal transferAmmount) {
            TransactionId = Guid.NewGuid().ToString();
            Sender = senderKey;
            Receiver = receiverKey;
            TransferAmmount = transferAmmount;
        }
    }
}