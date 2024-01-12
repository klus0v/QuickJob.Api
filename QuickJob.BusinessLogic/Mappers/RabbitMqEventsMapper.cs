using QuickJob.DataModel.Postgres.Entities;
using QuickJob.DataModel.RabbitMQ;

namespace QuickJob.BusinessLogic.Mappers;

public static class RabbitMqEventsMapper
{
    public static ApprovedRespondEvent ToApprovedRespondEvent(this Response r) =>
        new(r.UserId, r.OrderId);
}