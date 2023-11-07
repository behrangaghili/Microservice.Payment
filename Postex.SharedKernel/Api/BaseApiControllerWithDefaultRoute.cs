using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Postex.SharedKernel.Api
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiControllerWithDefaultRoute : BaseApiController
    {
    }
}
