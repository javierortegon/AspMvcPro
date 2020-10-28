
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventario2.Models;

namespace Inventario2.Models
{
    public class IndexModel:BaseModelo
    {
        public List<usuario> Personas { get; set; }

    }
}