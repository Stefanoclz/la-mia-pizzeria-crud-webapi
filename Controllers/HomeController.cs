using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
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
            return View();  
        }

        public IActionResult ChiSiamo()
        {
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Pizze()
        {
            PizzaContext context = new PizzaContext();
            List<Pizza> listaPizze = context.Pizza.ToList();
            List<PizzaCategory> listaPizzeCat = new List<PizzaCategory>();
            foreach (Pizza pizza in listaPizze)
            {
                PizzaCategory pizzaCategory = new PizzaCategory();
                pizzaCategory.Pizza = pizza;
                pizzaCategory.Categories = context.Category.Where(c => c.Id == pizza.CategoryId).ToList();
                listaPizzeCat.Add(pizzaCategory);
            }
            return View(listaPizzeCat);
        }


        public IActionResult Detail(int id)
        {


            using (PizzaContext context = new PizzaContext())
            {
                Pizza singola = context.Pizza.Where(singola => singola.id == id).FirstOrDefault();
                if (singola == null)
                {
                    return NotFound($"La Pizza con id {id} non è stata trovata");

                }
                else
                {
                    PizzaCategory pizzaCat = new PizzaCategory();
                    context.Entry(singola).Collection("listaIngredienti").Load();
                    pizzaCat.Pizza = singola;

                    List<string> listaIngr = new List<string>();

                    foreach (Ingrediente str in singola.listaIngredienti)
                    {
                        listaIngr.Add(str.Name);
                    }

                    pizzaCat.IngredientiSelezionati = listaIngr;

                    pizzaCat.Categories = context.Category.Where(c => c.Id == singola.CategoryId).ToList();
                    return View(pizzaCat);
                }
            }

        }
    }
}