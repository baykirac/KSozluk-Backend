namespace KSozluk.Application.Common
{
    public abstract class QueryBase<TResponse> : RequestBase<TResponse> where TResponse : ResponseBase { }
}
