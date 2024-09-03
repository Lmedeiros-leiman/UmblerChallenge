using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UmbraChallenge.Data.Models;

/*

    Only the tables that will be created in the database model.
    Realistically, it's not necessary to specify some of the models like the transaction table or transfer keys table.
    However, this needs to be done to access the tables directly within the services to avoid headaches.

*/

namespace UmbraChallenge.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public required DbSet<Transaction> Transactions { get; set; }
    public required DbSet<UserTransferKey> UserTransferKeys { get; set; }
}
