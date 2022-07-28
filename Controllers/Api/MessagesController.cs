using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static;
using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly PizzaContext _context;

        public MessagesController(PizzaContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Get()
        {
            PizzaContext db = new PizzaContext();

            List<Message> messages = new List<Message>();

            messages = db.Messages.ToList();

            return Ok(messages);
        }


        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult Post ([FromBody] Message message)
        {
            using(PizzaContext db = new PizzaContext())
            {
                db.Messages.Add(message);
                db.SaveChanges();
            }

            return Ok();
        }

     
    }
}
