using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vezeeta.Repository.Repository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAll();
		T GetById(string Id);
		T GetId(int id);
		void Insert(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Remove(T entity);
		void SaveChanges();
	}
}
