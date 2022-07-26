using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_static.Controllers.api
{
    [Route("Api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            PizzaContext db = new PizzaContext();

            IQueryable<Pizza> pizzas = db.Pizza;

            return Ok(pizzas.ToList());
        }
    }
}
