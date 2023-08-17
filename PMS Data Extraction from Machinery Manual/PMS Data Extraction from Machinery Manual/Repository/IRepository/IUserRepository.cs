using PMS_Data_Extraction_from_Machinery_Manual.Models;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository
{
    public interface IUserRepository
    {
        List<User> GetServiceAsync();
        List<User> GetRegister(int id);
    
        public string InsertData(User data);
    /*    public string EditPages(User data);*/
        public bool UpdateDetails(User data);

    }
}
