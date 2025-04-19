using Interlink.Abstractions;

namespace Interlink.Test.Notification;

public class UserCreated(string userName) : INotification
{
    public string UserName { get; } = userName;
}
