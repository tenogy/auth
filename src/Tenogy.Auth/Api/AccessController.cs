using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
	[Route("access")]
	[Authorize]
	public class AccessController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
		}
	}
}
