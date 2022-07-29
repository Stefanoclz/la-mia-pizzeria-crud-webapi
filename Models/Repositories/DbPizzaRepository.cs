using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using la_mia_pizzeria_static.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using la_mia_pizzeria_static.Models.Repositories;

namespace la_mia_pizzeria_static.Models.Repositories
{
    public class DbPizzaRepository
    {

        public List<PizzaCategory> GetList()
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
            return listaPizzeCat;
        }

        // GET: HomeController1/Details/5
        public Pizza GetById(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza singola = context.Pizza.Where(p => p.id == id).FirstOrDefault();

                return singola;
                
            }

        }

        // GET: HomeController1/Create
        public void Create(Pizza pizza, List<string> ingr)
        {
            using(PizzaContext context = new PizzaContext())
            {
                List<Ingrediente> listaIngr = new List<Ingrediente>();

                foreach(string ingrItem in ingr)
                {
                    Ingrediente ingr = context.Ingrediente.Where(i => i.Name == ingrItem).FirstOrDefault();
                    listaIngr.Add(ingr);
                }

                pizza.listaIngredienti = listaIngr;

                context.Pizza.Add(pizza);
                context.SaveChanges();

            }
        }
      


        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Update(Pizza pizza, List<SelectListItem> SelectedIngr)
        {
            using(PizzaContext db = new PizzaContext())
            {

                List<Ingrediente> ListaIngr = new List<Ingrediente>();

                foreach(SelectListItem item in SelectedIngr)
                {
                    Ingrediente find = db.Ingrediente.Where(i => i.Id == int.Parse(item.Value)).FirstOrDefault();
                    if(find != null)
                    {
                        ListaIngr.Add(find);
                    }                  
                }
                pizza.listaIngredienti = ListaIngr;

                db.Pizza.Update(pizza);
                db.SaveChanges();
            }
        }

        // GET: HomeController1/Delete/5
        /*public ActionResult Delete(int id)
        {
            return View();
        }*/

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Delete(Pizza pizza)
        {
            using (PizzaContext context = new PizzaContext())
            {
                
                context.Pizza.Remove(pizza);
                context.SaveChanges();

            }
        }
    }
}
