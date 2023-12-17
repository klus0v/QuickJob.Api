using QuickJob.DataModel.Api.Requests.Orders;
using QuickJob.DataModel.Api.Responses.Orders;
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
            ResponsesCount = order.ResponsesCount,
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
    
    public static Order ToEntity(this UpdateOrderRequest updateOrderRequest, Guid orderId)
    {
        var order = new Order
        {
            Id = orderId, 
            EditDateTime = DateTime.UtcNow
        };
        
        if (updateOrderRequest.Title != null)
            order.Title = updateOrderRequest.Title;
        if (updateOrderRequest.Description != null)
            order.Description = updateOrderRequest.Description;
        if (updateOrderRequest.Address != null)
            order.Address = updateOrderRequest.Address;
        if (updateOrderRequest.Categories != null)
            order.Categories = updateOrderRequest.Categories;
        if (updateOrderRequest.Skills != null)
            order.Skills = updateOrderRequest.Skills;
        if (updateOrderRequest.StartDateTime != null)
            order.StartDateTime = (DateTime)updateOrderRequest.StartDateTime;
        if (updateOrderRequest.EndDateTime != null)
            order.EndDateTime = (DateTime)updateOrderRequest.EndDateTime;
        if (updateOrderRequest.Limit != null)
            order.Limit = (int)updateOrderRequest.Limit;
        if (updateOrderRequest.PaymentType != null)
            order.PaymentType = (PaymentTypes)updateOrderRequest.PaymentType;
        if (updateOrderRequest.WorkHours != null)
            order.WorkHours = (double)updateOrderRequest.WorkHours;
        if (updateOrderRequest.Price != null)
            order.Price = (double)updateOrderRequest.Price;

        return order;
    }
    
}