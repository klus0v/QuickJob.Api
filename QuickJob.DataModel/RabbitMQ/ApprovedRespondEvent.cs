namespace QuickJob.DataModel.RabbitMQ;

public record ApprovedRespondEvent(
    Guid UserId,
    Guid OrderId
    );