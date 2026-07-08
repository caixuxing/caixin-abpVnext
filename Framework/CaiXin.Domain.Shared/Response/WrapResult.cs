namespace CaiXin.Domain.Shared.Response;

public class WrapResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public int Code { get; set; }
    public string TraceId { get; set; }
    public DateTime Timestamp { get; set; }

    public WrapResult()
    {
        Success = true;
        Message = "Success";
        Data = default(T);
        Code = 200;
        TraceId = Guid.NewGuid().ToString("N");
        Timestamp = DateTime.UtcNow;
    }

    public WrapResult(bool success, string message, T data = default, int code = 200, string traceId = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Code = code;
        TraceId = traceId ?? Guid.NewGuid().ToString("N");
        Timestamp = DateTime.UtcNow;
    }

    public void SetSuccess(T data, string message = "Success", int code = 200, string traceId = null)
    {
        Success = true;
        Data = data;
        Message = message;
        Code = code;
        TraceId = traceId ?? Guid.NewGuid().ToString("N");
        Timestamp = DateTime.UtcNow;
    }

    public void SetFail(string message = "Fail", int code = 500, string traceId = null)
    {
        Success = false;
        Message = message;
        Data = default(T);
        Code = code;
        TraceId = traceId ?? Guid.NewGuid().ToString("N");
        Timestamp = DateTime.UtcNow;
    }

    public void SetFailWithData(T data, string message = "Fail", int code = 500, string traceId = null)
    {
        Success = false;
        Data = data;
        Message = message;
        Code = code;
        TraceId = traceId ?? Guid.NewGuid().ToString("N");
        Timestamp = DateTime.UtcNow;
    }

    public void SetData(T data)
    {
        Data = data;
    }
}