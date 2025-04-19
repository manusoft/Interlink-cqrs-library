namespace Interlink.Test.Notification;

public class AccountService(IPublisher publisher)
{
    public async Task RegisterUser(string username)
    {
        // Save to DB...
        await publisher.Publish(new UserCreated(username));
    }
}
