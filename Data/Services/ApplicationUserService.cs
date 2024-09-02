using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using UmbraChallenge.Data.Models;




namespace UmbraChallenge.Data.Services {
    public interface IApplicationUserService {
        public Task<ApplicationUser?> GetCurrentAuthUser();



    }

    public class ApplicationUserService(ApplicationDbContext databaseContext, AuthenticationStateProvider AuthStatus, ILogger<ApplicationUserService> logger) : IApplicationUserService {
        private readonly ILogger _logger = logger;
        private readonly ApplicationDbContext _context = databaseContext;
        private readonly AuthenticationStateProvider _authStatus = AuthStatus;

        public async Task<ApplicationUser?> GetCurrentAuthUser() {
            
            var user = (await _authStatus.GetAuthenticationStateAsync()).User;
            


            if (user.Identity != null && user.Identity?.IsAuthenticated == true) {
                string? userName = user.Identity.Name;

                var a = userName?.Normalize().ToUpper();

                if ( !string.IsNullOrEmpty(userName)) {
                    return await _context.Users
                        .Where(u => u.NormalizedUserName == userName.Normalize().ToUpper() )
                        .Include(u => u.UserTransactions)
                        .Include(u => u.UserKeysList)
                        .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync to avoid exceptions if no match is found
                }
            }
            return null;
        }

        // registration and login







        // casual data fetching.

    }
}