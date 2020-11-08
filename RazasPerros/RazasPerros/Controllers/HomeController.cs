using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazasPerros.Models;
using RazasPerros.Models.ViewModels;
using RazasPerros.Repositories;

namespace RazasPerros.Controllers
{
	public class HomeController : Controller
	{
		[Route("/")]
		[Route("Razas/{id}")]
		public IActionResult Index(string id)
		{
			RazasRepository repos = new RazasRepository();
			IndexViewModel vm = new IndexViewModel
			{
				Razas = id == null ? repos.GetRazas() : repos.GetRazasByLetraInicial(id),
				LetrasIniciales = repos.GetLetrasIniciales()
			};
			return View(vm);
		}
		[Route("Raza/{id}")]
		public IActionResult InfoPerro(string id)
		{
			RazasRepository repos = new RazasRepository();
			InfoPerroViewModel vm = new InfoPerroViewModel();
			vm.Raza = repos.GetRazaByNombre(id);

			if (vm.Raza == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				vm.OtrasRazas = repos.Get4RandomRazasExcept(id);
				return View(vm);
			}
		}
		[Route("RazasPorPais")]
		public IActionResult RazasPorPais()
		{
			sistem14_razasContext context = new sistem14_razasContext();
			//RazasRepository repos = new RazasRepository();
			var paises = context.Paises.Include(x => x.Razas).OrderBy(x => x.Nombre);
		
			return View(paises);
		}
	}
}
