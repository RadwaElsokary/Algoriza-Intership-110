using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;

namespace VezeetaServices.AdminSettingServices
{
	public class AdminSettingRepository : IAdminSettingRepository
	{
		private readonly IRepository<Discound> repository;
		private readonly ApplicationDbContext context;
		public AdminSettingRepository(IRepository<Discound> repository,ApplicationDbContext context)
		{
			this.repository = repository;
			this.context = context;
		}
		public bool AddDiscound(DiscoundDto model)
		{
			var discound = new Discound();
			discound.DiscoundCode = model.DiscoundCodeCoupon;
			discound.Type = model.Type;
			discound.Value = model.Value;
			discound.RequestNumber = model.RequestNumber;
			discound.IsActive = true;
			repository.Insert(discound);
			repository.SaveChanges();
			return true;

		}
		public bool EditDiscoud(int id, DiscoundDto model)
		{
			var requestnum = context.Requests.Include(a => a.Discound).Where(a => a.DiscoundId == id).ToList();
			var result = repository.GetId(id);
			result.DiscoundCode = model.DiscoundCodeCoupon;
			result.Type = model.Type;
			result.Value = model.Value;
			result.RequestNumber = model.RequestNumber;
			if (requestnum.Count() == 0)
			{
				repository.Update(result);
				repository.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
			
		}

		public bool DeleteDiscound(int id)
		{
			var discound = repository.GetId(id);
			 repository.Delete(discound);
			repository.SaveChanges();
			return true;
		}
		public bool DeactiveDiscound(int id)
		{
			var result = repository.GetId(id);
			result.IsActive = false;
			repository.Update(result);
			repository.SaveChanges();
			return true;
		}
	}
}
