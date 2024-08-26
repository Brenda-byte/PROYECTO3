using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PROYECTO3.Models;
using PROYECTO3.Models.ViewModels;
using Serilog; // Asegúrate de agregar el paquete y configurarlo

namespace PROYECTO3.Controllers
{
    public class TablaController : Controller
    {
        // Asegúrate de inyectar el contexto si usas DI
        private readonly CRUDEntities4 _db;

        public TablaController()
        {
            _db = new CRUDEntities4(); // O usa la inyección de dependencias aquí
        }

        // GET: Tabla
        public ActionResult Index()
        {
            List<ListTablaViewModel> lst;
            using (_db)
            {
                lst = (from d in _db.Tablas
                       select new ListTablaViewModel
                       {
                           id = d.id,
                           nombre = d.nombre,
                           fecha_nacimiento = d.fecha_nacimiento.HasValue ? d.fecha_nacimiento.Value : default(DateTime),
                           correo = d.correo,
                       }).ToList();
            }
            return View(lst);
        }

        // GET: Tabla/Nuevo
        public ActionResult Nuevo()
        {
            return View();
        }

        // POST: Tabla/Nuevo
        [HttpPost]
        public ActionResult Nuevo(TablaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db)
                    {
                        var oTabla = new Tabla
                        {
                            correo = model.correo,
                            fecha_nacimiento = model.fecha_nacimiento,
                            nombre = model.nombre
                        };

                        _db.Tablas.Add(oTabla);
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Tabla"); // Redirige a la acción Index
                }

                // Si el modelo no es válido, regresa la vista con el modelo para mostrar los errores
                return View(model);
            }
            catch (Exception ex)
            {
                // Registrar el error
                Log.Error(ex, "Error al guardar el nuevo registro.");

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";

                // Devolver la vista con el modelo
                return View(model);
            }
        }
    }
}
