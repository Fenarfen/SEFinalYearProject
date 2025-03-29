using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustardRM.Common.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(string userID, string email, IEnumerable<Claim>? extraClaims);
    ClaimsPrincipal? ValidateToken(string token);
}
