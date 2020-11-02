using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazasPerros.Models;
using Microsoft.EntityFrameworkCore;

namespace RazasPerros.Repositories
{
	public class RazasRepository
	{
		sistem14_razasContext context = new sistem14_razasContext();

		public IEnumerable<RazaViewModel> GetRazas()
		{
			return context.Razas.OrderBy(x => x.Nombre)
				.Select(x => new RazaViewModel
				{
					Id = x.Id,
					Nombre = x.Nombre
				});
		}

		public IEnumerable<RazaViewModel> GetRazasByLetraInicial(string letra)
		{
			return GetRazas().Where(x => x.Nombre.StartsWith(letra));
		}


		public IEnumerable<char> GetLetrasIniciales()
		{
			return context.Razas.OrderBy(x => x.Nombre)
				.Select(x => x.Nombre.First()).Distinct();
		}

		public Razas GetRazaByNombre(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			return context.Razas
				.Include(x => x.Estadisticasraza)
				.Include(x => x.Caracteristicasfisicas)
				.Include(x => x.IdPaisNavigation)
				.FirstOrDefault(x => x.Nombre == nombre);
		}

		public IEnumerable<RazaViewModel> Get4RandomRazasExcept(string nombre)
		{
			nombre = nombre.Replace("-", " ");
			Random r = new Random();
			return context.Razas
				.Where(x => x.Nombre != nombre)
				.OrderBy(x => r.Next())
				.Take(4)
				.Select(x => new RazaViewModel { Id = x.Id, Nombre = x.Nombre });
		}

	}
}
