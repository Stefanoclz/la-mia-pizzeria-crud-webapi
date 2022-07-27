using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers.api
{
    [Route("Api/Pizzas")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string? search)
        {
            PizzaContext db = new PizzaContext();

            List<Pizza> pizzas = db.Pizza.ToList();

            if (search != null && search != "")
            {
                pizzas = db.Pizza.Where(p => p.name.Contains(search)).ToList();
            }

            return Ok(pizzas);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
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

                    return Ok(pizzaCat);

                }

            }
        }


    }
}
