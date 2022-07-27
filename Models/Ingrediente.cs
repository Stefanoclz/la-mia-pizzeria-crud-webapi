using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Table("Ingredienti")]
public class Ingrediente
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<Pizza>? listaPizze { get; set; }

}

