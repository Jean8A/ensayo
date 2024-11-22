using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ENSAYO.Models;
using System.Diagnostics;

namespace ENSAYO.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly EnsayoContext _context;

        public HabitacionesController(EnsayoContext context)
        {
            _context = context;
        }

        // GET: Habitaciones
        public async Task<IActionResult> Index()
        {
            var ensayoContext = _context.Habitaciones.Include(h => h.IdTipoHabitacionNavigation)
                .Include(h => h.ImagenHabitacions)
                .ThenInclude(ih => ih.IdImagenNavigation);
               
            return View(await ensayoContext.ToListAsync());
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdTipoHabitacionNavigation)
                .Include(h => h.ImagenHabitacions)  // Asegúrate de cargar las imágenes relacionadas
                .ThenInclude(ih => ih.IdImagenNavigation)  // Asegúrate de cargar la entidad de Imagen
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }

            return View(habitacione);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            // Filtrar los tipos de habitación disponibles (Estado == true)
            var tiposHabitacionDisponibles = _context.TipoHabitaciones
                                                      .Where(t => t.Estado == true) // Solo los disponibles
                                                      .ToList();
            ViewData["IdTipoHabitacion"] = new SelectList(tiposHabitacionDisponibles, "IdTipoHabitacion", "NombreTipoHabitacion");
            return View();
        }

        // POST: Habitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHabitacion,IdTipoHabitacion,NombreHabitacion,Estado,Descripcion,Costo")] Habitacione habitacione, List<IFormFile> uploadedFiles)
        {
            Debug.WriteLine($"Valor de Estado recibido: {habitacione.Estado}");
            if (ModelState.IsValid)
            {
                // Agregar la nueva habitación
                _context.Add(habitacione);
                await _context.SaveChangesAsync();  // Guardamos primero la habitación para que tenga un ID generado

                // Verificar si se han subido archivos
                if (uploadedFiles != null && uploadedFiles.Count > 0)
                {
                    foreach (var file in uploadedFiles)
                    {
                        if (file.Length > 0)
                        {
                            // Asegurarnos de que la carpeta para guardar imágenes existe
                            var directoryPath = Path.Combine("wwwroot/imagenes");
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);  // Crear la carpeta si no existe
                            }

                            // Ruta completa para guardar el archivo
                            var filePath = Path.Combine(directoryPath, file.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);  // Guardamos el archivo en la carpeta
                            }

                            // Crear la entidad `Imagene` y asociarla con la URL de la imagen
                            var nuevaImagen = new Imagene
                            {
                                UrlImagen = $"/imagenes/{file.FileName}"
                            };

                            // Guardamos la imagen en la base de datos
                            _context.Imagenes.Add(nuevaImagen);
                            await _context.SaveChangesAsync();  // Guardamos la imagen

                            // Relacionamos la imagen con la habitación
                            var imagenHabitacion = new ImagenHabitacion
                            {
                                IdHabitacion = habitacione.IdHabitacion,  // Relacionamos con la habitación
                                IdImagen = nuevaImagen.IdImagen  // Asociamos la imagen con la nueva imagen registrada
                            };

                            _context.ImagenHabitacions.Add(imagenHabitacion);
                        }
                    }

                    // Guardamos las relaciones de las imágenes
                    await _context.SaveChangesAsync();
                }

                // Redirigimos al índice después de guardar
                return RedirectToAction(nameof(Index));
            }

            // En caso de error, volvemos a mostrar el formulario con los datos actuales
            ViewData["IdTipoHabitacion"] = new SelectList(_context.TipoHabitaciones, "IdTipoHabitacion", "IdTipoHabitacion", habitacione.IdTipoHabitacion);
            return View(habitacione);
        }



        // GET: Habitaciones/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var habitacione = await _context.Habitaciones.FindAsync(id);
        //    if (habitacione == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdTipoHabitacion"] = new SelectList(_context.TipoHabitaciones, "IdTipoHabitacion", "IdTipoHabitacion", habitacione.IdTipoHabitacion);
        //    return View(habitacione);
        //}
        // GET: Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.ImagenHabitacions)
                .ThenInclude(ih => ih.IdImagenNavigation)
                .FirstOrDefaultAsync(h => h.IdHabitacion == id);

            if (habitacione == null)
            {
                return NotFound();
            }

            ViewData["IdTipoHabitacion"] = new SelectList(_context.TipoHabitaciones, "IdTipoHabitacion", "NombreTipoHabitacion", habitacione.IdTipoHabitacion);
            return View(habitacione);
        }


        // POST: Habitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdHabitacion,IdTipoHabitacion,NombreHabitacion,Estado,Descripcion,Costo")] Habitacione habitacione)
        //{
        //    if (id != habitacione.IdHabitacion)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(habitacione);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HabitacioneExists(habitacione.IdHabitacion))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdTipoHabitacion"] = new SelectList(_context.TipoHabitaciones, "IdTipoHabitacion", "IdTipoHabitacion", habitacione.IdTipoHabitacion);
        //    return View(habitacione);
        //}
        public async Task<IActionResult> Edit(int id, [Bind("IdHabitacion,IdTipoHabitacion,NombreHabitacion,Estado,Descripcion,Costo")] Habitacione habitacione, List<IFormFile> uploadedFiles)
        {
            if (id != habitacione.IdHabitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualiza la habitación
                    _context.Update(habitacione);
                    await _context.SaveChangesAsync();

                    // Manejo de carga de archivos
                    if (uploadedFiles != null && uploadedFiles.Count > 0)
                    {
                        foreach (var file in uploadedFiles)
                        {
                            if (file.Length > 0)
                            {
                                // Aquí podrías cambiar la lógica para guardar el archivo en un almacenamiento permanente
                                var filePath = Path.Combine("wwwroot/imagenes", file.FileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                // Crear y guardar la imagen
                                var imagen = new Imagene
                                {
                                    UrlImagen = $"/imagenes/{file.FileName}"
                                };
                                _context.Imagenes.Add(imagen);
                                await _context.SaveChangesAsync();

                                // Crear y guardar la relación de imagen con la habitación
                                var imagenHabitacion = new ImagenHabitacion
                                {
                                    IdHabitacion = habitacione.IdHabitacion,
                                    IdImagen = imagen.IdImagen
                                };
                                _context.ImagenHabitacions.Add(imagenHabitacion);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacioneExists(habitacione.IdHabitacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTipoHabitacion"] = new SelectList(_context.TipoHabitaciones, "IdTipoHabitacion", "NombreTipoHabitacion", habitacione.IdTipoHabitacion);
            return View(habitacione);
        }




        // GET: Habitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .Include(h => h.IdTipoHabitacionNavigation)
                .FirstOrDefaultAsync(m => m.IdHabitacion == id);
            if (habitacione == null)
            {
                return NotFound();
            }

            return View(habitacione);
        }

        // POST: Habitaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacione = await _context.Habitaciones
                .Include(h => h.ImagenHabitacions)
                .FirstOrDefaultAsync(h => h.IdHabitacion == id);

            if (habitacione == null)
            {  return NotFound(); }

            // Eliminar todas las relaciones en la tabla intermedia ImagenHabitacion
            _context.ImagenHabitacions.RemoveRange(habitacione.ImagenHabitacions);

            // Ahora eliminar la habitación
            _context.Habitaciones.Remove(habitacione);


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacioneExists(int id)
        {
            return _context.Habitaciones.Any(e => e.IdHabitacion == id);
        }

        /*  cambios mios  */



        // GET: Habitaciones/UploadImage/5
        public async Task<IActionResult> UploadImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habitacione = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.IdHabitacion == id);

            if (habitacione == null)
            {
                return NotFound();
            }

            // Retorna la vista con el modelo para subir la imagen
            return View(new ImagenHabitacion { IdHabitacion = id.Value });
        }

        // POST: Habitaciones/UploadImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Por favor seleccione un archivo.");
                return View();
            }

            // Guardamos la imagen en el sistema de archivos (en este caso wwwroot/images)
            var imageUrl = await SaveImageAsync(file);

            // Creamos una nueva imagen y la asociamos con la habitación.
            var imagen = new Imagene
            {
                UrlImagen = imageUrl
            };

            _context.Imagenes.Add(imagen);
            await _context.SaveChangesAsync();

            // Creamos la relación entre la imagen y la habitación
            var imagenHabitacion = new ImagenHabitacion
            {
                IdImagen = imagen.IdImagen,
                IdHabitacion = id
            };

            _context.ImagenHabitacions.Add(imagenHabitacion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }
        // Método para guardar la imagen en el servidor (en la carpeta wwwroot/images)
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Devolver la URL de la imagen (en este caso sería la ruta pública para servir la imagen)
            return $"/imagenes/{file.FileName}";
        }
        [HttpPost]
        public async Task<IActionResult> ActualizarEstado(int id, bool estado)
        {
            try
            {
                var habitacion = await _context.Habitaciones.FindAsync(id);
                if (habitacion == null)
                {
                    return NotFound();
                }

                // Verifica si el estado recibido es el mismo que el actual
                if (habitacion.Estado == estado)
                {
                    return Json(new { success = true });
                }

                habitacion.Estado = estado;
                _context.Update(habitacion);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar el error
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult GestionarImagenes(int idHabitacion)
        {
            // Consulta con las relaciones necesarias
            var habitacion = _context.Habitaciones
                .Include(h => h.ImagenHabitacions)
                .ThenInclude(ih => ih.IdImagenNavigation)
                .FirstOrDefault(h => h.IdHabitacion == idHabitacion);

            // Si la habitación no existe, devuelve un error 404
            if (habitacion == null)
            {
                return NotFound($"No se encontró la habitación con ID {idHabitacion}");
            }

            // Si no hay imágenes asociadas, muestra un mensaje en la vista
            if (habitacion.ImagenHabitacions == null || !habitacion.ImagenHabitacions.Any())
            {
                ViewBag.Mensaje = "No hay imágenes asociadas a esta habitación.";
            }

            return View(habitacion);
        }

        [HttpPost]
        public IActionResult EliminarImagen(int idImagen)
        {
            // Buscar la relación de ImagenHabitacion incluyendo la imagen asociada
            var imagenHabitacion = _context.ImagenHabitacions
                .Include(ih => ih.IdImagenNavigation)
                .FirstOrDefault(ih => ih.IdImagen == idImagen);

            if (imagenHabitacion == null)
            {
                TempData["Mensaje"] = "La imagen no se encontró.";
                return RedirectToAction("GestionarImagenes", new { idHabitacion = (int?)null }); // Evitar errores
            }

            // Eliminar la imagen física del servidor
            var rutaImagen = imagenHabitacion.IdImagenNavigation?.UrlImagen;
            if (!string.IsNullOrWhiteSpace(rutaImagen))
            {
                try
                {
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = $"No se pudo eliminar la imagen del servidor: {ex.Message}";
                }
            }

            // Eliminar las entradas de la base de datos
            try
            {
                _context.ImagenHabitacions.Remove(imagenHabitacion);

                if (imagenHabitacion.IdImagenNavigation != null)
                {
                    _context.Imagenes.Remove(imagenHabitacion.IdImagenNavigation);
                }

                _context.SaveChanges();

                TempData["Mensaje"] = "La imagen se eliminó correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Ocurrió un error al eliminar la imagen: {ex.Message}";
            }

            // Redirigir de vuelta a la gestión de imágenes
            return RedirectToAction("GestionarImagenes", new { idHabitacion = imagenHabitacion.IdHabitacion });
        }




    }
}
