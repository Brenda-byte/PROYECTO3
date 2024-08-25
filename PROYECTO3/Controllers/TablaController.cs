using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PROYECTO3.Models;
using PROYECTO3.Models.ViewModels;

namespace PROYECTO3.Controllers
{
    public class TablaController : Controller
    {
        // GET: Tabla
        public ActionResult Index()
        {
            List<ListTablaViewModel> lst;
            using (CRUDEntities4 db = new CRUDEntities4())
            {
                lst = (from d in db.Tablas
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
                    using (CRUDEntities4 db = new CRUDEntities4())
                    {
                        var oTabla = new Tabla
                        {
                            correo = model.correo,
                            fecha_nacimiento = model.fecha_nacimiento,
                            nombre = model.nombre
                        };

                        db.Tablas.Add(oTabla);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index"); // Redirige a la acción Index
                }

                // Si el modelo no es válido, regresa la vista con el modelo para mostrar los errores
                return View(model);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error usando un sistema de logging, o mostrar un mensaje de error genérico
                // Por ejemplo, puedes usar el siguiente para fines de desarrollo:
                // ViewBag.ErrorMessage = ex.Message;
                // return View(model);
                throw; // Mejor lanzar la excepción para no ocultar errores en producción
            }
        }
    }
}
