using CurrencyReader.Models;
using System.Web.Mvc;

namespace CurrencyReader.Controllers
{
    public class CurrencyController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpPost]
        public ActionResult Convert(CurrencyConverter model)
        {
            return Json(model.Convert());
        }
    }
}