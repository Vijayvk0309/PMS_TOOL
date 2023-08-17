using CsvHelper;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Formats.Asn1;
using System.Globalization;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using iTextSharp.text;
using static OfficeOpenXml.ExcelErrorValue;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace PMS_Data_Extraction_from_Machinery_Manual.Controllers
{
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;
        private readonly ILogin _loginUser;
        private readonly IExcelValidateRepository _context;

        public MainController(ILogger<MainController> logger, ILogin loguser, IExcelValidateRepository CreateExcel)
        {
            _loginUser = loguser;
            _logger = logger;
            _context = CreateExcel;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult User()
        {
            return View();
        }
        public IActionResult AnalysisSheet()
        {
            var ClientNames = _context.GetExcelValues().Select(l => l.ClientName).Distinct().ToList();
            ViewBag.ClientNames = new SelectList(ClientNames);

            return View();
        }

        public JsonResult GetVesselNames(string ClientName)
        {
            var VesselNames = _context.GetExcelValues()
                .Where(l => l.ClientName == ClientName)
                .Select(l => l.VesselName)
                .Distinct()
                .ToList();

            return Json(VesselNames);
        }

        public JsonResult GetEquipmentNames(string ClientName, string VesselName)
        {
            var EquipmentNames = _context.GetExcelValues()
                .Where(l => l.ClientName == ClientName && l.VesselName == VesselName)
                .Select(l => l.EquipmentName)
                .Distinct()
                .ToList();

            return Json(EquipmentNames);
        }
        public JsonResult GetAllDetails(string ClientName, string VesselName, string EquipmentName)
        {
            var AllDetailsGet = _context.GetExcelValues()
                .Where(l => l.ClientName == ClientName && l.VesselName == VesselName && l.EquipmentName == EquipmentName)
              /*  .Select(l => l.EquipmentName)*/
                .ToList();

            return Json(AllDetailsGet);
        }
        /* public IActionResult GetReport()
         {
             return View("File");
         }*/
        [HttpPost]
        public IActionResult Index(string username, string passcode)
        {
            var issuccess = _loginUser.AuthenticateUser(username, passcode);


            if (issuccess.Result != null)
            {
                ViewBag.username = string.Format("Successfully logged-in", username);
                if (issuccess.Result.Access == "Admin")
                {
                    return RedirectToAction("Admin");
                }
                else
                {
                    return RedirectToAction("User");
                }
            }
            else
            {
                ViewBag.username = string.Format("Login Failed - Check Your UserName or Password ", username);
                return View();
            }
        }
        private bool ContainsSpecialCharacters(string text)
        {
            return text.Any(c => c == '/' || c == '.' || c == '-');
        }
      /*  public bool CheckIfRecordExists(string clientName, string vesselName, string manualName)
        {
            bool exists = _context.GetExcelValues.Any(data => data.ClientName == clientName && data.VesselName == vesselName && data.ManualName == manualName);

            return exists;
        }*/

        private bool HasError(ExcelValidate rowData)
        {
            return !string.IsNullOrEmpty(rowData.ClientNameErrorMessage)
                 || !string.IsNullOrEmpty(rowData.VesselNameErrorMessage)
                 || !string.IsNullOrEmpty(rowData.ManualPathErrorMessage)
                 || !string.IsNullOrEmpty(rowData.ManualNameErrorMessage)
                 || !string.IsNullOrEmpty(rowData.TotalPagesErrorMessage)
                 || !string.IsNullOrEmpty(rowData.EquipmentNameErrorMessage)
                 || !string.IsNullOrEmpty(rowData.MakerErrorMessage)
                 || !string.IsNullOrEmpty(rowData.ModelTypeErrorMessage)
                 || !string.IsNullOrEmpty(rowData.NoOfUnitErrorMessage)
                 || !string.IsNullOrEmpty(rowData.SparePageNoErrorMessage)
                 || !string.IsNullOrEmpty(rowData.JobPageNoErrorMessage)
                 || !string.IsNullOrEmpty(rowData.TechnicalDataErrorMessage)
                 || !string.IsNullOrEmpty(rowData.RemarksErrorMessage)
                ;
        }
        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.file = string.Format("Please select an Excel file.", file);
                return View("User", file);
            }
          
            // Read data from the Excel file and validate missing values
            var excelData = new List<ExcelValidate>();
            using (var package = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                for (int row = 2; row <= rowCount; row++) 
                {
                    var rowData = new ExcelValidate();
                    var isExist = false;

                    string ClientName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                    string VesselName = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                    string ManualName = worksheet.Cells[row, 4].Value?.ToString()?.Trim();

                    var existsInDatabase = _context.GetExcelValuesExist(ClientName, VesselName, ManualName);
                    if (existsInDatabase == "1")
                    {
                        isExist = true;
                        rowData.ClientNameErrorMessage = "Already exists";
                        rowData.VesselNameErrorMessage = "Already exists";
                        rowData.ManualNameErrorMessage = "Already exists";
                    }

               

                    for (int col = 1; col <= colCount; col++)
                    {
                        string cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                        
                        switch (col)
                        {                        
                            case 1:

                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.ClientNameErrorMessage = "Client Name is missing.";
                                }

                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.ClientNameErrorMessage = "Invalid custom page format";
                                }
                                else if (isExist == true)
                                {
                                    rowData.ClientNameErrorMessage = "Already exists";
                                }
                                else
                                {
                                    rowData.ClientName = cellValue;
                                }

                                break;
                            case 2:

                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.VesselNameErrorMessage = "Vessel Name is missing.";
                                }
                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.VesselNameErrorMessage = "Invalid custom page format";
                                }
                                else if (isExist == true)
                                {
                                    rowData.VesselNameErrorMessage = "Already exit";
                                }
                                else
                                {
                                    rowData.VesselName = cellValue;
                                }

                                break;
                            case 3:

                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.ManualPathErrorMessage = "Manual Path is missing.";
                                }
                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.ManualPathErrorMessage = "Invalid custom page format";

                                }
                                else
                                {
                                    rowData.ManualPath = cellValue;
                                }

                                break;
                            case 4:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.ManualNameErrorMessage = "ManualName is missing.";
                                }
                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.ManualNameErrorMessage = "Invalid custom page format";
                                }
                                else if (isExist == true)
                                {
                                    rowData.ManualNameErrorMessage = "Already exit";
                                }
                                else
                                {
                                    rowData.ManualName = cellValue;

                                }
                                break;
                            case 5:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.TotalPagesErrorMessage = "TotalPages is missing.";
                                }
                                else
                                {
                                    if (int.TryParse(cellValue, out int integerValue))
                                    {
                                        rowData.TotalPages = integerValue;
                                    }
                                    else
                                    {
                                        rowData.TotalPagesErrorMessage = "Integer value only allowed.";
                                    }
                                }

                                break;
                            case 6:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.EquipmentNameErrorMessage = "EquipmentName is missing.";
                                }
                                else
                                {
                                    rowData.EquipmentName = cellValue;

                                }

                                break;
                            case 7:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.MakerErrorMessage = "Maker is missing.";
                                }
                                else
                                {
                                    rowData.Maker = cellValue;

                                }

                                break;
                            case 8:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.ModelTypeErrorMessage = "ModelType is missing.";
                                }
                                else
                                {
                                    rowData.ModelType = cellValue;
                                }

                                break;
                            case 9:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.NoOfUnitErrorMessage = "NoOfUnit is missing.";
                                }
                                else
                                {
                                    rowData.NoOfUnit = cellValue;
                                }

                                break;
                            case 10:

                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.SparePageNoErrorMessage = "SparePageNo is missing.";
                                }
                                else if (Regex.IsMatch(cellValue, @"[a-zA-Z]") && Regex.IsMatch(cellValue, @"\d"))
                                {
                                    rowData.SparePageNoErrorMessage = "Invalid custom page format";
                                }
                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.SparePageNoErrorMessage = "Invalid custom page format";
                                }

                                else if (!Regex.IsMatch(cellValue, @"^[0-9,-]+$"))
                                {
                                    rowData.JobPageNoErrorMessage = "Invalid custom page format";
                                }

                                else
                                {                              
                                    string[] rangeStrings = cellValue.Trim().Split(',');
                                    List<string> processedValues = new List<string>();
                                    foreach (string rangeString in rangeStrings)
                                    {
                                        string[] rangeParts = rangeString.Split('-');
                                        if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                                        {
                                            var valuesInRange = Enumerable.Range(start, end - start + 1);
                                            processedValues.AddRange(valuesInRange.Select(v => v.ToString()));
                                        }
                                        else
                                        {
                                            processedValues.Add(rangeString);
                                        }
                                    }
                                    rowData.SparePageNo = string.Join(", ", processedValues);                             
                                }
                                break;
                            case 11:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.JobPageNoErrorMessage = "JobPageNo is missing.";
                                }
                                else if (Regex.IsMatch(cellValue, @"[a-zA-Z]") && Regex.IsMatch(cellValue, @"\d"))
                                {
                                    rowData.JobPageNoErrorMessage = "Invalid custom page format";
                                }
                                else if (Regex.IsMatch(cellValue, @"^[!@#$%^&*()_+\=\[\]{};':""\\|,.<>\/?]*$"))
                                {
                                    rowData.JobPageNoErrorMessage = "Invalid custom page format";
                                }

                                else if (!Regex.IsMatch(cellValue, @"^[0-9,-]+$"))
                                {
                                    rowData.JobPageNoErrorMessage = "Invalid custom page format";
                                }

                                else
                                {
                                    string[] rangeStrings = cellValue.Trim().Split(',');
                                    List<string> processedValues = new List<string>();
                                    foreach (string rangeString in rangeStrings)
                                    {
                                        string[] rangeParts = rangeString.Split('-');
                                        if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
                                        {
                                            var valuesInRange = Enumerable.Range(start, end - start + 1);
                                            processedValues.AddRange(valuesInRange.Select(v => v.ToString()));
                                        }
                                        else
                                        {
                                            processedValues.Add(rangeString);
                                        }
                                    }
                                    rowData.JobPageNo = string.Join(", ", processedValues);
                                }
                                break;                             
                            case 12:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.TechnicalDataErrorMessage = "TechnicalData is missing.";
                                }
                                else
                                {
                                    rowData.TechnicalData = cellValue;
                                }

                                break;
                            case 13:
                                if (string.IsNullOrEmpty(cellValue))
                                {
                                    rowData.RemarksErrorMessage = "Remarks is missing.";
                                }
                                else
                                {
                                    rowData.Remarks = cellValue;
                                }

                                break;
                        }
                    
                    }

                   

                    if (HasError(rowData))
                    {
                        excelData.Add(rowData);
                        var id = rowData.ExcelId;
                    }
                    else
                    {
                        _context.ExcelData(rowData);
                        TempData["SuccessMessage"] = "Data added successfully!";
                    }

                }


            }
            if (excelData.Count > 0)
            {
                return View("UploadExcel", excelData);
            }

            return RedirectToAction("AnalysisSheet");
        }
        [HttpGet]
        public IActionResult ManualAnalysisSheet()
        {
            List<ExcelValidate> userslist = new List<ExcelValidate>();
            userslist = _context.GetExcelValues().ToList();
            return View(userslist);
        }
        /*public IActionResult ViewPdf(int id)
        {
            var pdfFile = _context.GetPdfFileById(id);
            if (pdfFile == null)
            {
                return NotFound();
            }

            // Return the PDF file to the view
            return File(pdfFile.Content, "application/pdf");
        }*/

        public FileResult GetReport(string filePath)
        {          
            byte[] FileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(FileBytes, "application/pdf");
        }


        public IActionResult GetSparePagereport(string filePath, string sparePageNo,string ManualName)
        {

            var pdfReader = new PdfReader(filePath);

            try
            {
                pdfReader.SelectPages(sparePageNo);
                string OutputPath = "D:/PMS/Manuals/TechnicalManual/Process/";
              /*  string outputFileName = $"{ManualName+ "_Spare"}.pdf";*/
                string outputFileName = "Spare_"+ManualName;

                using (var fs = new FileStream(Path.Combine(OutputPath, outputFileName), FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = null;
                    try
                    {
                        stamper = new PdfStamper(pdfReader, fs);
                    }
                    finally
                    {
                        stamper?.Close();
                    }
                }             
                byte[] FileBytes = System.IO.File.ReadAllBytes(Path.Combine(OutputPath, outputFileName));
                return File(FileBytes, "application/pdf");
            }
            finally
            {
                pdfReader.Close();
            }
           
        }

        public IActionResult GetJobPagereport(string filePath, string JobPageNo)
        {

            var pdfReader = new PdfReader(filePath);

            try
            {
                pdfReader.SelectPages(JobPageNo);
                string OutputPath = "D:/PMS/Manuals/TechnicalManual/Process/";
                string outputFileName = $"JobPage_output_page.pdf";

                using (var fs = new FileStream(Path.Combine(OutputPath, outputFileName), FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = null;
                    try
                    {
                        stamper = new PdfStamper(pdfReader, fs);
                    }
                    finally
                    {
                        stamper?.Close();
                    }
                }

                byte[] FileBytes = System.IO.File.ReadAllBytes(Path.Combine(OutputPath, outputFileName));
                return File(FileBytes, "application/pdf");
            }
            finally
            {
                pdfReader.Close();
            }

        }


    }
}