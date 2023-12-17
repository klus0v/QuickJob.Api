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
    private readonly IQuickJobService quickJobService;

    public OrdersController(IQuickJobService quickJobService)
    {
        this.quickJobService = quickJobService;
    }
    
    #region All

    [HttpPost]
    public async Task<IActionResult> Create([FromForm]CreateOrderRequest createOrderRequest)
    {
        var order = await quickJobService.CreateOrder(createOrderRequest);
        return Ok(order);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Get(Guid orderId)
    {
        var order = await quickJobService.GetOrder(orderId);
        return Ok(order);
    }
    
    [HttpPatch("{orderId}")]
    public async Task<IActionResult> Update(Guid orderId, UpdateOrderRequest updateOrderRequest)
    {
        var order = await quickJobService.UpdateOrder(orderId, updateOrderRequest);
        return Ok(order);
    }
    
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> Delete(Guid orderId)
    {
        await quickJobService.DeleteOrder(orderId);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> Search(SearchOrdersRequest searchOrdersRequest)
    {
        var orders = await quickJobService.SearchOrders(searchOrdersRequest);
        return Ok(orders);
    }
    
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] HistoryTypes historyType = HistoryTypes.All)
    {
        var orders = await quickJobService.GetOrdersHistory(historyType);
        return Ok(orders);
    }

    #endregion

    #region Workers
    
    [HttpPut("{orderId}/responses")]
    public async Task<IActionResult> RespondToOrder(Guid orderId)
    {
        await quickJobService.RespondToOrder(orderId);
        return Ok();
    }
    
    [HttpDelete("{orderId}/responses/{responseId}")]
    public async Task<IActionResult> DeleteRespondToOrder(Guid responseId)
    {
        await quickJobService.DeleteRespondToOrder(responseId);
        return Ok();
    }
    
    #endregion

    #region Customers

    [HttpPut("{orderId}/responses/{responseId}/approve")]
    public async Task<IActionResult> ApproveRespondToOrder(Guid responseId)
    {
        await quickJobService.SetRespondStatus(responseId, ResponseStatuses.Approved);
        return Ok();
    }
    
    [HttpPut("{orderId}/responses/{responseId}/reject")]
    public async Task<IActionResult> RejectRespondToOrder(Guid responseId)
    {
        await quickJobService.SetRespondStatus(responseId, ResponseStatuses.Rejected);
        return Ok();
    }
    
    #endregion
}
