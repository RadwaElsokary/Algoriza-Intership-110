using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vezeeta.Domain.Models;
//using Vezeeta.Services.AdminServices;
//using Vezeeta.Services.RoleServices;
//using Vezeeta.Services.SpecializationServices;


namespace Vezeeta.Repository
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Specialization>()
				.HasMany(a=>a.Doctor)
				.WithOne(b=>b.Specialist)
				.HasForeignKey(b=>b.SpecialistId);

			modelBuilder.Entity<Doctor>()
				.HasMany(a => a.Appointments)
				.WithOne(b => b.Doctor)
				.HasForeignKey(b => b.DoctorId);

			modelBuilder.Entity<Appointment>()
				.HasMany(a => a.Time)
				.WithOne(b => b.Appointment)
				.HasForeignKey(b => b.AppointmentId);

			modelBuilder.Entity<Request>()
				.HasOne(a => a.Discound)
				.WithMany(b => b.Requests)
				.HasForeignKey(b => b.DiscoundId);

			modelBuilder.Entity<Request>()
				.HasOne(a => a.Patient)
				.WithMany(b => b.Requests)
				.HasForeignKey(b => b.PatientId);

			modelBuilder.Entity<Request>()
				.HasOne(a => a.Time)
				.WithOne(b => b.Request)
				.HasForeignKey<Time>(b => b.RequestId)
				;


			//modelBuilder.Entity<Request>()
			//	.HasKey(t => new { t.Id, t.TimeId, t.DoctorId});
			//modelBuilder.Entity<Request>()
			//	.HasOne(r=>r.Times)
			//	.WithOne(a=>a.Request)
			//	.HasForeignKey(t => new {t. })

			//foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
			//{
			//	foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
			//}

			//modelBuilder.SeedsAdmin();
			//modelBuilder.SeedSpecialization();
			//modelBuilder.SeedRoles();

		}

		public virtual DbSet<Doctor> Doctors { set; get; }
		public virtual DbSet<Specialization> Specializations { set; get; }
		public virtual DbSet<Appointment> Appointments { set; get; }
		public virtual DbSet<Time> Times { set; get; }

		public virtual DbSet<Request> Requests { set; get; }
		public virtual DbSet<Discound> Discounds { set; get; }
	}
}
