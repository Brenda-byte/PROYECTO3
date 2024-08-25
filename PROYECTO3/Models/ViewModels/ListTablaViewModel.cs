using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROYECTO3.Models.ViewModels
{
    public class ListTablaViewModel
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public DateTime fecha_nacimiento { get; set; }


    }
}