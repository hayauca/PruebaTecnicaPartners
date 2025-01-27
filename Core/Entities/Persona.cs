using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Persona
    {
        public int identificador { get; set; }
        public string nombres { get; set; } = string.Empty;
        public string apellidos { get; set; } = string.Empty;
        public string numeroIdentificacion { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string tipoIdentificacion { get; set; } = string.Empty;
        public DateTime fechaCreacion { get; set; }

       


    }
}
