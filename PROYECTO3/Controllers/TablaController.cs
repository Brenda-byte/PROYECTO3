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
        private readonly CRUDEntities4 _db;

        // Constructor
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
                            fecha_nacimiento = model.Fecha_nacimiento, // Asegúrate de que el nombre del campo coincide
                            nombre = model.nombre
                        };

                        _db.Tablas.Add(oTabla);
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
                Log.Error(ex, "Error al guardar el nuevo registro.");

                // Mostrar un mensaje genérico al usuario
                ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde.";

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
                    Fecha_nacimiento = oTabla.fecha_nacimiento, // Asegúrate de que el nombre del campo coincide
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
                        var oTabla = _db.Tablas.Find(model.id); // Buscar el registro existente
                        if (oTabla == null)
                        {
                            return HttpNotFound("Registro no encontrado.");
                        }

                        // Actualizar las propiedades del registro existente
                        oTabla.correo = model.correo;
                        oTabla.fecha_nacimiento = model.Fecha_nacimiento; // Asegúrate de que el nombre del campo coincide
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
    }
}
