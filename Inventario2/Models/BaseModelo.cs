using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Routing;

namespace Inventario2.Models
{
    public class BaseModelo
    {
        public int ActualPage { get; set; }
        public int Total { get; set; }
        public int RecordsPage { get; set; }
        public RouteValueDictionary ValoresQueryString { get; set; }

    }
}