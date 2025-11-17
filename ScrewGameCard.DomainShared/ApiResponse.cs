namespace ScrewGameCard.DomainShared;

public enum ApiResponseStatus
{
    Success,
    Error
}

public class ApiResponse<T>
{
    public ApiResponseStatus Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public string? CorrelationId { get; set; }
    public int? ErrorCode { get; set; }

    public static ApiResponse<T> Success(T data, string? message = null, string? correlationId = null)
    {
        return new ApiResponse<T>
        {
            Status = ApiResponseStatus.Success,
            Data = data,
            Message = message,
            CorrelationId = correlationId
        };
    }

    public static ApiResponse<T> Error(string message, string? correlationId = null, int? errorCode = null)
    {
        return new ApiResponse<T>
        {
            Status = ApiResponseStatus.Error,
            Message = message,
            CorrelationId = correlationId,
            ErrorCode = errorCode
        };
    }
}