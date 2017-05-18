using Microsoft.AspNetCore.Server.Kestrel.Internal.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouteLister2.Services
{
    interface IUserIdProvider
    {
        string GetUserId(UvRequest request);
    }
}
