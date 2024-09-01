using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmbraChallenge.Models
{
    
    public class Currency
    {
        public double Value {get; set;} = 0d;
        public readonly string CurrencyType = "R$";
        
        // Normally a currency/money would have a type, like Dollar, Real, Peso, Bitcoin, Ethereum, etc.
        // But that would be a secondary challenge that requires both an api (that i dont know an endpoint for) and a money converter class.
        // For now, currency is just a value.
    }
}