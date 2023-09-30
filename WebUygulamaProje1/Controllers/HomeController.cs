using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(); //burada ki view Home klasörü içerisindeki İndex.cshtmli return eder ve gösterir. Biz meselea buraya tırnak içerisinde "test" yazarsak ve cshtml li de test.cshtlm yaparsak bu sefer bunu çağıracaktır.
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}