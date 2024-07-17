using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;  // Ensure you have the correct namespace for your models

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        // GET: Insuree
        public ActionResult Index()
        {
            // Retrieve and display all records
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insurees = db.Insurees.ToList();
                return View(insurees);
            }
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int id)
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insuree = db.Insurees.Find(id);
                return View(insuree);
            }
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        public ActionResult Create(Insuree insuree)
        {
            try
            {
                // Calculate quote based on the provided information
                insuree.Quote = CalculateQuote(insuree);

                // Save to database
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    db.Insurees.Add(insuree);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int id)
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insuree = db.Insurees.Find(id);
                return View(insuree);
            }
        }

        // POST: Insuree/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Insuree insuree)
        {
            try
            {
                // Update quote based on edited information
                insuree.Quote = CalculateQuote(insuree);

                // Update database
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    db.Entry(insuree).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int id)
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insuree = db.Insurees.Find(id);
                return View(insuree);
            }
        }

        // POST: Insuree/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (InsuranceEntities db = new InsuranceEntities())
                {
                    var insuree = db.Insurees.Find(id);
                    db.Insurees.Remove(insuree);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            using (InsuranceEntities db = new InsuranceEntities())
            {
                var insurees = db.Insurees.ToList(); // Assuming Insurees is your DbSet in ApplicationDbContext
                return View(insurees);
            }
        }

        // Method to calculate quote based on provided information
        private decimal CalculateQuote(Insuree insuree)
        {
            decimal quote = 50;  // Base price

            // Age adjustments
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (age <= 18)
                quote += 100;
            else if (age >= 19 && age <= 25)
                quote += 50;
            else
                quote += 25;

            // Car year adjustments
            if (insuree.CarYear < 2000)
                quote += 25;
            else if (insuree.CarYear > 2015)
                quote += 25;

            // Car make adjustments
            if (insuree.CarMake.ToLower() == "porsche")
            {
                quote += 25;

                // Specific model adjustments
                if (insuree.CarModel.ToLower() == "911 carrera")
                    quote += 25;
            }

            // Speeding tickets adjustment
            quote += 10 * insuree.SpeedingTickets;

            // DUI adjustment
            if (insuree.DUI)
                quote *= 1.25m;  // 25% increase

            // Coverage type adjustment (assuming this is not provided in the form)
            // if (insuree.CoverageType == "full")
            //     quote *= 1.5m;  // 50% increase

            return quote;
        }
    }
}
