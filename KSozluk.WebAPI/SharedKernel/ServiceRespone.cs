using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSozluk.WebAPI.SharedKernel
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ServiceResponse(T data, bool success = true, string message = null)
        {
            Data = data;
            Success = success;
            Message = message;
        }
    }
}
