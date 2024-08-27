using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PROYECTO3.Models;
using PROYECTO3.Models.ViewModels;
using Serilog; // Asegúrate de agregar el paquete y configurarlo

namespace PROYECTO3.Controllers
{
    public class TablaController : Controller
    {
        private readonly CRUDEntities4 _db;

        // Constructor con inyección de dependencias (mejor práctica)
        public TablaController()
        {
            _db = new CRUDEntities4();
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
                    using (CRUDEntities4 db = new CRUDEntities4())
                    {
                        var oTabla = new Tabla();
                        oTabla.correo = model.correo;
                        oTabla.fecha_nacimiento = model.fecha_nacimiento;
                        oTabla.nombre = model.nombre;

                        db.Tablas.Add(oTabla);
                        db.SaveChanges();
                    }

                    return Redirect("~/Tabla/Index");
                }

                return View(model);

            }
            catch (Exception ex)
            {
                // Registrar el error
                Log.Error(ex, "Error al guardar el nuevo registro.");

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde." + "\n"+ ex.Message;

                // Devolver la vista con el modelo
                return View(model);
            }
        }

        // GET: Tabla/Editar/5
        public ActionResult Editar(int id)
        {
            TablaViewModel model;
            using (_db)
            {
                var oTabla = _db.Tablas.Find(id);
                if (oTabla == null)
                {
                    return HttpNotFound("Registro no encontrado.");
                }

                model = new TablaViewModel
                {
                    nombre = oTabla.nombre,
                    correo = oTabla.correo,
                    fecha_nacimiento = oTabla.fecha_nacimiento,
                    id = oTabla.id
                };
            }
            return View(model);
        }

        // POST: Tabla/Editar
        [HttpPost]
        public ActionResult Editar(TablaViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (_db)
                    {
                        var oTabla = _db.Tablas.Find(model.id);
                        if (oTabla == null)
                        {
                            return HttpNotFound("Registro no encontrado.");
                        }

                        oTabla.correo = model.correo;
                        oTabla.fecha_nacimiento = model.fecha_nacimiento;
                        oTabla.nombre = model.nombre;

                        _db.Entry(oTabla).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                    return RedirectToAction("Index"); // Redirige a la acción Index
                }

                // Si el modelo no es válido, regresa la vista con el modelo para mostrar los errores
                return View(model);
            }
            catch (Exception ex)
            {
                // Registrar el error
                Log.Error(ex, "Error al guardar el registro.");

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";

                // Devolver la vista con el modelo
                return View(model);
            }
        }

        // GET: Tabla/Eliminar/5
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            try
            {
                using (_db)
                {
                    var oTabla = _db.Tablas.Find(id);
                    if (oTabla == null)
                    {
                        return HttpNotFound("Registro no encontrado.");
                    }

                    _db.Tablas.Remove(oTabla);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Registrar el error
                Log.Error(ex, "Error al eliminar el registro.");

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";
            }

            return RedirectToAction("Index");
        }
    }
}
