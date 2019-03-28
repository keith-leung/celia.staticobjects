using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.StaticObjects.Abstractions;
using Celia.io.Core.StaticObjects.Services; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Celia.io.Core.StaticObjects.OpenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IImageService _imageService;
        private readonly IStorageService _storageService;
        private readonly IServiceAppService _serviceAppService;

        public ImagesController(ILogger<ImagesController> logger, IServiceAppService serviceAppService,
            IImageService imageService, IStorageService storageService)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this._storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            this._serviceAppService = serviceAppService ?? throw new ArgumentNullException(nameof(serviceAppService));
        }

        // GET api/images
        [HttpHead]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost("uploadimg")]
        public async Task<ImageElementResponseResult> UploadImgAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            IFormFile formFile, [FromQuery] [Required] string storageId,
            [FromQuery] string objectId = "", [FromQuery] string extension = "",
            [FromQuery] string filePath = "")
        //[FromBody] UploadImgRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ImageElementResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new ImageElementResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                if (!_storageService.ValidStorage(storageId))
                {
                    return new ImageElementResponseResult()
                    {
                        Code = (int)HttpStatusCode.InsufficientStorage, // 507
                        Message = "Storage is invalid. "
                    };
                }
                if (string.IsNullOrEmpty(objectId))
                {
                    objectId = IdGenerator.GenerateObjectId().ToString();
                }
                if (string.IsNullOrEmpty(extension) && !string.IsNullOrEmpty(formFile.FileName))
                {
                    extension = Path.GetExtension(formFile.FileName).Trim('.');
                }
                ImageElement element = null;
                using (var stream = formFile.OpenReadStream())
                {
                    _logger.LogInformation("BR.StaticObjects.WebAPI_Core.ImagesController.UploadImg: "
                        + $"storageId={storageId}, fileSize={stream.Length}, extension={extension}, srcFile={formFile.FileName}");
                    element = await _imageService.UploadImgAsync(appId, stream, storageId, objectId, extension, filePath, formFile.FileName);
                }
                return new ImageElementResponseResult()
                {
                    Code = 200,
                    Data = element,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.UploadImg");
                return new ImageElementResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("findbyobjectid")]
        public async Task<ImageGroupResponseResult> FindByObjectIdAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromQuery] [Required] string objectId)
        {
            try
            {
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new ImageGroupResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                return new ImageGroupResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.FindImgByIdAsync(objectId)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.FindByObjectIdAsync");
                return new ImageGroupResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("geturlbysize")]
        public async Task<UrlResponseResult> GetUrlAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromQuery] [Required] string objectId,
            [FromQuery] [Required] int type,
            [FromQuery] string format = "",
            [FromQuery] int maxWidthHeight = 10000,
            [FromQuery] int percentage = 100)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                return new UrlResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.GetUrlAsync(objectId, (MediaElementUrlType)type, format, maxWidthHeight, percentage)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.GetUrlAsync");
                return new UrlResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("geturlbystyle")]
        public async Task<UrlResponseResult> GetUrlAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromQuery] [Required] string objectId,
            [FromQuery] [Required] int type, string styleName = "")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                return new UrlResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.GetUrlAsync(objectId, (MediaElementUrlType)type, styleName)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.GetUrlAsync");
                return new UrlResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("geturlcustom")]
        public async Task<UrlResponseResult> GetUrlCustomAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromQuery] [Required] string objectId,
            [FromQuery] [Required] int type, string customStyleProcessStr = "")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                return new UrlResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.GetUrlCustomAsync(objectId, (MediaElementUrlType)type, customStyleProcessStr)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.GetUrlCustomAsync");
                return new UrlResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("publish")]
        public async Task<UrlResponseResult> PublishAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromBody] MediaElementActionRequest element)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new UrlResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                await _imageService.PublishAsync(appId, element.ObjectId);
                return new UrlResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.GetUrlAsync(element.ObjectId, MediaElementUrlType.PublishOutputUrl, string.Empty)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.PublishAsync");
                return new UrlResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("revokepublish")]
        public async Task<ActionResponseResult> RevokePublishAsync(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromBody] MediaElementActionRequest element)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ActionResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new ActionResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                await _imageService.RevokePublishAsync(appId, element.ObjectId);
                return new ActionResponseResult()
                {
                    Code = 200,
                    Data = await _imageService.GetUrlAsync(element.ObjectId, MediaElementUrlType.PublishOutputUrl, string.Empty)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.RevokePublishAsync");
                return new ActionResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpPost("AddImgRelations")]
        public async Task<ActionResponseResult> AddImgRelations(
            [FromHeader] [Required] string appId,
            [FromHeader] [Required] string appSecret,
            [FromBody] AddImgRelationsEntity RelationsEntity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ActionResponseResult()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new ActionResponseResult()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                var Result = await _imageService.AddImgRelationsAsync(RelationsEntity.ImgId, RelationsEntity.ObjectId);
                return new ActionResponseResult()
                {
                    Code = 200,
                    Data = Result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.RevokePublishAsync");
                return new ActionResponseResult()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }

        [HttpGet("geturlbybatch")]
        public async Task<UrlBatchEntity> GetUrlAsyncBatch(
        [FromHeader] [Required] string appId,
        [FromHeader] [Required] string appSecret,
        [FromQuery] [Required] string objectId,
        [FromQuery] [Required] int type,
        [FromQuery] string format = "",
        [FromQuery] int maxWidthHeight = 10000,
        [FromQuery] int percentage = 100)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new UrlBatchEntity()
                    {
                        Code = (int)HttpStatusCode.BadRequest, //400
                    };
                }
                if (!_serviceAppService.IsValid(appId, appSecret))
                {
                    return new UrlBatchEntity()
                    {
                        Code = (int)HttpStatusCode.Unauthorized, // 401
                        Message = "AppId, AppSecret is not authorized, or. "
                    };
                }
                string[] objectIdArray = objectId.Split('|');
                Hashtable hashtable = new Hashtable();
                foreach (var item in objectIdArray)
                {
                    if (String.IsNullOrEmpty(item)) { continue; }
                    else
                    {
                        hashtable.Add(item, await _imageService.GetUrlAsync(item, (MediaElementUrlType)type, format, maxWidthHeight, percentage));
                    }
                }
                return new UrlBatchEntity()
                {
                    Code = 200,
                    Data = hashtable
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(), ex, "BR.StaticObjects.WebAPI_Core.ImagesController.GetUrlAsync");
                return new UrlBatchEntity()
                {
                    Code = 500,
                    Message = ex.Message,
                };
            }
        }
    }
}