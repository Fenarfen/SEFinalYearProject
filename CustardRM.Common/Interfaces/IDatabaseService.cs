using CustardRM.Common.Models.Entities;
using CustardRM.Common.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustardRM.Common.Interfaces;

public interface IDatabaseService
{
	public IDbConnection CreateConnection();

	public Task<IEnumerable<User>> GetUsersAsync();
	public int? VerifyLoginDetails(LoginRequest req);
    public bool DoesEmailExist(string email);
	public bool CreateUser(CreateUserRequest req, string hash, string salt);
}
