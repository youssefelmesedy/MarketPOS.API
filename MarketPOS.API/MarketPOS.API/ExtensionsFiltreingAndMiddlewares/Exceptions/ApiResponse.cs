namespace MarketPOS.Shared;
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> SuccessResult(T data, int status = 200, string? message = null)
        => new() { Success = true, Status = status, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message, int status = 400, object? errors = null)
        => new() { Success = false, Status = status, Message = message, Errors = errors };
}