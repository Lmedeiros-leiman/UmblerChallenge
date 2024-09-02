

namespace UmbraChallenge.Data.Services {
    using UmbraChallenge.Data.Models;
    public interface IUserTransferKeysService {
        public Task<Boolean> AddKeyToUser(ApplicationUser targetUser, UmbraChallenge.Components.Account.Pages.Manage.AddTransferKey.InputKeyModel Input);
    
    }
    public class UserTransferKeyService(ApplicationDbContext databaseContext) : IUserTransferKeysService {
        private readonly ApplicationDbContext _context = databaseContext;
        
        public async Task<Boolean> AddKeyToUser(ApplicationUser targetUser, UmbraChallenge.Components.Account.Pages.Manage.AddTransferKey.InputKeyModel Input) {
            
            var databaseUser = await  _context.Users.FindAsync(targetUser.Id);
            if (databaseUser is null) { return false;}

            UserTransferKey newKey = new() { User = databaseUser, KeyValue = Input.KeyValue, KeyType = Input.KeyType };

            databaseUser.UserKeysList.Add(newKey);
            await _context.SaveChangesAsync();

            return true;

        }


    }
}