using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vezeeta.Domain.Models;
using Vezeeta.Domain.ModelsDto;
using Vezeeta.Repository;

namespace Vezeeta.Api.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		

		public AccountController(IWebHostEnvironment hostingEnvironment,
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			this.hostingEnvironment = hostingEnvironment;
			this.userManager = userManager;
			this.signInManager = signInManager;
			
		}

		private string ProcessUploadFileRegistration(RegistrationDto model)
		{
			string uniqueFileName = null;
			if (model.Photo != null)
			{
				string uploadFile = Path.Combine(hostingEnvironment.WebRootPath, "images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
				string filePath = Path.Combine(uploadFile, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					model.Photo.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}


		[HttpPost]
		[Route("Registration")]
		public async Task<IActionResult> Registration([FromBody] RegistrationDto model)
		{
			
				string uniqueFileName = ProcessUploadFileRegistration(model);

				var user = new ApplicationUser
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					UserName = model.Email,
					Gender = model.Gender,
					BirthOfDate = model.BirthOfDate,
					PhoneNumber = model.PhoneNumber,
					PhotoPath = uniqueFileName
				};


					var result = await userManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(user,"Patient");
					    return StatusCode(200, true);
					}
					else
					{
						return StatusCode(400,false);
					}
				
			
		}


		[HttpPost]
		[Route("Login")]
		public async Task <IActionResult> Login([FromBody]LoginDto login)
		{
			//login.ExternalLogin = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			
				var user =await userManager.FindByEmailAsync(login.Email);
				var result = await signInManager.PasswordSignInAsync(login.Email,login.Password,false,false);
				if (result.Succeeded)
				{
					return StatusCode(200,true);
				}
				else
				{
					return StatusCode(400,false);
				}
			

		}

		}
	}
