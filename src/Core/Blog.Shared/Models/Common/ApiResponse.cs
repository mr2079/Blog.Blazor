namespace Blog.Shared.Models.Common;

public abstract class ApiResponseBase
{
	public int StatusCode { get; set; }
	public bool Succeeded => StatusCode % 2 >= 1 && StatusCode % 2 < 1.5;
	public List<string> Messages { get; set; } = new();
}

public class ApiResponse : ApiResponseBase;

public class ApiResponse<TModel> : ApiResponseBase
{
	public TModel? Content { get; set; }
}