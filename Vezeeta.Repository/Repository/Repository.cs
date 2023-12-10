using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Repository.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext context;
		private DbSet<T> entities;

		public Repository(ApplicationDbContext context)
		{
			this.context = context;
			entities = context.Set<T>();
		}
		public void Delete(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			entities.Remove(entity);
			context.SaveChanges();
		}

		public IEnumerable<T> GetAll()
		{
			return entities.AsEnumerable();
		}

		public T GetById(string Id)
		{
			return entities.Find(Id);
		}
		public T GetId (int id)
		{
			return entities.Find(id);
		}
		public void Insert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			entities.Add(entity);
			context.SaveChanges();
		}

		public void Remove(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			entities.Remove(entity);
			
		}

		public void SaveChanges()
		{
			context.SaveChanges();
		}

		public void Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
		    var result =entities.Attach(entity);
			result.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
			context.SaveChanges();
		}
	}
}
