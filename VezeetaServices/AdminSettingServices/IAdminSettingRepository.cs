using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;

namespace VezeetaServices.AdminSettingServices
{
	public interface IAdminSettingRepository 
	{
		bool AddDiscound(DiscoundDto discound);
		bool EditDiscoud(int id, DiscoundDto model);
		bool DeleteDiscound(int id);
		bool DeactiveDiscound(int id);
	}
}
