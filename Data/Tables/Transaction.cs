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
    public class Transaction( UserTransferKey senderKey, UserTransferKey receiverKey, decimal transferAmmount)
    {
        
        [Key]
        public string TransactionId {  get; set;} = Guid.NewGuid().ToString();

        public UserTransferKey Sender = senderKey;
        public UserTransferKey Receiver = receiverKey;
        public decimal TransferAmmount = transferAmmount;

    }
}