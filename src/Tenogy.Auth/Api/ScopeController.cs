using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
	[Route("scopes")]
	[Authorize]
	public class ScopeController: Controller
	{
		[HttpGet]
		[Route("scope")]
		[Authorize("Tenogy.Api:Access")]
		public IActionResult Scope()
		{
			return Json(new
			{
				Message = "Hello from Tenogy.Api:Access"
			});
		}

		[HttpGet]
		[Route("scope1")]
		[Authorize("Tenogy.Api:Access1")]
		public IActionResult Scope1()
		{
			return Json(new
			{
				Message = "Hello from Tenogy.Api:Access1"
			});
		}

		[HttpGet]
		[Route("scope2")]
		[Authorize("Tenogy.Api:Access2")]
		public IActionResult Scope2()
		{
			return Json(new
			{
				Message = "Hello from Tenogy.Api:Access2"
			});
		}
	}
}
