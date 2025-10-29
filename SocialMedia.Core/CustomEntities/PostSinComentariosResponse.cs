using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.CustomEntities
{
    public class PostSinComentariosResponse
    {
        public int IdPost { get; set; }
        public string PostDescription { get; set; } // Corregido de [Post Description]
        public DateTime PostDate { get; set; }     // Corregido de [Post Date]
    }
}
