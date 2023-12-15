using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickJob.BusinessLogic.Services;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;

namespace QuickJob.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersService ordersService;

    public OrdersController(IOrdersService ordersService)
    {
        this.ordersService = ordersService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest createOrderRequest)
    {
        var order = await ordersService.CreateOrder(createOrderRequest);
        return Ok(order);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Get(Guid orderId)
    {
        var order = await ordersService.GetOrder(orderId);
        return Ok(order);
    }
    
    [HttpPatch("{orderId}")]
    public async Task<IActionResult> Update(Guid orderId, UpdateOrderRequest updateOrderRequest)
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
    
    [HttpGet]
    public async Task<IActionResult> Search(SearchOrdersRequest searchOrdersRequest)
    {
        var orders = await ordersService.SearchOrders(searchOrdersRequest);
        return Ok(orders);
    }

    #region Workers
    
    [HttpPut("{orderId}/responses")]
    public async Task<IActionResult> RespondToOrder(Guid orderId)
    {
        await ordersService.RespondToOrder(orderId);
        return Ok();
    }
    
    [HttpDelete("{orderId}/responses/{responseId}")]
    public async Task<IActionResult> DeleteRespondToOrder(Guid responseId)
    {
        await ordersService.DeleteRespondToOrder(responseId);
        return Ok();
    }
    
    #endregion

    #region Customers

    [HttpPut("{orderId}/responses/{responseId}/approve")]
    public async Task<IActionResult> ApproveRespondToOrder(Guid responseId)
    {
        await ordersService.SetRespondStatus(responseId, ResponseStatuses.Approved);
        return Ok();
    }
    
    [HttpPut("{orderId}/responses/{responseId}/reject")]
    public async Task<IActionResult> RejectRespondToOrder(Guid responseId)
    {
        await ordersService.SetRespondStatus(responseId, ResponseStatuses.Rejected);
        return Ok();
    }
    
    #endregion
    
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] HistoryTypes historyType = HistoryTypes.All)
    {
        var orders = await ordersService.GetOrdersHistory(historyType);
        return Ok(orders);
    }
}
