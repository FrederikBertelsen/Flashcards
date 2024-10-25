using Backend.Models;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Backend.Controllers;

public class BaseController(SessionManager sessionManager) : ControllerBase
{
    [NonAction]
    protected async Task<ActionResult> WithAuthAsync(Func<User, Task<ActionResult>> authorizedFunction)
    {
        try
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            if (user == null)
                return Unauthorized("Invalid or expired session.");
            
            return await authorizedFunction(user);
        }
        catch (Exception e)
        {
            return Unauthorized($"Authorization failed: {e.Message}");
        }
    }

    [NonAction]
    protected async Task<ActionResult> WithAuth(Func<User, ActionResult> authorizedFunction)
    {
        try
        {
            var user = await sessionManager.ValidateRequestAsync(Request);
            if (user == null)
                return Unauthorized("Invalid or expired session.");

            return authorizedFunction(user);
        }
        catch (Exception e)
        {
            return Unauthorized($"Authorization failed: {e.Message}");
        }
    }
}