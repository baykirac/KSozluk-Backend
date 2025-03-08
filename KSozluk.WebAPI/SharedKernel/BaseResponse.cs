using System;

namespace KSozluk.WebAPI.SharedKernel;

public class BaseResponse<T>
{
    public T Data { get; set; }

    public bool Success { get; set; }
    public string Message { get; set; }

    public BaseResponse(bool success, string message, T data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public BaseResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}

