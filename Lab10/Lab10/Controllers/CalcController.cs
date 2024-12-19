using Lab10.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Lab10.Controllers
{
    public class CalcController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(OperandsModel model)
        {
            var cleanedX = model.X.Replace(",", ".");
            double x;
            if (double.TryParse(cleanedX, NumberStyles.Any, CultureInfo.InvariantCulture, out x))
            {
                var result = 5 * Math.Pow((x - 3), 6) - 4 * Math.Pow((x - 8), 4) + 42;
                ViewBag.Result = $"Значення функції: {result}";
            }
            else
            {
                ViewBag.Result = "Помилка: введене значення некоректне.";
            }

            return View();
        }
    }
}
