using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.StaticObjects.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.OpenAPI.Models
{
    public class InternalOpenAuthService : IOpenAppAuthService
    {
        private string _appId;
        private string _appSecret;

        public InternalOpenAuthService(string appId, string appSecret)
        {
            this._appId = appId;
            this._appSecret = appSecret;
        }

        public async Task<ResponseResult<string>> CheckAuthAsync(string appid, long ts, string sign, string path)
        {
            try
            {
                long currentTs = DateTimeUtils.GetCurrentMillisecondsTimestamp();

                if (Math.Abs(ts - currentTs) > 60 * 10 * 1000) //正负10分钟
                {
                    ResponseResult<string> result = new ResponseResult<string>();
                    result.Code = (int)HttpStatusCode.GatewayTimeout;

                    return result;
                }

                string matchedSign = HashUtils.OpenApiSign(appid, _appSecret, ts, path.ToLower());
                bool ok = matchedSign.Equals(sign, StringComparison.InvariantCultureIgnoreCase);

                if (!ok)
                {
                    return new ResponseResult<string>()
                    {
                        Code = (int)HttpStatusCode.Unauthorized,
                        Message = "The sign is not valid. "
                    };
                }

                return new ResponseResult<string>()
                {
                    Code = 200
                };
            }
            catch (Exception auth)
            {
                return new ResponseResult<string>()
                {
                    Code = 500,
                    Message = auth.Message,
                };
            }
        }
    }
}
