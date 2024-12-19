namespace Infra.Response;

public class Result<T>
{
    public bool isSuccess { get; set; }
    public string? StatusMessage { get; set; }
    public string? Status { get; set; }
    public T? Data { get; set; }

    public Result<T> Success(T data, string status, string statusMessage) =>
        new()
        {
            Data = data,
            Status = status,
            StatusMessage = statusMessage,
            isSuccess = true,
        };

    public Result<T> Failure(string status, string statusMessage) =>
        new()
        {
            Status = status,
            StatusMessage = statusMessage,
            isSuccess = false,
        };
}
