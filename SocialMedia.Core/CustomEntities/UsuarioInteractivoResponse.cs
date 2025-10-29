using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.CustomEntities
{
    public class UsuarioInteractivoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Dapper mapeará 'cantidadDeUsuariosDiferentes' a esta propiedad (no distingue may/min)
        public int CantidadDeUsuariosDiferentes { get; set; }
    }
}
