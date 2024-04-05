using AutoMapper;
using Blog.Application.Models.SiteSetting;
using Blog.Domain.Entites;
using Blog.Domain.Enums;
using Blog.Shared.Models.Auth;
using Blog.Shared.Models.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blog.Application.Contracts.Services;
using Microsoft.Extensions.Options;

namespace Blog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
	UserManager<User> userManager,
	IMapper mapper,
	IOptionsSnapshot<SiteSettings> siteSettings,
	ITokenService tokenService) : ControllerBase
{
	#region Register

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

	#endregion

	#region Login
	
	[HttpPost("[action]", Name = "Login")]
	public async Task<ActionResult<ApiResponse<TokenModel>>> Login(LoginModel info)
	{
		var res = new ApiResponse<TokenModel>();

		var user = await userManager.FindByNameAsync(info.PhoneNumber);

		var loginRes = await userManager.CheckPasswordAsync(user!, info.Password);

		if (!loginRes)
		{
			res.StatusCode = StatusCodes.Status400BadRequest;
			res.Messages.Add("شماره موبایل یا کلمه عبور وارد شده صحیح نمی باشد");
			return Ok(res);
		}

		var userRoles = await userManager.GetRolesAsync(user!);

		var authClaims = new List<Claim>
			{
				new(ClaimTypes.Name, user?.UserName ?? string.Empty),
				new("DisplayName", $"{user?.DisplayName}"),
				new("ImageName", user?.ImageName ?? string.Empty),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

		authClaims.AddRange(userRoles.Select(role => new Claim("Role", role)));

		var token = tokenService.CreateAccessToken(authClaims);
		var refreshToken = tokenService.GenerateToken(64);

		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiryTime = DateTime.Now.AddDays(siteSettings.Value.Jwt.RefreshTokenValidityInDays);

		await userManager.UpdateAsync(user);

		res.StatusCode = StatusCodes.Status200OK;
		res.Messages.Add("احراز هویت با موفقیت انجام شد");
		res.Content = new TokenModel()
		{
			AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
			AccessTokenExpiryDateTime = token.ValidTo.ToLocalTime(),
			RefreshToken = refreshToken,
			RefreshTokenExpiryDateTime = user.RefreshTokenExpiryTime
		};

		return Ok(res);
	}

	#endregion

	#region Refresh

	[HttpPost("[action]", Name = "RefreshTokens")]
	public async Task<ActionResult<ApiResponse<TokenModel>>> Refresh(RefreshModel info)
	{
		var response = new ApiResponse<TokenModel>();

		var principal = tokenService
			.GetPrincipalFromExpiredToken(info.AccessToken);

		if (principal is null)
		{
			response.StatusCode = StatusCodes.Status400BadRequest;
			return response;
		}

		var user = await userManager.FindByNameAsync(principal.Identity?.Name!);

		if (user is null)
		{
			response.StatusCode = StatusCodes.Status404NotFound;
			return response;
		}

		if (user.RefreshToken != info.RefreshToken ||
		    user.RefreshTokenExpiryTime <= DateTime.Now)
		{
			user.RefreshToken = null;
			user.RefreshTokenExpiryTime = null;
			await userManager.UpdateAsync(user);

			response.StatusCode = StatusCodes.Status400BadRequest;
			return response;
		}

		var newAccessToken = tokenService.CreateAccessToken(principal.Claims.ToList());
		var newRefreshToken = tokenService.GenerateToken(64);

		user.RefreshToken = newRefreshToken;

		var updateRes = await userManager.UpdateAsync(user);

		if (!updateRes.Succeeded)
		{
			response.StatusCode = StatusCodes.Status500InternalServerError;
			return response;
		}

		response.StatusCode = StatusCodes.Status200OK;
		response.Content = new()
		{
			AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
			AccessTokenExpiryDateTime = newAccessToken.ValidTo.ToLocalTime(),
			RefreshToken = newRefreshToken,
			RefreshTokenExpiryDateTime = user.RefreshTokenExpiryTime
		};

		return Ok(response);
	}

	#endregion

	#region Logout

	[HttpPost("[action]", Name = "Logout")]
	public async Task<ActionResult<ApiResponse>> Logout(LogoutModel info)
	{
		var response = new ApiResponse();

		if (string.IsNullOrWhiteSpace(info.UserName))
		{
			response.StatusCode = StatusCodes.Status400BadRequest;
			return response;
		}

		var user = await userManager.FindByNameAsync(info.UserName);

		if (user is null)
		{
			response.StatusCode = StatusCodes.Status404NotFound;
			return response;
		}

		user.RefreshToken = null;
		user.RefreshTokenExpiryTime = null;
		await userManager.UpdateAsync(user);

		response.StatusCode = StatusCodes.Status200OK;

		return Ok(response);
	}

	#endregion
}