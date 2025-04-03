using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustardRM.Common.Interfaces
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> SendGet(string url);
        Task<HttpResponseMessage> SendPost(string url, object data);
    }
}
