using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;

namespace Vezeeta.Services.AdminServices
{
	public static  class SeedAdmin 
	{
		public static  void SeedsAdmin(this ModelBuilder modelBuilder)
		{

			var AdminUser = new ApplicationUser
			{
				
				FirstName = "Admin",
				LastName = "User",
				Email = "admin@email.com",
				UserName = "admin@email.com",
				Gender = Gender.Female,
				PhoneNumber = "0112913842",
				BirthOfDate = DateTime.Parse("6-7-2001"),
				EmailConfirmed = false,
				LockoutEnabled=true,
				NormalizedEmail="ADMIN@EMAIL.COM",
				NormalizedUserName= "ADMIN@EMAIL.COM"


			};

			var Password = new PasswordHasher<ApplicationUser>();
			var hashed = Password.HashPassword(AdminUser, "Admin123###");
			AdminUser.PasswordHash = hashed;

			modelBuilder.Entity<ApplicationUser>().HasData(AdminUser);
			modelBuilder.Entity<IdentityUserRole<string>>().HasData(
				new IdentityUserRole<string>()
				{
					RoleId = "0",
					UserId =AdminUser.Id});
		}
	}

		

	}


