using Microsoft.AspNetCore.Mvc;
using PMS_Data_Extraction_from_Machinery_Manual.Models;
using PMS_Data_Extraction_from_Machinery_Manual.Repository.IRepository;

namespace PMS_Data_Extraction_from_Machinery_Manual.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _context;
        public AdminController(DbContextClass context, IUserRepository createuser)
        {
            _context = createuser;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(User data)
        {
            string result = "";
            try
            {
                if (data.Id == 0)
                {
                    result = _context.InsertData(data);

                }
             /*   else
                {
                    result = _context.UpdateDetails(data);
                }*/
                TempData["ResultOk"] = result;
                return RedirectToAction("Table");

            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet]
        public IActionResult Table()
        {
            List<User> userslist = new List<User>();
            userslist = _context.GetServiceAsync().ToList();
            return View(userslist);
        }

        /*   public IActionResult EditUser(int id)
           {
               try
               {
                   var result = _context.GetRegister(id);
                   if (result == null)
                   {
                       return NotFound();
                   }
                   TempData["ResultOk"] = result;
                   return RedirectToAction("EditUser");

               }
               catch (Exception ex)
               {
                   return Ok(ex);
               }
           }*/
        // GET: Student/Edit/5
        public ActionResult EditUser(int id)
        {
            // Retrieve the user with the specified id from the database
            var user = _context.GetServiceAsync().Find(smodel => smodel.Id == id);

            if (user == null)
            {
                // Handle if user with the specified id is not found
                return RedirectToAction("Index"); // Or display an error message
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(User smodel)
        {
            try
            {
               
                _context.UpdateDetails(smodel);
                return RedirectToAction("Table");
            }
            catch
            {
                return View();
            }
        }

        /*     [HttpPost]
             public ActionResult EditUser(int id, User smodel)
             {

                 string result = "";
                 try
                 {

                     if (smodel.Id == 0)
                     {
                         result = _context.InsertData(smodel);
                         return RedirectToAction("Table");
                     }
                     else
                     {
                         result = _context.UpdateDetails(smodel);
                         return RedirectToAction("Table");
                     }
                 }

                 catch
                 {
                     return View(smodel);
                 }
             }*/


    }
}
