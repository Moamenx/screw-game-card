namespace ScrewGameCard.DomainShared;

public static class DomainErrorCodes
{
    // HTTP Status Codes
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int Forbidden = 403;
    public const int NotFound = 404;

    // Application Specific Codes
    public const int UsernameAlreadyExists = 1001;
    public const int InvalidCredentials = 1002;
    public const int InvalidRefreshToken = 1003;
    public const int PlayerNotFound = 1004;
}