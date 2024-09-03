namespace UmbraChallenge.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UmbraChallenge.Data.Models;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public required DbSet<Transaction> Transactions { get; set; }
    public required DbSet<UserTransferKey> UserTransferKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        // creates some users to test the app.
        // they all have the same password: empty/null

        var users = new List<ApplicationUser> {
                new() {Id = Guid.NewGuid().ToString(), UserName = "admin@localhost", Email = "admin@localhost", NormalizedEmail = "admin@localhost".ToUpper(),  EmailConfirmed = true},
                new() {Id = Guid.NewGuid().ToString(), UserName = "john@localhost", Email = "john@localhost", NormalizedEmail = "john@localhost".ToUpper(), EmailConfirmed = true},
                new() {Id = Guid.NewGuid().ToString(), UserName = "maria@localhost", Email = "maria@localhost", NormalizedEmail = "maria@localhost".ToUpper(), EmailConfirmed = true}
            };
        builder.Entity<ApplicationUser>().HasData(users);

        var EmailKeys = new List<UserTransferKey>() {
            new() {KeyId = Guid.NewGuid().ToString(), UserId = users[0].Id, KeyType = PossibleTransferKeys.Email, KeyValue = "admin@localhost"},
            new() {KeyId = Guid.NewGuid().ToString(), UserId = users[1].Id, KeyType = PossibleTransferKeys.Email, KeyValue = "john@localhost"},
            new() {KeyId = Guid.NewGuid().ToString(), UserId = users[2].Id, KeyType = PossibleTransferKeys.Email, KeyValue = "maria@localhost"}
        };
        builder.Entity<UserTransferKey>().HasData(EmailKeys);

    }
}
