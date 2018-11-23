using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeesManagementSystem_MVC.Controllers
{
    public class HomeController : Controller
    {
        public string username = "user";
        public string userPassword = "123";

        // GET: Home
        public ActionResult Index()
        {
            using (var context = new DB_EmployeesEntities())
            {
                List<Employees> empList = context.Employees.Where(p => p.IsValid == "Y").ToList();
                return View(empList);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string lUsername = fc["username"];
            string lUserPassword = fc["userPassword"];

            if (lUsername == username && lUserPassword == userPassword)
            {
                Session["yetki"] = lUsername;
                return Redirect("/Home/Index");
            }

            else
                return Redirect("/Home/Login");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection fc)
        {
            using (var context = new DB_EmployeesEntities())
            {
                var emp = new Employees()
                {
                    FirstName = fc["firstName"],
                    LastName = fc["lastName"],
                    Title = fc["title"],
                    TitleOfCourtesy = fc["titleOfCourtesy"],
                    City = fc["city"],
                    DateOfRegistration = DateTime.Now,
                    UserName = Session["yetki"].ToString(),
                    UserTrnx = "A",
                    IsValid = "Y"
                };

                context.Employees.Add(emp);
                context.SaveChanges();

                return Redirect("/Home/Index");
            }
        }

        public ActionResult Edit(int ID)
        {
            using (var context = new DB_EmployeesEntities())
            {
                var emp = context.Employees.Where(p => p.EmployeeID == ID).FirstOrDefault();
                return View(emp);
            }
        }

        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {
            int ID = int.Parse(fc["ID"]);

            using (var context = new DB_EmployeesEntities())
            {
                var emp = context.Employees.Where(p => p.EmployeeID == ID).FirstOrDefault();

                emp.FirstName = fc["firstName"];
                emp.LastName = fc["lastName"];
                emp.Title = fc["title"];
                emp.TitleOfCourtesy = fc["titleOfCourtesy"];
                emp.City = fc["city"];
                emp.DateOfUpdate = DateTime.Now;
                emp.UserName = Session["yetki"].ToString();
                emp.UserTrnx = "U";
                emp.IsValid = "Y";

                context.SaveChanges();

                return Redirect("/Home/Index");
            }
        }

        public ActionResult Delete(int ID)
        {
            using (var context = new DB_EmployeesEntities())
            {
                var emp = context.Employees.Where(p => p.EmployeeID == ID).FirstOrDefault();
                
                emp.DateOfUpdate = DateTime.Now;
                emp.UserName = Session["yetki"].ToString();
                emp.UserTrnx = "D";
                emp.IsValid = "N";

                context.SaveChanges();

                return Redirect("/Home/Index");
            }
        }
    }
}