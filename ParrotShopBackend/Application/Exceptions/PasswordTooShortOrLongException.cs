namespace ParrotShopBackend.Application.Exceptions;

[Serializable]
public class PasswordTooShortOrLongException : Exception
{
    public PasswordTooShortOrLongException
() : base() { }
    public PasswordTooShortOrLongException
(string msg) : base(msg) { }
    public PasswordTooShortOrLongException
(string msg, Exception innerException) : base(msg, innerException) { }
}