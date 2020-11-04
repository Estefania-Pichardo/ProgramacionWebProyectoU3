using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RazasPerros.Models;
using RazasPerros.Models.ViewModels;
using RazasPerros.Repositories;

namespace RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IWebHostEnvironment Enviroment { get; set; }
        public HomeController(IWebHostEnvironment env)
        {
            Enviroment = env;
        }
        public IActionResult Index(string id)
        {
            RazasRepository repos = new RazasRepository();
            IndexViewModel vm = new IndexViewModel();
            vm.Razas = repos.GetRazas();
            return View(vm);
        }
        public IActionResult Agregar()
        {
            RazaAdminViewModel rvm = new RazaAdminViewModel();
            RazasRepository repos = new RazasRepository();
            rvm.Paises = repos.GetPaises();
            return View(rvm);
        }

        [HttpPost]
        public IActionResult Agregar(RazaAdminViewModel rvm)
        {
            RazasRepository repos = new RazasRepository();
            try
            {
                if (rvm.Archivo == null)
                {
                    ModelState.AddModelError("", "Debe seleccionar la imagen del producto.");

                    rvm.Paises = repos.GetPaises();
                    return View(rvm);
                }
                else
                {
                    if (rvm.Archivo.ContentType != "image/jpeg" || rvm.Archivo.Length > 1024 * 1024 * 2)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un archivo jpg de menos de 2MB.");
                        rvm.Paises = repos.GetPaises();
                        return View(rvm);
                    }
                }

                repos.Insert(rvm.Raza);

                if (rvm.Archivo != null)
                {
                    FileStream fs = new FileStream(Enviroment.WebRootPath + "/imgs_perros/" + rvm.Raza.Id + "_0.jpg", FileMode.Create);
                    rvm.Archivo.CopyTo(fs);
                    fs.Close();
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                rvm.Paises = repos.GetPaises();
                return View(rvm);
            }
        }

        public IActionResult Editar(uint id)
        {
            RazaAdminViewModel rvm = new RazaAdminViewModel();
            RazasRepository repos = new RazasRepository();
            rvm.Paises = repos.GetPaises();
            rvm.Raza = repos.GetRazaById(id);
            if (rvm.Raza == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (System.IO.File.Exists(Enviroment.WebRootPath + $"/imgs_perros/{rvm.Raza.Id}_0.jpg"))
            {
                rvm.Imagen = rvm.Raza.Id + "_0.jpg";
            }
            else
            {
                rvm.Imagen = "NoPhoto.jpg";
            }

            return View(rvm);
        }
        [HttpPost]
        public IActionResult Editar(RazaAdminViewModel rvm)
        {
            RazasRepository repos = new RazasRepository();

            try
            {
                var original = repos.GetRazaById(rvm.Raza.Id);
                rvm.Paises = repos.GetPaises();
                if (original != null)
                {
                    // FALTA CREAR EL METODO UPDATE EN EL REPOSITORY
                    return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                rvm.Paises = repos.GetPaises();
                return View(rvm);
            }

        }

        public IActionResult Eliminar(uint id)
        {
            RazasRepository repos = new RazasRepository();
            var raza = repos.GetRazaById(id);
            return View(raza);
        }

        [HttpPost]
        public IActionResult Eliminar(Razas r)
        {
            RazasRepository repos = new RazasRepository();
            var original = repos.GetRazaById(r.Id);
            if (original != null)
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // CREAR EL METODO PARA ELIMINAR EN EL REPOSITORY. VA A SER UN UPDATE, PORQUE SERÁ ELIMINACIÓN LOGICA, NO FISICA.
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
