using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROYECTO3.Models.ViewModels
{
    public class TablaViewModel
    {
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        [Display (Name="Nombre"  )]
        public string nombre { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Correo electronico")]
        public string correo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime fecha_nacimiento { get; set; }
    }
}