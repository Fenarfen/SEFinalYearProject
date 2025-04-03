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
using CustardRM.Common.Models.DTOs;
using static CustardRM.Common.Models.Entities.Inventory;

namespace CustardRM.Common.Services;

public class DatabaseService : IDatabaseService
{
	private readonly IConfiguration _config;
	private readonly string _connString;
	private readonly PasswordHasher _passwordHasher;

	public DatabaseService(IConfiguration config)
	{
		_config = config;
		_connString = _config.GetConnectionString("DefaultConnection")?? 
			throw new Exception("Cannot find DefaultConnection in appsettings.json");
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

    public List<Inventory.StockItem> GetStockItems()
    {
        using var connection = CreateConnection();

        var sql = @"SELECT * FROM StockItem";

        var stockItems = connection.Query<StockItem>(sql).ToList();

        return stockItems;
    }

    public Inventory.StockItem GetStockItemByID(int id)
    {
        using var connection = CreateConnection();

        var sql = @"SELECT * FROM StockItem WHERE ID = @ID";

        var stockItem = connection.Query<StockItem>(sql, new { ID = id }).FirstOrDefault();

        return stockItem;
    }

	public List<Category> GetCategories()
	{
        using var connection = CreateConnection();

        var sql = @"SELECT * FROM Category";

        var result = connection.Query<Category>(sql).ToList();

        return result;
    }

	public Category GetCategoryByID(int id)
	{
        using var connection = CreateConnection();

        var sql = @"SELECT * FROM Category where CategoryID = @CategoryID";

        var category = connection.Query<Category>(sql, new { CategoryID = id }).FirstOrDefault();

        return category;
    }

	public List<Category> GetLinkedCategoriesByStockItemID(int id)
	{
        using var connection = CreateConnection();

        var sql = @"select c.* from StockItem si
					join StockItemCategoryLink cl on si.ID = cl.StockItemID
					join Category c on cl.CategoryID = c.CategoryID
					where si.ID = @ID";

        var categories = connection.Query<Category>(sql, new { ID = id }).ToList();

        return categories;
    }

    public List<Subcategory> GetSubcategoriesByCategoryID(int CategoryID)
	{
        using var connection = CreateConnection();

        var sql = @"select * from Subcategory where CategoryID = @CategoryID";

        var subcategories = connection.Query<Subcategory>(sql, new { CategoryID = CategoryID }).ToList();

        return subcategories;
    }
}
