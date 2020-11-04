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
			return context.Razas.Where(x=>x.Eliminado==0).OrderBy(x => x.Nombre)
				.Select(x => new RazaViewModel
				{
					Id = x.Id,
					Nombre = x.Nombre
				});
		}

		public Razas GetRazaById(uint id)
		{
			return context.Razas.Include(x => x.Estadisticasraza)
				.Include(x => x.Caracteristicasfisicas)
				.Include(x => x.IdPaisNavigation)
				.FirstOrDefault(x => x.Id == id);
		}
		public IEnumerable<RazaViewModel> GetRazasByLetraInicial(string letra)
		{
			return GetRazas().Where(x => x.Nombre.StartsWith(letra));
		}


		public IEnumerable<char> GetLetrasIniciales()
		{
			return context.Razas.OrderBy(x => x.Nombre).Select(x => x.Nombre.First()).Distinct();
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
			var aleatorio = r.Next();
			return context.Razas
				.Where(x => x.Nombre != nombre)
				.OrderBy(x => aleatorio)
				.Take(4)
				.Select(x => new RazaViewModel { Id = x.Id, Nombre = x.Nombre });
		}

		public IEnumerable<Paises> GetRazasByPais()
		{
			return context.Paises.Include(x => x.Razas).OrderBy(x => x.Nombre);
		}
		public IEnumerable<Paises> GetPaises()
		{
			return context.Paises.OrderBy(x => x.Nombre);
		}

		public virtual void Insert(Razas entidad)
		{
			if (Validate(entidad))
			{
				context.Add(entidad);
				context.SaveChanges();
			}
		}

		public virtual bool Validate(Razas entidad)
		{
			if (string.IsNullOrWhiteSpace(entidad.Nombre))
				throw new Exception("Ingrese el nombre de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Descripcion))
				throw new Exception("Ingrese la descripción de la raza");
			if (string.IsNullOrWhiteSpace(entidad.OtrosNombres))
				throw new Exception("Ingrese otro nombre de la raza");
			if (entidad.PesoMax <= 0)
				throw new Exception("Ingrese el peso maximo de la raza");
			if (entidad.PesoMin <= 0)
				throw new Exception("Ingrese el peso minimo de la raza");
			if (entidad.AlturaMax <= 0)
				throw new Exception("Ingrese la altura maxima de la raza");
			if (entidad.AlturaMin <= 0)
				throw new Exception("Ingrese la altura minima de la raza");
			if (entidad.EsperanzaVida <= 0)
				throw new Exception("Ingrese la esperanza de vida de la raza");
			if (entidad.Estadisticasraza.AmistadDesconocidos < 0)
				throw new Exception("Ingrese el grado de amistad con desconocidos");
			if (entidad.Estadisticasraza.AmistadPerros < 0)
				throw new Exception("Ingrese el grado de amistad con otras razas");
			if (entidad.Estadisticasraza.EjercicioObligatorio < 0)
				throw new Exception("Ingrese el grado de ejercicio obligatorio");
			if (entidad.Estadisticasraza.FacilidadEntrenamiento < 0)
				throw new Exception("Ingrese el grado de facilidad de entrenamiento de la raza");
			if (entidad.Estadisticasraza.NecesidadCepillado < 0)
				throw new Exception("Ingrese el grado de necesidad de cepillado de la raza");
			if (entidad.Estadisticasraza.NivelEnergia < 0)
				throw new Exception("Ingrese el grado de nivel de energia de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Caracteristicasfisicas.Cola))
				throw new Exception("Ingrese la descripción de la cola de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Caracteristicasfisicas.Color))
				throw new Exception("Ingrese la descripción del color de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Caracteristicasfisicas.Hocico))
				throw new Exception("Ingrese la descripción del hocico de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Caracteristicasfisicas.Patas))
				throw new Exception("Ingrese la descripción de las patas de la raza");
			if (string.IsNullOrWhiteSpace(entidad.Caracteristicasfisicas.Pelo))
				throw new Exception("Ingrese la descripción del pelo de la raza");
			return true;
		}

	}
}
