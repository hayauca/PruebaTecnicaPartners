using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Usuario
    {
        public int identificador { get; set; }
        public string usuarioNombre { get; set; } = string.Empty;
        public string pass { get; set; } = string.Empty;
        public DateTime fechaCreacion { get; set; }
    }
}
