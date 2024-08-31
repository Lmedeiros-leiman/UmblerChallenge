using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UmbraChallenge.Data.Services
{
    public interface IApplicationUserService {




    }
    
    public class ApplicationUserService(ApplicationDbContext databaseContext) : IApplicationUserService
    {
        private readonly ApplicationDbContext _context  = databaseContext;
        
        // registration and login


        public Boolean isEmailRegistered(string email) {
            
            return false;
        }






        // casual data fetching.
        
    }
}