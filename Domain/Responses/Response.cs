using System.Net;

namespace Domain.Responses;

public class Response<T>
{
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
    public T? Data { get; set; }

    public Response(HttpStatusCode code,List<string> errors,T data)
    {
        StatusCode = (int)code;
        Errors = errors;
        Data = data;
    }
    public Response(HttpStatusCode code,string errors,T data)
    {
        StatusCode = (int)code;
        Errors.Add(errors);
        Data = data;
    }

    
    public Response(HttpStatusCode code,List<string> errors)
    {
        StatusCode = (int)code;
        Errors = errors;
    }
    public Response(HttpStatusCode code, string errors)
    {
        StatusCode = (int)code;
        Errors.Add(errors);
    }
    public Response(HttpStatusCode code,T data)
    {
        StatusCode = (int)code;
        Data = data;
    }
    public Response(T data)
    {
        StatusCode = 200;
        Data = data;
    }
    
}