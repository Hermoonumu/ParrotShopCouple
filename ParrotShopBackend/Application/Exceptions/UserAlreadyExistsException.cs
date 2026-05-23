namespace ParrotShopBackend.Application.Exceptions;

[Serializable]
public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException() : base() { }
    public UserAlreadyExistsException(string msg) : base(msg) { }
    public UserAlreadyExistsException(string msg, Exception innerException) : base(msg, innerException) { }
}