using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/*
    This is the model for a user connection data,
    This is similar to how PIX works by having a certain key connected a specific user.

    We CAN allow for multple users to have the same type of key and value, since we could call both of then and let the user decide witch one is the correct one.
    but for simplicity every type value is unique.

*/

namespace UmbraChallenge.Data.Tables
{
    public enum PossibleTransferKeys
        {
            Email,
            Phone,
            Name,
            Card,
            CPF,
            CNPJ,

        };
    public class UserTransferKey
    {
        
        [Key]
        public required string KeyId {get; set;} = Guid.NewGuid().ToString();
        
        
        public required ApplicationUser User {get; set;}
        public required PossibleTransferKeys KeyType {get;set;}
        public required string KeyValue {get;set;}
    }
}