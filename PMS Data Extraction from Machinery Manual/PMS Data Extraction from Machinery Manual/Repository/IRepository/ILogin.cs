using PMS_Data_Extraction_from_Machinery_Manual.Models;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository
{
    public interface ILogin
    {
        Task<IEnumerable<User>> getuser();
        Task<User> AuthenticateUser(string username, string password);
    }
}
