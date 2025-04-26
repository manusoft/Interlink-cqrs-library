namespace Interlink.Test.Notification;

public class SendWelcomeEmail : INotificationHandler<UserCreated>
{
    public Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Welcome email sent to {notification.UserName}");
        return Task.CompletedTask;
    }
}
