using Datingnew.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Datingnew.Controllers
{
	[ServiceFilter(typeof(LogUserActivity))]
	[Route("api/[controller]")]
	[ApiController]
	public class BaseApiController : ControllerBase
	{

	}
}
