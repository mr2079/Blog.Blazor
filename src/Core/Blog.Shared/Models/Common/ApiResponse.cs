namespace Blog.Shared.Models.Common;

public abstract class ApiResponseBase
{
	private int _statusCode;
	public int StatusCode
	{
		get => _statusCode;
		set
		{
			_statusCode = value;

			Succeeded = _statusCode is >= 200 and < 300;
		}
	}
	public bool Succeeded { get; set; }
	public List<string> Messages { get; set; } = new();
}

public class ApiResponse : ApiResponseBase;

public class ApiResponse<TModel> : ApiResponseBase
{
	public TModel? Content { get; set; }
}