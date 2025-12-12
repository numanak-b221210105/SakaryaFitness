using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SakaryaFitnessApp.Models;

namespace SakaryaFitnessApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Hem site ilk açıldığında ("") hem de "/AnaSayfa" yazıldığında burası çalışır
    [Route("")]
    [Route("AnaSayfa")]
    public IActionResult Index()
    {
        return View();
    }

    // Adres çubuğunda "/Gizlilik" yazınca burası çalışır
    [Route("Gizlilik")]
    public IActionResult Privacy()
    {
        return View();
    }

    // EKLENEN KISIM: API Test sayfasını açmak için gerekli metod
    // Adres çubuğunda "/Home/ApiTest" yazınca burası çalışır
    public IActionResult ApiTest()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}