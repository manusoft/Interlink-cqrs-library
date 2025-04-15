﻿namespace Interlink;

internal class Sender(IServiceProvider provider) : ISender
{
    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for '{handlerType.FullName}' not found.");
        }

        return handler.Handle((dynamic)request, cancellationToken);
    }
}
