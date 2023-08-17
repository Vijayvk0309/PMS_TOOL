using Microsoft.Data.SqlClient;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;
using System.Data;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        public string? ConnectionString { get; }
        private string providerName;


        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = _configuration.GetConnectionString("DefaultConnection");
            con = new SqlConnection(constr);

        }
        public string InsertData(User data)
        {
            string result = "";
            connection();
            SqlCommand cmd = new SqlCommand("InsertUpdateDelete_User", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", data.UserName);
            cmd.Parameters.AddWithValue("@Password", data.Password);
            cmd.Parameters.AddWithValue("@Access", data.Access);
            cmd.Parameters.AddWithValue("@EmployeeId", data.EmployeeId);
            cmd.Parameters.AddWithValue("@Query", 1);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return result;

            }
            else
            {

                return result;
            }
        }
        public bool UpdateDetails(User data)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserName", data.UserName);
            cmd.Parameters.AddWithValue("@Password", data.Password);
            cmd.Parameters.AddWithValue("@EmployeeId", data.EmployeeId);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        /*  public string UpdateDetails(User data)
          {
              string result = "";
              connection();
              SqlCommand cmd = new SqlCommand("UpdateUserDetails", con);
              cmd.CommandType = CommandType.StoredProcedure;

              cmd.Parameters.AddWithValue("@UserName", data.UserName);
              cmd.Parameters.AddWithValue("@Password", data.Password);
              cmd.Parameters.AddWithValue("@EmployeeId", data.EmployeeId);
              con.Open();
              int i = cmd.ExecuteNonQuery();
              con.Close();

              if (i >= 1)
                  return result;
              else
                  return result;
          }*/
        /*   public string EditPages(User data)
           {
               string result = "";

               connection();
               SqlCommand cmd = new SqlCommand("InsertUpdateDelete_User", con);

               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@UserName", data.UserName);
               cmd.Parameters.AddWithValue("@Password", data.Password);
               cmd.Parameters.AddWithValue("@Access", data.Access);
               cmd.Parameters.AddWithValue("@EmployeeId", data.EmployeeId);
               cmd.Parameters.AddWithValue("@Query", 2);
               con.Open();
               int i = cmd.ExecuteNonQuery();
               con.Close();
               if (i >= 1)
               {

                   return result;

               }
               else
               {

                   return result;
               }
           }*/

        public List<User> GetRegister(int id)
        {
            List<User> result = new List<User>();
            connection();
            SqlCommand cmd = new SqlCommand("InsertUpdateDelete_User", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Query", 5);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            result = (from DataRow dr in dt.Rows

                      select new User()
                      {
                          Id = Convert.ToInt32(dr["Id"]),
                          UserName = Convert.ToString(dr["UserName"]),
                          Password = Convert.ToString(dr["Password"]),
                          Access = Convert.ToString(dr["Access"]),
                          EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                      }).ToList();
            return result;

        }

        public List<User> GetServiceAsync()
        {
            List<User> result = new List<User>();
            connection();
            SqlCommand cmd = new SqlCommand("InsertUpdateDelete_User", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Query", 4);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            result = (from DataRow dr in dt.Rows

                      select new User()
                      {
                          Id = Convert.ToInt32(dr["Id"]),
                          UserName = Convert.ToString(dr["UserName"]),
                          Password = Convert.ToString(dr["Password"]),
                          Access = Convert.ToString(dr["Access"]),
                          EmployeeId = Convert.ToInt32(dr["EmployeeId"]),
                      }).ToList();
            return result;

        }
       

       

         
    }

}
