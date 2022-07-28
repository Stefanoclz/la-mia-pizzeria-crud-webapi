using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        public Message()
        {

        }
    }
}
