using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.api
{
    [Route("Api/Pizzas")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            PizzaContext db = new PizzaContext();

            List<Pizza> pizzas = db.Pizza.ToList();

            return Ok(pizzas);
        }
    }
}
