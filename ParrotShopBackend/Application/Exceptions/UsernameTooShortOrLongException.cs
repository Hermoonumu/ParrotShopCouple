namespace ParrotShopBackend.Application.Exceptions;

[Serializable]
public class UsernameTooShortOrLongException : Exception
{
    public UsernameTooShortOrLongException() : base() { }
    public UsernameTooShortOrLongException(string msg) : base(msg) { }
    public UsernameTooShortOrLongException(string msg, Exception innerException) : base(msg, innerException) { }
}