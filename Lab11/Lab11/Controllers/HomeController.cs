using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab11.Controllers
{
    public class HomeController : Controller
    {
        private PhoneDBEntities db = new PhoneDBEntities();
        public ActionResult Index()
        {
            var brands = (from brand in db.Brand select brand).ToList();

            return View(brands);
        }

        public ActionResult Details(int id)
        {
            var price = (from barndPrice in db.BrandPrice where barndPrice.BrandID == id select barndPrice.Price).First();

            return View(price);
        }

        [HttpGet]
        public ActionResult Create()
        {
            Brand brand = new Brand();
            return View(brand);
        }


        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Brand.Add(brand);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, ex);
            }
            return View(brand);
        }

        [HttpGet]
        public ActionResult Edit(int id) 
        {
            var brandEdit = (from brand in db.Brand where brand.BrandID == id select brand).First();
            return View(brandEdit);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var brandEdit = (from brand in db.Brand where brand.BrandID == id select brand).First();

            try
            {
                UpdateModel(brandEdit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch 
            {
                return View(brandEdit);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var brandEdit = (from brand in db.Brand where brand.BrandID == id select brand).First();
            return View(brandEdit);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var brandDelete = (from brand in db.Brand where brand.BrandID == id select brand).First();

            try
            {
                db.Brand.Remove(brandDelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(brandDelete);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}