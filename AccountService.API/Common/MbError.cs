namespace AccountService.API.Common;

public class MbError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Dictionary<string, string[]> Details { get; set; } = new Dictionary<string, string[]>();

    public MbError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public MbError(string code, string message, Dictionary<string, string[]> details)
    {
        Code = code;
        Message = message;
        Details = details;
    }
}