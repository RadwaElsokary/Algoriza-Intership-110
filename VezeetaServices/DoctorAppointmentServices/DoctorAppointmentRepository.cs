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

namespace VezeetaServices.AppointmentServices
{

	public class DoctorAppointmentRepository : IAppointmentRepository
	{

		private IRepository<Appointment> repository;
		private IRepository<Time> repositoryTime;
		private ApplicationDbContext context;
		public DoctorAppointmentRepository(IRepository<Appointment> repository, IRepository<Time> repositoryTime, ApplicationDbContext context)
		{
			this.repository = repository;
			this.repositoryTime = repositoryTime;
			this.context = context;
		}
		public bool AddAppointment(string DoctorId, Appointment appointment)
		{
			repository.Insert(appointment);
			repository.SaveChanges();
			return true;


		}
		public Time GetTime(int id)
		{
			var result = repositoryTime.GetId(id);
			return result;
		}

		public bool DeleteAppointment(int TimeId)
		{
			Time time = repositoryTime.GetId(TimeId);
			if (time.RequestId == null) 
		    { 
			    repositoryTime.Remove(time);
				repositoryTime.SaveChanges();

			}
			else
			{
				return false;
			}
			return true;
		}

		public  bool UpdateAppointment(int TimeId ,EditTimeDto dtotime)
		{

				Time time = repositoryTime.GetId(TimeId);
				if(time.RequestId == null)
			{
				time.Times = dtotime.Time;
				repositoryTime.Update(time);
			}
			else
			{
				return false;
			}
			return true;
		}


	}
}
