using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickJob.BusinessLogic.Extensions;
using QuickJob.DataModel.Configuration;
using IConfigurationProvider = Vostok.Configuration.Abstractions.IConfigurationProvider;

namespace QuickJob.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly KeycloackSettings keycloackSettings;

    public OrdersController(IConfigurationProvider configurationProvider)
    {
        keycloackSettings = configurationProvider.Get<KeycloackSettings>();
    }
    
    [HttpGet("{orderId}")]
    public async Task<IActionResult> Get(Guid orderId)
    {
        var userId = User.GetId();
        //todo
        return Ok(userId ?? "NULL");
    }
}
