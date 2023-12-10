using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Vezeeta.Domain.Models;
using Vezeeta.Repository;
using Vezeeta.Repository.Repository;
using Vezeeta.Services.DoctorServices;
using VezeetaServices.AdminSettingServices;
using VezeetaServices.AppointmentServices;
using VezeetaServices.PatientServices;
using VezeetaServices.RequestServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen() ;

var provider = builder.Services.BuildServiceProvider();
var Configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SQLServerConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

builder.Services.AddTransient<IAdminDoctorRepository,AdminDoctorRepository >();
builder.Services.AddTransient<IAppointmentRepository, DoctorAppointmentRepository>();
builder.Services.AddTransient<IAdminpatientRepository, AdminPatientRepository>();
builder.Services.AddTransient<IRequestRepository, RequestRepository>();
builder.Services.AddTransient<IAdminSettingRepository, AdminSettingRepository>();

//builder.Services.AddMvc().AddNewtonsoftJson();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
