using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using System.Text.RegularExpressions;

namespace PMS_Data_Extraction_from_Machinery_Manual.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
      /*  public IActionResult UploadExcel()
        {
            return View();
        }
      */
    }
}
