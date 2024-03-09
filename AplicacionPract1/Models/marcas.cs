using System.ComponentModel.DataAnnotations;
namespace AplicacionPract1.Models
{
    public class marcas
    {

        [Key]

        public int id_marcas { get; set; }

        public string nombre_marcas { get; set; }

        public string estado { get; set; }

    }
}
