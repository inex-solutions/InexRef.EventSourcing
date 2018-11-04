using System.Collections.Generic;
using System.Linq;
using InexRef.EventSourcing.Tests.Account.Domain;
using Microsoft.AspNetCore.Mvc;

namespace InexRef.EventSourcing.Tests.Account.Application.Web.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<AccountId> Get()
        {
            return Enumerable.Empty<AccountId>();
        }
    }
}