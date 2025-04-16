namespace Interlink.Test.Exceptions;

// Added NotFoundException class  
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}
