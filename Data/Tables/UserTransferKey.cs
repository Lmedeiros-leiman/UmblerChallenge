using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


/*
    This is the model for a user connection data,
    This is similar to how PIX works by having a certain key connected a specific user.

    We CAN allow for multple users to have the same type of key and value, since we could call both of then and let the user decide witch one is the correct one.
    but for simplicity every type value is unique.

*/

namespace UmbraChallenge.Data.Models
{
    public enum PossibleTransferKeys {
        Email,
        Phone,
        Name,
        Card,
        CPF,
        CNPJ,
        Deposit,

    };
    
    [Table("UserTransferKeys")]
    public class UserTransferKey
    {
        
        [Key]
        public string KeyId {get; set;} = Guid.NewGuid().ToString();
        public required string UserId {get; set;}
        
        [DataType(DataType.DateTime)]
        public DateTime CreationTimeStamp {get; set;} = DateTime.UtcNow;
        public  ApplicationUser? User {get; set;}
        public required PossibleTransferKeys KeyType {get;set;}
        public bool IsActive {get;set;} = true;
        public required string KeyValue {get;set;}
    }
}