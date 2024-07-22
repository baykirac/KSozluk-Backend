using MediatR;

namespace KSozluk.Application.Common
{
    public abstract class RequestBase<TResponse> : IRequest<TResponse> where TResponse : ResponseBase { }
}
