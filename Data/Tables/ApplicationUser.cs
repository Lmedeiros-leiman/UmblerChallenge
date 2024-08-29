using Microsoft.AspNetCore.Identity;
using UmbraChallenge.Data.Tables;

namespace UmbraChallenge.Data;

// The default user table.
// It has all the good built in stuff that the Identity framework provides with our 
// Each user has additionaly a personal NickName and transfer keys that will link to their accounts
// 
public class ApplicationUser : IdentityUser
{
    public string? Nickname { get; set; }
    public required ICollection<UserTransferKey> UserKeysList { get; set; } = [];
    public required ICollection<Transaction> UserTransactions { get;set;} = [];
    
}

