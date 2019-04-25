using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.MicroServices.Utilities.Webs.Clients;
using Celia.io.Core.StaticObjects.Abstractions;
using Celia.io.Core.StaticObjects.Abstractions.DTOs;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.OpenSDK
{
    public class ImageManager
    {
        private WebClient _client = null;
        private string _hostAndPort;
        private string _appId;
        private string _appSecret;

        public ImageManager(string hostAndPort, string appId, string appSecret)
        {
            this._hostAndPort = hostAndPort ?? throw new ArgumentNullException(nameof(hostAndPort));
            this._appId = appId ?? throw new ArgumentNullException(nameof(appId));
            this._appSecret = appSecret ?? throw new ArgumentNullException(nameof(appSecret));

            _client = new WebClient();
        }

        public async Task<ImageElementResponseResult> UploadImg(Stream file,
            string storageId, string fileName, string extension,
            string objectId = "", string filePath = "")
        {
            //    [FromQuery]
            //[Required]
            //string storageId,
            //    [FromQuery] string objectId = "", [FromQuery] string extension = "",
            //    [FromQuery] string filePath = ""
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback
                = (request, cert, chain, errors) =>
            {
                return true;
            };

            HttpClient client = new HttpClient(httpClientHandler);

            client.BaseAddress = new Uri(this._hostAndPort);
            client.DefaultRequestHeaders.Add("appId", this._appId);
            long dt = Celia.io.Core.MicroServices.Utilities.DateTimeUtils
                .GetCurrentMillisecondsTimestamp();
            string appsecret = this._appSecret;
            string path = "/api/images/uploadimg";
            client.DefaultRequestHeaders.Add("ts", dt.ToString());

            string sign = HashUtils.OpenApiSign(this._appId,
                this._appSecret, dt, path);
            //{appsecret}!@{appid}#{timestamp}&{content}
            client.DefaultRequestHeaders.Add("sign", sign);

            using (var content = new MultipartFormDataContent())
            {
                //public IFormFile FormFile { get; set; } 
                //public string ObjectId { get; set; } 
                //[Required]
                //public string StorageId { get; set; } 
                //public string FilePath { get; set; } 
                //public string Extension { get; set; }

                content.Add(new StreamContent(file)
                {
                    Headers =
                                {
                                    ContentLength = file.Length,
                                    ContentType = new MediaTypeHeaderValue($"image/{extension}")
                                }
                }
                , "FormFile", fileName);

                UriBuilder builder = new UriBuilder(this._hostAndPort);
                builder.Path = path;
                StringBuilder sb = new StringBuilder();
                sb.Append($"storageId={storageId}");
                if (!string.IsNullOrEmpty(objectId))
                {
                    sb.Append($"&objectId={objectId}");
                }
                if (!string.IsNullOrEmpty(extension))
                {
                    sb.Append($"&extension={extension}");
                }
                if (!string.IsNullOrEmpty(filePath))
                {
                    sb.Append($"filePath={filePath}");
                }
                builder.Query = sb.ToString();

                var response = await client.PostAsync(builder.Uri.ToString(), content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception($"{response.StatusCode}  {response.ReasonPhrase}");
                }

                string result = await response.Content.ReadAsStringAsync();
                var resObj = JObject.Parse(result).ToObject(typeof(ImageElementResponseResult));
                if (resObj != null && resObj is ImageElementResponseResult)
                    return resObj as ImageElementResponseResult;
            }

            return new ImageElementResponseResult()
            {
                Code = 200,
            };
        }

        public async Task<UrlResponseResult> Publish(string objectId)
        {
            long dt = Celia.io.Core.MicroServices.Utilities.DateTimeUtils
                .GetCurrentMillisecondsTimestamp();
            string appsecret = this._appSecret;
            string path = "/api/images/publish";

            string sign = HashUtils.OpenApiSign(this._appId,
                this._appSecret, dt, path);
            //{appsecret}!@{appid}#{timestamp}&{content} 

            var res = await _client.Post(
                $"{_hostAndPort.ToString().Trim(new char[] { })}/api/images/publish")
                .Header("appId", this._appId).Header("ts", dt).Header("sign", sign)
                .JsonData<MediaElementActionRequest>(new MediaElementActionRequest() { ObjectId = objectId })
                .ResultAsync();

            var obj = JObject.Parse(res).ToObject(typeof(UrlResponseResult));
            return obj as UrlResponseResult;
        }

        public async Task<string> RevokePublish(string objectId)
        {
            long dt = Celia.io.Core.MicroServices.Utilities.DateTimeUtils
                .GetCurrentMillisecondsTimestamp();
            string appsecret = this._appSecret;
            string path = "/api/images/revokepublish";

            string sign = HashUtils.OpenApiSign(this._appId,
                this._appSecret, dt, path);
            //{appsecret}!@{appid}#{timestamp}&{content} 

            var res = await _client.Post(
                $"{_hostAndPort.ToString().Trim(new char[] { })}/api/images/revokepublish")
                .Header("appId", this._appId).Header("ts", dt).Header("sign", sign)
                .JsonData<MediaElementActionRequest>(new MediaElementActionRequest() { ObjectId = objectId })
                .ResultAsync();

            return res;
        }

        public async Task<UrlResponseResult> GetUrlAsync(string objectId, MediaElementUrlType type,
            string format = "", int maxWidthHeight = 10000, int percentage = 100)
        {
            long dt = Celia.io.Core.MicroServices.Utilities.DateTimeUtils
                .GetCurrentMillisecondsTimestamp();
            string appsecret = this._appSecret;
            string path = "/api/images/geturlbysize";

            string sign = HashUtils.OpenApiSign(this._appId,
                this._appSecret, dt, path);
            //{appsecret}!@{appid}#{timestamp}&{content} 

            var request = _client.Get(
                $"{_hostAndPort.ToString().Trim(new char[] { })}/api/images/geturlbysize")
                .Header("appId", this._appId).Header("ts", dt).Header("sign", sign)
                .UrlData("objectId", objectId).UrlData("type", (int)type);

            if (!string.IsNullOrEmpty(format))
            {
                request = request.UrlData("format", format);
            }
            if (maxWidthHeight < 4096)
            {
                request = request.UrlData("maxWidthHeight", maxWidthHeight);
            }
            if (percentage > 0 && percentage < 100)
            {
                request = request.UrlData("percentage", percentage);
            }

            //[FromQuery] [Required] string objectId,
            //    [FromQuery]
            //[Required]
            //int type,
            //    [FromQuery] string format = "",
            //    [FromQuery] int maxWidthHeight = 10000,
            //    [FromQuery] int percentage = 100
            //

            var res = await request.ResultAsync();

            var obj = JObject.Parse(res).ToObject(typeof(UrlResponseResult));
            return obj as UrlResponseResult;
        }

        public async Task<UrlsResponseResult> GetUrlsAsync(string[] objectIds, MediaElementUrlType type,
            string format = "", int maxWidthHeight = 10000, int percentage = 100)
        {
            long dt = DateTimeUtils.GetCurrentMillisecondsTimestamp();
            string appsecret = this._appSecret;
            string path = "/api/images/geturlsbysize";

            string sign = HashUtils.OpenApiSign(this._appId,
                this._appSecret, dt, path);
            //{appsecret}!@{appid}#{timestamp}&{content} 

            var res = await _client.Post(
                $"{_hostAndPort.ToString().Trim(new char[] { })}/api/images/geturlsbysize")
                .Header("appId", this._appId).Header("ts", dt).Header("sign", sign)
                .JsonData<GetUrlsBySizeRequest>(new GetUrlsBySizeRequest()
                {
                    ObjectIds = objectIds,
                    Type = (int)type,
                    Format = format,
                    MaxWidthHeight = maxWidthHeight,
                    Percentage = percentage
                })
                .ResultAsync();

            var obj = JObject.Parse(res).ToObject(typeof(UrlsResponseResult));
            return obj as UrlsResponseResult;
        }
    }
}
