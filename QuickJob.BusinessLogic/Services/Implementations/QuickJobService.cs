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

public sealed class QuickJobService : IQuickJobService
{
    private readonly IOrdersStorage ordersStorage;
    private readonly IS3Storage s3Storage;
    private readonly IResponsesStorage responsesStorage;

    public QuickJobService(
        IOrdersStorage ordersStorage, 
        IResponsesStorage responsesStorage, 
        IS3Storage s3Storage)
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

        var createResult = await ordersStorage.CreateOrder(order);
        if (!createResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(createResult.ErrorResult.ErrorMessage) );

        return order.ToResponse();
    }
    
    public async Task<OrderResponse> GetOrder(Guid orderId)
    {
        var orderResult = await ordersStorage.GetOrderById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        if (orderResult.Response == null)
            throw new CustomHttpException(HttpStatusCode.NotFound, HttpErrors.NotFound(orderId) );
        
        var order = orderResult.Response;
        var userId = RequestContext.ClientInfo.UserId;
        if (!order.IsActive && order.CustomerId != userId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));

        var orderResponse = order.ToResponse();
        if (order.CustomerId == userId)
        {
            var responsesResult = await responsesStorage.GetResponsesByOrderId(orderId);
            if (!responsesResult.IsSuccessful)
                throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
            if (responsesResult.Response.Count != 0)
                orderResponse.Responses = responsesResult.Response.Select(resp => new ResponseResponse(resp));
            orderResponse.CurrentUserIsCustomer = true;
        }
        
        return orderResponse;
    }

    public async Task<OrderResponse> UpdateOrder(Guid orderId, UpdateOrderRequest updateOrderRequest)
    {
        var orderResult = await ordersStorage.GetOrderById(orderId);
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
        
        var order = updateOrderRequest.ToEntity(orderId);
        var updateResult = await ordersStorage.UpdateOrder(order);
        if (!updateResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        return order.ToResponse();
    }

    public async Task DeleteOrder(Guid orderId)
    {
        var orderResult = await ordersStorage.GetOrderById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
        if (orderResult.Response.CustomerId != RequestContext.ClientInfo.UserId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));

        //todo await responsesStorage.DeleteByIdOrderId(orderId);
        var deleteResult = await ordersStorage.DeleteOrderById(orderResult.Response);
        if (!deleteResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage) );
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
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));
        if (order.ResponsesCount == order.Limit)
            throw new CustomHttpException(HttpStatusCode.Conflict, HttpErrors.LimitExceeded());

        
        //await responsesStorage.AddResponse(orderId, RequestContext.ClientInfo.UserId);
    }

    public async Task DeleteRespondToOrder(Guid responseId)
    {
        //var response = await responsesStorage.GetResponse(responseId, RequestContext.ClientInfo.UserId);
        var response = new ResponseResponse
        {
            
        };
        
        //await responsesStorage.DeleteResponseByUd(responseId);
        
        //if (response.Status == ResponseStatuses.Approved) {}
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
            throw new CustomHttpException(HttpStatusCode.Conflict, HttpErrors.LimitExceeded());
        
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
    
    private async Task<List<string>> UploadFiles(List<IFormFile> files)
    {
        var s3Result = await s3Storage.UploadFiles(files);

        if (!s3Result.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.AWS(s3Result.ErrorResult.ErrorMessage));
        
        return s3Result.Response;
    }
}