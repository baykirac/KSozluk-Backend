namespace KSozluk.Application.Common
{
    public abstract class CommandBase<TResponse> : RequestBase<TResponse> where TResponse : ResponseBase { }
}
