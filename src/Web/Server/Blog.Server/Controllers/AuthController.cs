using AutoMapper;
using Blog.Domain.Entites;
using Blog.Domain.Enums;
using Blog.Shared.Models.Auth;
using Blog.Shared.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
	UserManager<User> userManager,
	IMapper mapper) : ControllerBase
{
	[HttpPost("[action]", Name = "Register")]
	public async Task<ActionResult<ApiResponse>> Register(RegisterModel info)
	{
		var res = new ApiResponse();

		var existedUser = await userManager.FindByNameAsync(info.PhoneNumber);
		if (existedUser != null)
		{
			res.StatusCode = StatusCodes.Status400BadRequest;
			res.Messages.Add("کاربری با شماره موبایل وارد شده در سامانه موجود می باشد");
			return Ok(res);
		}

		var user = mapper.Map<User>(info);

		var createResult = await userManager.CreateAsync(user, info.Password);

		if (!createResult.Succeeded)
		{
			res.StatusCode = StatusCodes.Status500InternalServerError;
			res.Messages.Add("لطفا مجددا تلاش کنید");
			return Ok(res);
		}

		await userManager.AddToRoleAsync(user, RoleEnum.User.ToString());

		res.StatusCode = StatusCodes.Status200OK;
		res.Messages.Add("ثبت نام با موفقیت انجام شد");

		return Ok(res);
	}

	//[HttpPost("[action]", Name = "Login")]
	//public async Task<ActionResult<ApiResponse<TokenModel>>> Login(LoginModel info)
	//{
	//	return Ok();
	//}
}