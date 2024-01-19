using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
using QuickJob.DataModel.Api.Responses.Responses;
using QuickJob.DataModel.Postgres.Entities;

namespace QuickJob.BusinessLogic.Mappers;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            ApprovedResponsesCount = order.ApprovedResponsesCount,
            TotalResponsesCount = order.Responses == null ? 0 : order.Responses.Count,
            Title = order.Title,
            Description = order.Description,
            Address = order.Address,
            StartDateTime = order.StartDateTime,
            EndDateTime = order.EndDateTime,
            Categories = order.Categories ?? null,
            Skills = order.Skills ?? null,
            Limit = order.Limit,
            PaymentType = order.PaymentType.ToString(),
            WorkHours = order.WorkHours,
            Price = order.Price,
            FileUrls = order.FileUrls,
            IsActive = order.IsActive,
            EditDateTime = order.EditDateTime,
            CreateDateTime = order.CreateDateTime
        };
    }
    
    public static OrderResponse ToResponse(this Order order, ResponseStatus status)
    {
        var orderResponse = order.ToResponse();
        orderResponse.ResponseStatus = status.ToString();
        return orderResponse;
    }
    
    public static Order ToEntity(this CreateOrderRequest createOrderRequest, Guid userId)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = userId,
            Title = createOrderRequest.Title,
            Description = createOrderRequest.Description,
            Address = createOrderRequest.Address,
            Categories = createOrderRequest.Categories ?? null,
            Skills = createOrderRequest.Skills ?? null,
            StartDateTime = createOrderRequest.StartDateTime,
            EndDateTime = createOrderRequest.EndDateTime,
            Limit = createOrderRequest.Limit,
            PaymentType = createOrderRequest.PaymentType,
            WorkHours = createOrderRequest.WorkHours,
            Price = createOrderRequest.Price,
            CreateDateTime = DateTime.UtcNow,
        };
    }
    
    public static Order Update(this Order order, UpdateOrderRequest request)
    {
        order.EditDateTime = DateTime.UtcNow;
        
        if (request.Title != null)
            order.Title = request.Title;
        if (request.Description != null)
            order.Description = request.Description;
        if (request.Address != null)
            order.Address = request.Address;
        if (request.Categories != null)
            order.Categories = request.Categories;
        if (request.Skills != null)
            order.Skills = request.Skills;
        if (request.StartDateTime != null)
            order.StartDateTime = (DateTime)request.StartDateTime;
        if (request.EndDateTime != null)
            order.EndDateTime = (DateTime)request.EndDateTime;
        if (request.Limit != null)
            order.Limit = (int)request.Limit;
        if (request.PaymentType != null)
            order.PaymentType = (PaymentType)request.PaymentType;
        if (request.WorkHours != null)
            order.WorkHours = (double)request.WorkHours;
        if (request.Price != null)
            order.Price = (double)request.Price;
        if (request.FileUrls?.Count != 0)
            order.FileUrls = request.FileUrls;

        return order;
    }
    
}