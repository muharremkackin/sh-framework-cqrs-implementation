namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class NotificationBehavior<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification, IHasNotificationId
{
    public abstract Task HandleAsync(TNotification notification, CancellationToken cancellationToken = new());
}