using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Pizza> listaPizze { get; set; }
    public Category()
    {

    }
}

