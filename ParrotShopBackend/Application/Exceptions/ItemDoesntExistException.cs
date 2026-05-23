namespace ParrotShopBackend.Application.Exceptions;

[Serializable]
public class ItemDoesntExistException : Exception
{
    public ItemDoesntExistException() : base() { }
    public ItemDoesntExistException(string msg) : base(msg) { }
    public ItemDoesntExistException(string msg, Exception innerException) : base(msg, innerException) { }
}