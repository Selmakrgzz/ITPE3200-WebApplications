//Importerer nødvendige bibloteker i koden for å jobbe med b.la controller og IActionResult
using Microsoft.AspNetCore.Mvc;

//Her opprettes et namespace kalt MyShop.Controller, som indikerer at dette er en del av kontrollerne
//i applikasjonen MyShop. Brukes for å organisere klasser i større applikasjoner
namespace MyShop.Controllers
{
    //HomeController kan håndtere HTTP-forespørsler og returnere HTTP-responser
    public class HomeController : Controller
    {
        //GET: /<controller>/
        //Index() metoden håndterer GET-forespørselen som standard
        public IActionResult Index()
        {
            //View() metoden vil returnere en HTML-side som svar. Den vil da lete etter en 
            //Razor fil som heter Index.cshtml i mappen Views/Home/
            return View();
        }
    }
}