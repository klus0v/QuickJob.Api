using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickJob.BusinessLogic.Services;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
using QuickJob.DataModel.Api.Responses.Responses;

namespace QuickJob.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IResponsesService responsesService;
    private readonly IOrdersService ordersService;

    public OrdersController(IResponsesService responsesService, IOrdersService ordersService)
    {
        this.responsesService = responsesService;
        this.ordersService = ordersService;
    }
    
    #region Orders

    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromForm]CreateOrderRequest createOrderRequest)
    {
        var order = await ordersService.CreateOrder(createOrderRequest);
        return Ok(order);
    }

    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid orderId)
    {
        var order = await ordersService.GetOrder(orderId);
        return Ok(order);
    }
    
    [HttpPatch("{orderId}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid orderId, [FromForm] UpdateOrderRequest updateOrderRequest)
    {
        var order = await ordersService.UpdateOrder(orderId, updateOrderRequest);
        return Ok(order);
    }
    
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(Guid orderId)
    {
        await ordersService.DeleteOrder(orderId);
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(SearchOrdersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] SearchOrdersRequest searchOrdersRequest)
    {
        var orders = await ordersService.SearchOrders(searchOrdersRequest);
        return Ok(orders);
    }
    
    [HttpGet("history")]
    [ProducesResponseType(typeof(SearchOrdersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory([FromQuery] HistoryType historyType = HistoryType.All)
    {
        var orders = await ordersService.GetOrdersHistory(historyType);
        return Ok(orders);
    }

    #endregion

    #region Responses
    
    [HttpPut("{orderId}/responses")]
    public async Task<IActionResult> RespondToOrder(Guid orderId)
    {
        await responsesService.RespondToOrder(orderId);
        return Ok();
    }
    
    [HttpDelete("{orderId}/responses/{responseId}")]
    public async Task<IActionResult> DeleteRespondToOrder(Guid responseId)
    {
        await responsesService.DeleteRespondToOrder(responseId);
        return Ok();
    }

    [HttpPut("{orderId}/responses/{responseId}/approve")]
    public async Task<IActionResult> ApproveRespondToOrder(Guid responseId)
    {
        await responsesService.SetRespondStatus(responseId, ResponseStatus.Approved);
        return Ok();
    }
    
    [HttpPut("{orderId}/responses/{responseId}/reject")]
    public async Task<IActionResult> RejectRespondToOrder(Guid responseId)
    {
        await responsesService.SetRespondStatus(responseId, ResponseStatus.Rejected);
        return Ok();
    }
    
    #endregion
}
