using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Tenogy.Auth.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HealthController : ControllerBase
	{

		[HttpGet]
		public HealthState Get()
		{
			return new HealthState
			{
				Status = "healthy",
				Version = Environment.GetEnvironmentVariable("APP_VERSION")
			};
		}
	}

	public class HealthState
	{
		public string Status { get; set; }
		public string Version { get; set; }
	}
}
