namespace BibliotecaAPI.Exceptions;

public class BusinessConflictException : Exception
{
    public BusinessConflictException(string message)
        : base(message)
    {
    }
}