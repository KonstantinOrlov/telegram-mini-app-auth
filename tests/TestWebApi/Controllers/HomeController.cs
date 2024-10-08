using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neftm.TelegramMiniApp.Authorization;
using Neftm.TelegramMiniApp.Authorization.Extensions;
using Neftm.TelegramMiniApp.Authorization.Models;

namespace TestWebApi.Controllers;

public class ResponseDto
{
	public string UserId { get; set; }
	public string UserName { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string ChatInstance { get; set; }
}

[Authorize]
[ApiController, Route("api/home")]

public class HomeController : ControllerBase
{
	[HttpGet]
	[Authorize(AuthenticationSchemes = TmaDefaults.AuthenticationScheme)]
	public async Task<ActionResult<TmaUserPrincipals>> Index(CancellationToken ct)
	{
		if (User.TryGetTmaUser(out var result))
		{
			return result;
		}
		else
		{
			return new TmaUserPrincipals();
		}
	}	
	
	[HttpGet("premium")]
	[Authorize(AuthenticationSchemes = TmaDefaults.AuthenticationScheme, Policy = TmaDefaults.PremiumPolicy)]
	public async Task<IActionResult> Prem(CancellationToken ct)
	{
		return Ok("Only for wealthy");
	}
}