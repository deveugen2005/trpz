using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Lab12.Controllers
{
    public class PhoneBrand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class PhonePrice
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
    }

    public class WApiController : ApiController
    {
        private PhoneDBEntities db = new PhoneDBEntities();

        [HttpGet]
        [ActionName("GetBrands")]
        public ICollection<PhoneBrand> GetBrands() 
        {
            var brands = (from brand in db.Brand select brand).ToList();

            Collection<PhoneBrand> PB = new Collection<PhoneBrand>();

            foreach (var brand in brands)
            {
                PB.Add(new PhoneBrand { ID = brand.BrandID, Name = brand.Name, Country = brand.Country });
            }

            return PB;
        }

        [HttpGet]
        [ActionName("GetPrice")]
        public ICollection<PhonePrice> GetPrice(int Id)
        {
            var prices = (from price in db.Price where price.PriceID == Id select price).ToList();

            Collection<PhonePrice> PP = new Collection<PhonePrice>();

            foreach (var price in prices)
            {
                PP.Add(new PhonePrice { ID = price.PriceID, Price = price.Price1, Currency = price.Currency });
            }

            return PP;
        }

        [HttpPost]
        [ActionName("CreateBrand")]
        public HttpResponseMessage CreateBrand(Brand brand)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);

            try
            {
                db.Brand.Add(brand);
                db.SaveChanges();
                response.Content = new StringContent("{Id: " + brand.BrandID + ",Name: " + brand.Name + ",Country: " + brand.Country + "}", 
                    Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent("{Error: "+ex.Message+"}", Encoding.UTF8, "application/json");
            }
            return response;
        }

        [HttpPost]
        [ActionName("UpdateBrand")]
        public HttpResponseMessage UpdateBrand(Brand brand)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            var br = (from b in db.Brand where b.BrandID == brand.BrandID select b).First();

            try
            {
                db.Brand.Remove(br);
                db.Brand.Add(brand);
                db.SaveChanges();
                response.Content = new StringContent("{Id: " + brand.BrandID + ",Name: " + brand.Name + ",Country: " + brand.Country + "}",
                    Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent("{Error: " + ex.Message + "}", Encoding.UTF8, "application/json");
            }
            return response;
        }

        [HttpPost]
        [ActionName("DeleteBrand")]
        public HttpResponseMessage DeleteBrand(Brand brand)
        {
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            var br = (from b in db.Brand where b.BrandID == brand.BrandID select b).First();

            try
            {
                db.Brand.Remove(br);
                db.SaveChanges();
                response.Content = new StringContent("{Id: " + brand.BrandID + ",Name: " + brand.Name + ",Country: " + brand.Country + "}",
                    Encoding.UTF8, "application/json");
            }
            catch (Exception ex)
            {
                response.Content = new StringContent("{Error: " + ex.Message + "}", Encoding.UTF8, "application/json");
            }
            return response;
        }
    }
}