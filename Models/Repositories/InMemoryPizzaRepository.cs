
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
    public class InMemoryPizzaRepository
    {
        private static List<PizzaCategory> Pizze = new List<PizzaCategory>();
        public void Create(Pizza pizza, List<string> selectedIngr)
        {
            pizza.id = Pizze.Count;
            
            pizza.listaIngredienti = new List<Ingrediente>();

            foreach(string ingr in selectedIngr)
            {
                Ingrediente nuovo = new Ingrediente();
                nuovo.Name = ingr;
                pizza.listaIngredienti.Add(nuovo);
            }

            PizzaCategory pizzaCat = new PizzaCategory();
            pizzaCat.Pizza = pizza;

            InMemoryPizzaRepository.Pizze.Add(pizzaCat);
        }

        public void Delete(Pizza pizza)
        {
            int indicePostDaEliminare = -1;
            for (int i = 0; i < InMemoryPizzaRepository.Pizze.Count; i++)
            {
                Pizza pizzaToCheck = InMemoryPizzaRepository.Pizze[i].Pizza;
                if (pizzaToCheck.id == pizza.id)
                {
                    indicePostDaEliminare = i;
                }
            }
            if (indicePostDaEliminare != -1)
            {
                InMemoryPizzaRepository.Pizze.RemoveAt(indicePostDaEliminare);
            }
        }

        public PizzaCategory GetById(int id)
        {
            Pizza pizzaDaTrovare = null;
            for (int i = 0; i < InMemoryPizzaRepository.Pizze.Count; i++)
            {
                Pizza pizzaToCheck = InMemoryPizzaRepository.Pizze[i];
                if (pizzaToCheck.id == id)
                {
                    pizzaDaTrovare = pizzaToCheck;
                }
            }
            return pizzaDaTrovare;
        }



        public List<PizzaCategory> GetList()
        {
            return InMemoryPizzaRepository.Pizze;
        }



        public void Update(Pizza pizza, List<string> selectedIngr)
        {
            int indicePostDaTrovare = -1;
            for (int i = 0; i < InMemoryPizzaRepository.Pizze.Count; i++)
            {
                Pizza postToCheck = InMemoryPizzaRepository.Pizze[i];
                if (postToCheck.id == pizza.id)
                {
                    indicePostDaTrovare = i;
                }
            }
            if (indicePostDaTrovare != -1)
            {
                InMemoryPizzaRepository.Pizze[indicePostDaTrovare] = pizza;
            }
        }
    }
}
