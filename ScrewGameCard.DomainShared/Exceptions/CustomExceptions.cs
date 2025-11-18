namespace ScrewGameCard.DomainShared.Exceptions;

public class AppException : Exception
{
    public int ErrorCode { get; }

    public AppException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message, 400) { }
    public BadRequestException(string message, int errorCode) : base(message, errorCode) { }
}

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message, 401) { }
    public UnauthorizedException(string message, int errorCode) : base(message, errorCode) { }
}

public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message, 403) { }
    public ForbiddenException(string message, int errorCode) : base(message, errorCode) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string message) : base(message, 404) { }
    public NotFoundException(string message, int errorCode) : base(message, errorCode) { }
}