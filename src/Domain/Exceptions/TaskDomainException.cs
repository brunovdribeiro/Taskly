namespace Domain.Exceptions;

public class TaskDomainException : Exception
{
    public TaskDomainException() { }
    public TaskDomainException(string message) : base(message) { }
    public TaskDomainException(string message, Exception inner) : base(message, inner) { }
}