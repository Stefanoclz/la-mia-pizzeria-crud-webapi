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

namespace la_mia_pizzeria_static.Controllers
{

    [Authorize]
    public class PizzaController : Controller
    {

        private InMemoryPizzaRepository pizzaRepository;

        // GET: HomeController1

        public PizzaController()
        {
            this.pizzaRepository = new InMemoryPizzaRepository();
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<PizzaCategory> pizze = pizzaRepository.GetList();
            return View("Index", pizze);
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            using(PizzaContext context = new PizzaContext())
            {
                Pizza pizzaFound = pizzaRepository.GetById(id);
                //context.Entry(pizzaFound).Collection("listaIngredienti").Load();
                if (pizzaFound == null)
                {
                    return NotFound($"La pizza con id {id} non è stata trovata");
                }
                else
                {
                    PizzaCategory pizzaCat = new PizzaCategory();
                    
                    pizzaCat.Pizza = pizzaFound;

                    List<string> listaIngr = new List<string>();

                    foreach (Ingrediente str in pizzaFound.listaIngredienti)
                    {
                        listaIngr.Add(str.Name);
                    }

                    pizzaCat.IngredientiSelezionati = listaIngr;

                    pizzaCat.Categories = context.Category.Where(c => c.Id == pizzaFound.CategoryId).ToList();
                    return View(pizzaCat);
                }
            }
            
        }


        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext context = new PizzaContext())
            {
                List<Category> categories = context.Category.ToList();
                List<Ingrediente> ingredienti = context.Ingrediente.ToList();
                PizzaCategory model = new PizzaCategory();
                model.Categories = categories;
                model.Pizza = new Pizza();

                List<SelectListItem> ingredientiList = new List<SelectListItem>();

                foreach (Ingrediente ingrediente in ingredienti)
                {
                    ingredientiList.Add(new SelectListItem() { Text = ingrediente.Name, Value = ingrediente.Id.ToString() });
                }

                model.Ingredienti = ingredientiList;

                return View(model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [MoreThanFiveWordsValidationAttribute]
        public ActionResult Create(PizzaCategory model)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext db = new PizzaContext())
                {
                    List<Ingrediente> listaIngr = new List<Ingrediente>();

                    foreach (string str in model.IngredientiSelezionati)
                    {
                        listaIngr.Add(db.Ingrediente.Where(ingr => ingr.Id == int.Parse(str)).First());
                    }

                    model.Pizza.listaIngredienti = listaIngr;
                    return View("Create", model);
                }
                    
            }

            Pizza pizzaToCreate = new Pizza();
            pizzaToCreate.name = model.Pizza.name;
            pizzaToCreate.description = model.Pizza.description;
            pizzaToCreate.prezzo = model.Pizza.prezzo;
            pizzaToCreate.fotoLink = model.Pizza.fotoLink;
            pizzaToCreate.CategoryId = model.Pizza.CategoryId;

            pizzaRepository.Create(pizzaToCreate, model.IngredientiSelezionati);
            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                // aggiungiamo include perchè vogliamo caricare anche i Tag del Post
                Pizza pizzaToEdit = pizzaRepository.GetById(id);
                if (pizzaToEdit == null)
                {
                    return NotFound();
                }
                else
                {
                    List<Category> categories = context.Category.ToList();
                    PizzaCategory model = new PizzaCategory();
                    model.Pizza = pizzaToEdit;
                    model.Categories = categories;
                    List<Ingrediente> ingredients = context.Ingrediente.ToList();
                    List<SelectListItem> IngredientsList = new List<SelectListItem>();
                    foreach (Ingrediente ingr in ingredients)
                    {
                        IngredientsList.Add(
                        new SelectListItem()
                        {
                            Text = ingr.Name,
                            Value = ingr.Id.ToString(),
                            // dobbiamo settare come selezionati i tag che sono presenti nel Post
                            Selected = pizzaToEdit.listaIngredienti.Any(m => m.Id == ingr.Id)
                        });
                    }
                    model.Ingredienti = IngredientsList;
                    return View(model);
                }
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PizzaCategory model)
        {
            if (!ModelState.IsValid)
            {
                using (PizzaContext context = new PizzaContext())
                {
                    // In caso di errore di validazione dobbiamo restituire ogni volta il model
                    // popolato con la lista delle categorie, perchè questi dati non vengono
                    // passati dal post e restituendolo direttamente la property Categories
                    // sarebbe null e la pagina darebbe errore
                    List<Category> categories = context.Category.ToList();
                    model.Categories = categories;
                    List<Ingrediente> ingredients = context.Ingrediente.ToList();
                    List<SelectListItem> IngredientsList = new List<SelectListItem>();
                    foreach (Ingrediente ingr in ingredients)
                    {
                        IngredientsList.Add(new SelectListItem() { Text = ingr.Name, Value = ingr.Id.ToString() });
                    }
                    model.Ingredienti = IngredientsList;
                    return View("Edit", model);
                }
            }
            // dobbiamo caricare anche i Tag collegati al post
            Pizza pizzaToEdit = pizzaRepository.GetById(id);
            if (pizzaToEdit != null)
            {
                // aggiorniamo i campi con i nuovi valori
                pizzaToEdit.name = model.Pizza.name;
                pizzaToEdit.description = model.Pizza.description;
                pizzaToEdit.prezzo = model.Pizza.prezzo;
                pizzaToEdit.fotoLink = model.Pizza.fotoLink;
                pizzaToEdit.CategoryId = model.Pizza.CategoryId;
                pizzaRepository.Update(pizzaToEdit, model.IngredientiSelezionati);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult FeedBack()
        {


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza pizzaToDelete = pizzaRepository.GetById(id);
            if (pizzaToDelete != null)
            {
                pizzaRepository.Delete(pizzaToDelete);
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }
    }

}

