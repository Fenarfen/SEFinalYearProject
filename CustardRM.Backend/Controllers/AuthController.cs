using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using CustardRM.Common.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.RateLimiting;
using CustardRM.Common.Interfaces;
using CustardRM.Common.Models.Entities;
using Azure.Core;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace CustardRM.Backend.Controllers;

public class AuthController : Controller
{
	private readonly IDatabaseService _databaseService;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IJwtTokenService _jwtTokenService;

	public AuthController(IDatabaseService databaseService, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
	{
		_databaseService = databaseService;
		_passwordHasher = passwordHasher;
		_jwtTokenService = jwtTokenService;
	}
	public IActionResult Index()
	{
		return View();
	}

	[HttpPost("api/user/signin")]
	[EnableRateLimiting("fixed")]
	public IActionResult SignIn([FromBody] Common.Models.Requests.LoginRequest request)
	{
		try
		{
			Console.WriteLine("Login request receieved at " + DateTime.UtcNow);

			if (request == null ||
				string.IsNullOrEmpty(request.Email) ||
				string.IsNullOrEmpty(request.Password)) 
			{
				return BadRequest("Missing Required Fields");
			}

			int? userID = _databaseService.VerifyLoginDetails(request);

			var token = _jwtTokenService.GenerateToken(userID.ToString(), request.Email, null);

            if (userID != null)
			{
				return Ok(new { token });
			}
			else
			{
				return Unauthorized(new { message = "Incorrect email or password, please try again." });
			}
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "An error occurred while processing your request. Please try again later.\n" + ex.ToString() });
		}
	}

	[HttpPost("api/user/create-account")]
	public IActionResult CreateAccount([FromBody] Common.Models.Requests.CreateUserRequest request)
	{
		try
		{
			if (request == null ||
				string.IsNullOrEmpty(request.FirstName) ||
				string.IsNullOrEmpty(request.LastName) ||
				string.IsNullOrEmpty(request.Email) ||
				string.IsNullOrEmpty(request.Password))
			{
				return BadRequest(new { message = "Required data is missing" });
			}

			var (hash, salt) = _passwordHasher.HashPassword(request.Password);

			var result = _databaseService.CreateUser(request, hash, salt);

			return Ok(new { message = result });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "An error occurred while processing your request. Please try again later.\n" + ex.ToString() });
		}
	}

	[HttpGet("api/user/does-email-exist/{email}")]
	public IActionResult DoesEmailExistEndpoint(string email)
	{
		try
		{
			bool result = _databaseService.DoesEmailExist(email);

			return Ok(new { message = result });
		}
		catch (Exception ex)
		{
			return StatusCode(500, new { message = "An error occurred while processing your request. Please try again later.\n" + ex.ToString() });
		}
	}
}
