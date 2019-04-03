using Celia.io.Core.MicroServices.Utilities;
using System;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.Services
{
    public interface IOpenAppAuthService
    {
        Task<ResponseResult<string>> CheckAuthAsync(string appid, long ts, string sign, string value);
    }
}
