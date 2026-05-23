namespace ParrotShopBackend.Application.Exceptions;

[Serializable]
public class RefreshFailedException : Exception
{
    public RefreshFailedException
() : base() { }
    public RefreshFailedException
(string msg) : base(msg) { }
    public RefreshFailedException
(string msg, Exception innerException) : base(msg, innerException) { }
}