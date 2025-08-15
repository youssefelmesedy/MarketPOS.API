namespace MarketPOS.Shared;
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public int Status { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string? message = null)
        => new() { Success = true, Status = 0, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message, object? errors = null)
        => new() { Success = false, Status = 0, Message = message, Errors = errors };
}