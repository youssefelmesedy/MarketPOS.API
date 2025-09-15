// This class represents a result that can be returned from operations, encapsulating success status, message, data, and errors. 
namespace MarketPOS.Shared.DTOs;
public class ResultDto<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }

    public static ResultDto<T> Ok(string message, T? data)
        => new() { IsSuccess = true, Message = message, Data = data };

    public static ResultDto<T> Fail(string message, object? errors = null)
        => new() { IsSuccess = false, Message = message, Errors = errors };
}


