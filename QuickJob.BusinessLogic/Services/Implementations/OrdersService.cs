using System.ComponentModel.Design;
using QuickJob.DataModel.Api;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Context;
using QuickJob.DataModel.Entites;

namespace QuickJob.BusinessLogic.Services.Implementations;

public class OrdersService : IOrdersService
{
    public async Task<OrderResponse> CreateOrder(CreateOrderRequest createOrderRequest)
    {
        var order = new Order(createOrderRequest)
        {
            CustomerId = RequestContext.ClientInfo.UserId
        };
        //await ordersStorage.CreateOrder(order);
        return new OrderResponse
        {
            
        };
    }
    
    public async Task<OrderResponse> GetOrder(Guid orderId)
    {
        //XZXZ
        //var order = await ordersStorage.GetOrderById(orderId);
        var order =  new OrderResponse
        {
            
        };
        if (order.CustomerId != RequestContext.ClientInfo.UserId)
            return order;

        //var responses = await responsesStorage.GetResponsesByOrderId(orderId).Were(x => x.Status != Rejected);
        order.Responses = new List<ResponseResponse>() { };
        return order;
    }

    public async Task<OrderResponse> UpdateOrder(Guid orderId, UpdateOrderRequest updateOrderRequest)
    {
        //var order = await ordersStorage.GetOrderById(orderId);
        var order = new OrderResponse
        {
            
        };
        if (order.CustomerId != RequestContext.ClientInfo.UserId)
            throw new CheckoutException("Нет прав на редактирование", 403);

        //await ordersStorage.UpdateOrderById(orderId, updateOrderRequest);
        return order;
    }

    public async Task DeleteOrder(Guid orderId)
    {
        //var order = await ordersStorage.GetOrderById(orderId);
        var order = new OrderResponse
        {
            
        };
        if (order.CustomerId != RequestContext.ClientInfo.UserId)
            throw new CheckoutException("Нет прав на удаление", 403);

        //await ordersStorage.DeleteOrderById(orderId);
    }

    public async Task<SearchOrdersResponse> SearchOrders(SearchOrdersRequest searchOrdersRequest)
    {
        //var orders = await ordersStorage.SearchOrders(searchOrdersRequest);
        return new SearchOrdersResponse
        {

        };
    }

    public async Task RespondToOrder(Guid orderId)
    {
        //var order = await ordersStorage.GetOrderById(orderId);
        var order = new OrderResponse
        {
            
        };
        if (order.CustomerId == RequestContext.ClientInfo.UserId)
            throw new CheckoutException("Нельзя откликнутся на свой заказ", 403);
        if (order.ResponsesCount == order.Limit)
            throw new CheckoutException("Заказ заполнен", 409);
        
        //await responsesStorage.AddResponse(orderId, RequestContext.ClientInfo.UserId);
    }

    public async Task DeleteRespondToOrder(Guid responseId)
    {
        //var response = await responsesStorage.GetResponse(responseId, RequestContext.ClientInfo.UserId);
        var response = new ResponseResponse
        {
            
        };
        
        //await responsesStorage.DeleteResponseByUd(responseId);
        
        if (response.Status == ResponseStatuses.Approved) {}
            //await ordersStorage.UpdateOrderById(response.OrderId, new updateOrderRequest, WITHFILTER);

    }

    public async Task SetRespondStatus(Guid responseId, ResponseStatuses responseStatus)
    {
        //var response = await responsesStorage.GetResponse(responseId, RequestContext.ClientInfo.UserId);
        //if response already set - 409
        //var order = await ordersStorage.GetOrderById(response.OrderId);
        var order = new OrderResponse
        {
            
        };
        if (responseStatus == ResponseStatuses.Approved && order.ResponsesCount == order.Limit)
            throw new CheckoutException("Заказ заполнен", 409);
        
        //await responsesStorage.UpdateResponse(responseId, responseStatus);
        if (responseStatus == ResponseStatuses.Rejected)
        {
            order.ResponsesCount--;
            //await ordersStorage.UpdateById(orderId, order);
        }
    }

    public async Task<SearchOrdersResponse> GetOrdersHistory(HistoryTypes historyType)
    {
        var orders = new SearchOrdersResponse();
        switch (historyType)
        {
            case HistoryTypes.All:
                await AddCustomerOrders(orders);
                await AddWorkerOrders(orders);
                break;
            case HistoryTypes.Customer:
                await AddCustomerOrders(orders);
                break;
            case HistoryTypes.Worker:
                await AddWorkerOrders(orders);
                break;
        }
        return orders;
    }

    private async Task AddWorkerOrders(SearchOrdersResponse orders)
    {
        //var responses = responsesStorage.GetResponsesByUserId(RequestContext.ClientInfo.UserId)
        //var workerOrders = ordersStorage.GetOrdersById(responses.Select(x => x.Id))
        throw new NotImplementedException();
    }

    private async Task AddCustomerOrders(SearchOrdersResponse orders)
    {
        //var customerOrders = ordersStorage.GetOrdersByUserId(RequestContext.ClientInfo.UserId)
        throw new NotImplementedException();
    }
}