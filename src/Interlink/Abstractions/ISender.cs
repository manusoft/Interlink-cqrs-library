using Interlink.Contracts;

namespace Interlink;

/// <summary>  
/// Defines a sender that can send requests and receive responses.  
/// </summary>  
public interface ISender
{
    /// <summary>  
    /// Sends a request and returns a response of the specified type.  
    /// </summary>  
    /// <typeparam name="TResponse">The type of the response.</typeparam>  
    /// <param name="request">The request to send.</param>  
    /// <param name="cancellationToken">A token to cancel the operation.</param>  
    /// <returns>A task that represents the asynchronous operation. The task result contains the response.</returns>  
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
