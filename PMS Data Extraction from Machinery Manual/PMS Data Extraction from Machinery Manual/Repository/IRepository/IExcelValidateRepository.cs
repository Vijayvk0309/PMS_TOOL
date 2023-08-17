using PMS_Data_Extraction_from_Machinery_Manual.Models;

namespace PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository
{
    public interface IExcelValidateRepository
    {
        // Excel Upload Post
        public string ExcelData(ExcelValidate data);
        List<ExcelValidate> GetExcelValues();
        public string GetExcelValuesExist(string ClientName, string VesselName, string ManualName);
        List<ExcelValidate> GetPdfFileById(int id);
    }
}
