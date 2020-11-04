using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazasPerros.Models.ViewModels
{
    public class RazaAdminViewModel
    {
        public Razas Raza { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
    }
}
