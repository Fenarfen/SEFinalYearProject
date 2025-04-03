using CustardRM.Common.Models.DTOs;
using CustardRM.Common.Models.Entities;
using CustardRM.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CustardRM.Common.Models.Entities.Inventory;

namespace CustardRM.Common.Interfaces;

public interface IDatabaseService
{
	public IDbConnection CreateConnection();

	public Task<IEnumerable<User>> GetUsersAsync();
	public int? VerifyLoginDetails(LoginRequest req);
    public bool DoesEmailExist(string email);
	public bool CreateUser(CreateUserRequest req, string hash, string salt);
    public List<StockItem> GetStockItems();
	public List<Category> GetCategories();
	public Category GetCategoryByID(int id);
	public List<Category> GetLinkedCategoriesByStockItemID(int id);
}
