using System.Net;
using Microsoft.AspNetCore.Http;
using QuickJob.BusinessLogic.Mappers;
using QuickJob.BusinessLogic.Storages;
using QuickJob.BusinessLogic.Storages.S3;
using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Context;
using QuickJob.DataModel.Exceptions;

namespace QuickJob.BusinessLogic.Services.Implementations;

public sealed class OrdersService : IOrdersService
{
    private readonly IOrdersStorage ordersStorage;
    private readonly IResponsesStorage responsesStorage;
    private readonly IS3Storage s3Storage;

    public OrdersService(IOrdersStorage ordersStorage, IResponsesStorage responsesStorage, IS3Storage s3Storage)
    {
        this.ordersStorage = ordersStorage;
        this.responsesStorage = responsesStorage;
        this.s3Storage = s3Storage;
    }

    public async Task<OrderResponse> CreateOrder(CreateOrderRequest createOrderRequest)
    {
        var order = createOrderRequest.ToEntity(RequestContext.ClientInfo.UserId);
        
        if (createOrderRequest.Files != null) 
            order.FileUrls = await UploadFiles(createOrderRequest.Files);

        var createResult = await ordersStorage.CreateEntity(order);
        if (!createResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(createResult.ErrorResult.ErrorMessage) );

        var orderResponse =  order.ToResponse();
        orderResponse.CurrentUserIsCustomer = true;
        return orderResponse;
    }
    
    public async Task<OrderResponse> GetOrder(Guid orderId)
    {
        var orderResult = await ordersStorage.GetFullOrderById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        if (orderResult.Response == null)
            throw new CustomHttpException(HttpStatusCode.NotFound, HttpErrors.NotFound(orderId) );
        
        var order = orderResult.Response;
        var currentUserId = RequestContext.ClientInfo.UserId;
        if (!order.IsActive && order.CustomerId != currentUserId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));

        var orderResponse = order.ToResponse();
        if (order.CustomerId == currentUserId)
        {
            if (order.Responses.Count != 0)
                orderResponse.Responses = order.Responses.Select(resp => new ResponseResponse(resp));
            orderResponse.CurrentUserIsCustomer = true;
            return orderResponse;
        }
        var currentUserResponse = order.Responses.FirstOrDefault(o => o.UserId == currentUserId);
        if (currentUserResponse != null) 
            orderResponse.ResponseStatus = currentUserResponse.Status.ToString();

        return orderResponse;
    }

    public async Task<OrderResponse> UpdateOrder(Guid orderId, UpdateOrderRequest updateOrderRequest)
    {
        var orderResult = await ordersStorage.GetEntityById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        if (orderResult.Response.CustomerId != RequestContext.ClientInfo.UserId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));

        if (updateOrderRequest.NewFiles != null)
        {
            updateOrderRequest.FileUrls ??= new List<string>();
            updateOrderRequest.FileUrls.AddRange(await UploadFiles(updateOrderRequest.NewFiles));
            updateOrderRequest.NewFiles = null;
        }
        
        var order = orderResult.Response.Update(updateOrderRequest);
        var updateResult = await ordersStorage.UpdateEntity(order);
        if (!updateResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(updateResult.ErrorResult.ErrorMessage) );
        return order.ToResponse();
    }

    public async Task DeleteOrder(Guid orderId)
    {
        var orderResult = await ordersStorage.GetEntityById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        if (orderResult.Response.CustomerId != RequestContext.ClientInfo.UserId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));

        var deleteResult = await ordersStorage.DeleteEntity(orderResult.Response);
        if (!deleteResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(deleteResult.ErrorResult.ErrorMessage) );
    }

    public async Task<SearchOrdersResponse> SearchOrders(SearchOrdersRequest searchOrdersRequest)
    {
        var searchResult = await ordersStorage.SearchOrders(searchOrdersRequest);
        if (!searchResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(searchResult.ErrorResult.ErrorMessage) );

        return new SearchOrdersResponse
        {
            FoundItems = searchResult.Response.Select(x => x.ToResponse()).ToList(),
            TotalCount = 0
        };
    }
    
    public async Task<SearchOrdersResponse> GetOrdersHistory(HistoryType historyType)
    {
        var orders = new SearchOrdersResponse
        {
            FoundItems = new List<OrderResponse>(),
            TotalCount = 0
        };
        switch (historyType)
        {
            case HistoryType.All:
                await AddCustomerOrders(orders);
                await AddWorkerOrders(orders);
                break;
            case HistoryType.Customer:
                await AddCustomerOrders(orders);
                break;
            case HistoryType.Worker:
                await AddWorkerOrders(orders);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(historyType), historyType, null);
        }
        return orders;
    }

    private async Task AddWorkerOrders(SearchOrdersResponse orders)
    {
        var responsesResult = await responsesStorage.GetResponsesByOrderId(RequestContext.ClientInfo.UserId);
        if (!responsesResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(responsesResult.ErrorResult.ErrorMessage));
        orders.FoundItems.AddRange(responsesResult.Response.Select(x => x.Order.ToResponse()).ToList());
    }

    private async Task AddCustomerOrders(SearchOrdersResponse orders)
    {
        var ordersResult = await ordersStorage.GetOrdersByCustomer(RequestContext.ClientInfo.UserId);
        if (!ordersResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(ordersResult.ErrorResult.ErrorMessage));
        orders.FoundItems.AddRange(ordersResult.Response.Select(x => x.ToResponse()).ToList());
    }
    
    private async Task<List<string>> UploadFiles(List<IFormFile> files)
    {
        var s3Result = await s3Storage.UploadFiles(files);
        if (!s3Result.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.AWS(s3Result.ErrorResult.ErrorMessage));
        return s3Result.Response;
    }
}