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

public sealed class ResponsesService : IResponsesService
{
    private readonly IOrdersStorage ordersStorage;
    private readonly IResponsesStorage responsesStorage;

    public ResponsesService(IOrdersStorage ordersStorage, IResponsesStorage responsesStorage)
    {
        this.ordersStorage = ordersStorage;
        this.responsesStorage = responsesStorage;
    }

    public async Task RespondToOrder(Guid orderId)
    {
        var userId = RequestContext.ClientInfo.UserId;
        var orderResult = await ordersStorage.GetEntityById(orderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage));
        
        if (orderResult.Response.CustomerId == userId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(orderId));
        if (orderResult.Response.ApprovedResponsesCount == orderResult.Response.Limit)
            throw new CustomHttpException(HttpStatusCode.Conflict, HttpErrors.LimitExceeded());

        var response = orderId.CreateRespondToEntity(userId, RequestContext.ClientInfo.Name);
        await responsesStorage.CreateEntity(response);
    }

    public async Task DeleteRespondToOrder(Guid responseId)
    {
        //todo normal result pattern
        var responseResult = await responsesStorage.GetEntityById(responseId);
        if (!responseResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(responseResult.ErrorResult.ErrorMessage) );
        if (responseResult.Response == null)
            throw new CustomHttpException(HttpStatusCode.NotFound, HttpErrors.NotFound(responseId));
        if (responseResult.Response.UserId != RequestContext.ClientInfo.UserId)
            throw new CustomHttpException(HttpStatusCode.Forbidden, HttpErrors.NoAccess(responseId));
        
        await responsesStorage.DeleteEntity(responseResult.Response);
        
        if (responseResult.Response.Status == ResponseStatus.Approved)
        {
            var order = responseResult.Response.Order;
            order.ApprovedResponsesCount--;
            await ordersStorage.UpdateEntity(order);
        }
    }

    public async Task SetRespondStatus(Guid responseId, ResponseStatus responseStatus)
    {
        //todo transactions
        var responseResult = await responsesStorage.GetEntityById(responseId);
        if (!responseResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(responseResult.ErrorResult.ErrorMessage));
        if (responseResult.Response == null)
            throw new CustomHttpException(HttpStatusCode.NotFound, HttpErrors.NotFound(responseId));
        var response = responseResult.Response;
        if (response.Status == responseStatus)
            throw new CustomHttpException(HttpStatusCode.Conflict, HttpErrors.StatusAlreadySet());
        var orderResult = await ordersStorage.GetEntityById(response.OrderId);
        if (!orderResult.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(orderResult.ErrorResult.ErrorMessage));
        var order = orderResult.Response;
        if (responseStatus == ResponseStatus.Approved && order.ApprovedResponsesCount == order.Limit)
            throw new CustomHttpException(HttpStatusCode.Conflict, HttpErrors.LimitExceeded());

        if (response.Status == ResponseStatus.Approved && responseStatus == ResponseStatus.Rejected)
        {
            order.ApprovedResponsesCount--;
            await ordersStorage.UpdateEntity(order);
        }
        if (responseStatus == ResponseStatus.Approved)
        {
            //todo notification
            order.ApprovedResponsesCount++;
            await ordersStorage.UpdateEntity(order);
        }
        
        response.Status = responseStatus;
        var result = await responsesStorage.UpdateEntity(response);
        if (!result.IsSuccessful)
            throw new CustomHttpException(HttpStatusCode.ServiceUnavailable, HttpErrors.Pg(result.ErrorResult.ErrorMessage));
        
    }
}