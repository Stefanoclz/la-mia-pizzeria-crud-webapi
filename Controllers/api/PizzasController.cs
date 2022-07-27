using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


    }
}
