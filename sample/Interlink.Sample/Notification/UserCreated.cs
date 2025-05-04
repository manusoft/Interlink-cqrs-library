using Interlink.Contracts;

namespace Interlink.Sample.Notification;

public class UserCreated(string userName) : INotification
{
    public string UserName { get; } = userName;
}
