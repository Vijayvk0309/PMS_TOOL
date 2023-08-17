using Microsoft.Data.SqlClient;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository
{
    public class ExcelValidateRepository : IExcelValidateRepository
    {
        private readonly IConfiguration _configuration;
        public string? ConnectionString { get; }
        private string providerName;


        public ExcelValidateRepository(IConfiguration configuration)
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
        public string ExcelData(ExcelValidate data)
        {
            string result = "";
            connection();
            SqlCommand cmd = new SqlCommand("AddExcelData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientName", data.ClientName);
            cmd.Parameters.AddWithValue("@VesselName", data.VesselName);
            cmd.Parameters.AddWithValue("@ManualPath", data.ManualPath);
            cmd.Parameters.AddWithValue("@ManualName", data.ManualName + ".pdf");
            cmd.Parameters.AddWithValue("@TotalPages", data.TotalPages);
            cmd.Parameters.AddWithValue("@EquipmentName", data.EquipmentName);
            cmd.Parameters.AddWithValue("@Maker", data.Maker);
            cmd.Parameters.AddWithValue("@ModelType", data.ModelType);
            cmd.Parameters.AddWithValue("@NoOfUnit", data.NoOfUnit);
            cmd.Parameters.AddWithValue("@SparePageNo", data.SparePageNo);
            cmd.Parameters.AddWithValue("@JobPageNo", data.JobPageNo);
            cmd.Parameters.AddWithValue("@TechnicalData", data.TechnicalData);
            cmd.Parameters.AddWithValue("@Remarks", data.Remarks);
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

       

        public List<ExcelValidate> GetExcelValues()
        {
            List<ExcelValidate> result = new List<ExcelValidate>();
            connection();
            SqlCommand cmd = new SqlCommand("GetExcelData", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            result = (from DataRow dr in dt.Rows
                      select new ExcelValidate()
                      {
                          ExcelId = Convert.ToInt32(dr["ExcelId"]),
                          ClientName = Convert.ToString(dr["ClientName"]),
                          VesselName = Convert.ToString(dr["VesselName"]),
                          ManualPath = Convert.ToString(dr["ManualPath"]),
                          ManualName = Convert.ToString(dr["ManualName"]),
                          FilePath = Convert.ToString(dr["ManualPath"]) + Convert.ToString(dr["ManualName"]),
                          OutputPath = "D:/PMS/Manuals/TechnicalManual/Process/",
                          EquipmentName = Convert.ToString(dr["EquipmentName"]),
                          TotalPages = Convert.ToInt32(dr["TotalPages"]),
                          SparePageNo = Convert.ToString(dr["SparePageNo"]),
                          JobPageNo = Convert.ToString(dr["JobPageNo"]),
                          Maker = Convert.ToString(dr["Maker"]),
                          ModelType= Convert.ToString(dr["ModelType"]),
                          NoOfUnit = Convert.ToString(dr["NoOfUnit"]),
                          TechnicalData = Convert.ToString(dr["TechnicalData"]),
                          Remarks = Convert.ToString(dr["Remarks"])

                      }).ToList();
            return result;
        }
  /*      public List<ExcelValidate> GetExcelValuesExist(string clientName, string vesselName, string manualName)
        {
            List<ExcelValidate> result = new List<ExcelValidate>();
            connection();
            SqlCommand cmd = new SqlCommand("CheckExcelDataExistence", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientName",clientName);
            cmd.Parameters.AddWithValue("@VesselName", vesselName);
            cmd.Parameters.AddWithValue("@ManualName", manualName);
           
        }*/
      /*  public string GetExcelValuesExist(string clientName, string vesselName, string manualName)
        {
            List<ExcelValidate> result = new List<ExcelValidate>();
            connection();

            SqlCommand cmd = new SqlCommand("CheckExcelDataExistence", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientName", clientName);
            cmd.Parameters.AddWithValue("@VesselName", vesselName);
            cmd.Parameters.AddWithValue("@ManualName", manualName);

            SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);
            cmd.ExecuteNonQuery();
            int procedureResult = Convert.ToInt32(outputParam.Value);
            return procedureResult;
        }*/


        public List<ExcelValidate> GetPdfFileById(int id)
        {
            connection();
            var command = new SqlCommand("GetPdfFile", con);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ExcelId", id);

            var result = new List<ExcelValidate>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new ExcelValidate()
                    {
                        ExcelId = (int)reader["ExcelId"],
                        ManualPath = (string)reader["ManualPath"],
                        ManualName = (string)reader["ManualName"],
                        FilePath = (string)reader["ManualPath"] + (string)reader["ManualName"]
                    });
                }
            }

            return result;
        }

        /*  public string GetExcelValuesExist(string ClientName, string VesselName, string ManualName)
          {
              connection();

              SqlCommand cmd = new SqlCommand("CheckExcelDatasExistence", con);
              cmd.CommandType = CommandType.StoredProcedure;
              cmd.Parameters.AddWithValue("@ClientName", ClientName);
              cmd.Parameters.AddWithValue("@VesselName", VesselName);
              cmd.Parameters.AddWithValue("@ManualName", ManualName);

              SqlParameter outputParam = new SqlParameter("@Result", SqlDbType.Int);
              outputParam.Direction = ParameterDirection.Output;
              cmd.Parameters.Add(outputParam);
              con.Open();
              cmd.ExecuteNonQuery();
              con.Close();
              int procedureResult = Convert.ToInt32(outputParam.Value);
              return procedureResult.ToString();
          }*/
       /* public string GetExcelValuesExist(string ClientName, string VesselName, string ManualName)
        {
            connection();

            SqlCommand cmd = new SqlCommand("CheckExcelDatasExistence", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientName", ClientName);
            cmd.Parameters.AddWithValue("@VesselName", VesselName);
            cmd.Parameters.AddWithValue("@ManualName", ManualName);

            // Create an output parameter
            SqlParameter outputParameter = new SqlParameter();
            outputParameter.ParameterName = "@OutputValue";
            outputParameter.SqlDbType = SqlDbType.NVarChar;
            outputParameter.Direction = ParameterDirection.Output;
            outputParameter.Size = 50;
            cmd.Parameters.Add(outputParameter);

            con.Open();
            cmd.ExecuteNonQuery();

            // Access the output parameter value
            string outputValue = outputParameter.Value.ToString();

            con.Close();

            return outputValue;
        }*/

        public string GetExcelValuesExist(string ClientName, string VesselName, string ManualName)
        {
            connection();

            SqlCommand cmd = new SqlCommand("CheckExcelDatasExistence", con);
          
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ClientName", ClientName);
            cmd.Parameters.AddWithValue("@VesselName", VesselName);
            cmd.Parameters.AddWithValue("@ManualName", ManualName + ".pdf");


            con.Open();

            int result = (int)cmd.ExecuteScalar();
            con.Close();

            if (result == 1)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

    }
}

