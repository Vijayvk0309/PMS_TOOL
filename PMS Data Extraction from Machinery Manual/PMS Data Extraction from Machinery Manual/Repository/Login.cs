using Microsoft.EntityFrameworkCore;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository
{
    public class Login : ILogin
    {
        private readonly DbContextClass _context;
        public Login(DbContextClass context)
        {
            _context = context;
        }

        public Task<IEnumerable<User>> getuser()
        {
            throw new NotImplementedException();
        }

        Task<User> ILogin.AuthenticateUser(string username, string password)
        {
            var succeeded = _context.Users.FirstOrDefaultAsync(authUser => authUser.UserName == username && authUser.Password == password);
            return succeeded;
        }
    }
}
