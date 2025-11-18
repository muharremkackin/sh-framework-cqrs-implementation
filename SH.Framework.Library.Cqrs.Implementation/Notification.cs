namespace SH.Framework.Library.Cqrs.Implementation;

public abstract class Notification: INotification, IHasNotificationId
{
    public Guid NotificationId => Guid.NewGuid();
}