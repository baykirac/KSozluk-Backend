using KSozluk.WebAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSozluk.WebAPI.SharedKernel
{
    public class ServiceResponse
    {
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
