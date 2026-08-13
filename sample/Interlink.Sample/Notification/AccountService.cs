namespace Interlink.Sample.Notification;

public class AccountService(IMediator mediator)
{
    public async Task RegisterUser(string username)
    {
        // Save to DB...
        await mediator.Publish(new UserCreated(username));
    }
}
