using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using CustardRM.Common.Interfaces;
using CustardRM.Common.Models.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using CustardRM.Common.Models.Requests;
using CustardRM.Common.Services;
using Azure.Core;

namespace CustardRM.Common.Services;

public class DatabaseService : IDatabaseService
{
	private readonly IConfiguration _config;
	private readonly string _connString;
	private readonly PasswordHasher _passwordHasher;

	public DatabaseService(IConfiguration config)
	{
		_config = config;
		_connString = _config.GetConnectionString("DefaultConnection");
		_passwordHasher = new PasswordHasher();
	}

	public IDbConnection CreateConnection() => new SqlConnection(_connString);

	public async Task<IEnumerable<User>> GetUsersAsync()
	{
		using var connection = CreateConnection();
		var sql = "SELECT * FROM Users";
		return await connection.QueryAsync<User>(sql);
	}

	public int? VerifyLoginDetails(LoginRequest req)
	{
		if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
		{
			return null;
		}

		using var connection = CreateConnection();
		var sql = @"SELECT ID, PasswordHash, PasswordSalt FROM [User] WHERE Email = @Email";

		var result = connection.QueryFirstOrDefault(sql, new { req.Email });

		if (result == null)
		{
			Console.WriteLine($"{req.Email} does not exist");
			return null;
		}

        Console.WriteLine($"Found {req.Email}");

		int ID = result.ID;
        string passwordHash = result.PasswordHash;
        string salt = result.PasswordSalt;

		Console.WriteLine($"Retreieved data: hash:{passwordHash} salt:{salt}");

        var passwordResult = _passwordHasher.VerifyPassword(req.Password, passwordHash, salt);

        Console.WriteLine($"Password verification result: {passwordResult}");

		if (!passwordResult) return null;

        return ID;
	}

	public bool DoesEmailExist(string email)
	{
		if (string.IsNullOrEmpty(email))
		{
			return false;
		}

		using var connection = CreateConnection();
		var sql = @"SELECT COUNT(*) FROM [User] WHERE Email = @Email";

		var result = connection.Query<int>(sql, new
		{
			Email = email,
		}).FirstOrDefault();

		return result > 0;
	}

	public bool CreateUser(CreateUserRequest req, string PasswordHash, string PasswordSalt)
	{
		using var connection = CreateConnection();

		var sql = @"INSERT INTO [User] (FirstName, LastName, Email, PasswordHash, PasswordSalt, PhoneNumber, CreatedAt, UpdatedAt)
					VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt, @PhoneNumber, @CreatedAt, null)";

		var param = new
		{
			req.FirstName,
			req.LastName,
			req.Email,
			PasswordHash,
			PasswordSalt,
			PhoneNumber = req.PhoneNumber?? null,
			CreatedAt = DateTime.UtcNow
		};

		int result = connection.Execute(sql, param);

		return result > 0;
	}
}
