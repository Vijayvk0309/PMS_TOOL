using System.ComponentModel.DataAnnotations;

namespace PMS_Data_Extraction_from_Machinery_Manual.Models
{
    public class ExcelValidate
    {
        [Key]
        public int ExcelId { get; set; }
        public string ClientName { get; set; }
        public string VesselName { get; set; }
        public string ManualPath { get; set; }
        public string ManualName { get; set; }
        public int TotalPages { get; set; }
        public string EquipmentName { get; set; }
        public string Maker { get; set; }
        public string ModelType { get; set; }
        public string NoOfUnit { get; set; }

        public string SparePageNo { get; set; }
        public string JobPageNo { get; set; }
        public string TechnicalData { get; set; }
        public string Remarks { get; set; }
        public string ClientNameErrorMessage { get; set; }
        public string VesselNameErrorMessage { get; set; }
        public string ManualPathErrorMessage { get; set; }
        public string ManualNameErrorMessage { get; set; }
        public string TotalPagesErrorMessage { get; set; }
        public string EquipmentNameErrorMessage { get; set; }
        public string MakerErrorMessage { get; set; }
        public string ModelTypeErrorMessage { get; set; }
        public string NoOfUnitErrorMessage { get; set; }
        public string SparePageNoErrorMessage { get; set; }
        public string JobPageNoErrorMessage { get; set; }
        public string TechnicalDataErrorMessage { get; set; }
        public string RemarksErrorMessage { get; set; }

        public string FilePath { get; set; }
        public string OutputPath { get; set; }

    }
}
