using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UmbraChallenge.Data.Models;

// The default user table.
// It has all the good built in stuff that the Identity framework provides with our 
// Each user has additionaly a personal NickName and transfer keys that will link to their accounts
// 
public class ApplicationUser : IdentityUser {
    //
    // these are here to possibily make it easier to cary these objects around without making multiple parameters.
    // since these empty lists could be manually filled.
    
    [NotMapped]
    public ICollection<Transaction> TransactionsList { get; set; } = [];
    [NotMapped]
    public ICollection<UserTransferKey> UserKeysList { get; set; } = [];
}

