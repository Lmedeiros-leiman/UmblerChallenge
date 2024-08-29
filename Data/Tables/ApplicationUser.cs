using Microsoft.AspNetCore.Identity;
using UmbraChallenge.Data.Tables;

namespace UmbraChallenge.Data;

// The default user table.
// It has all the good built in stuff that the Identity framework provides with our 
//
public class ApplicationUser : IdentityUser
{
    public string? ShortnedName { get; set; }
    public UserTransferKeys?  
}

