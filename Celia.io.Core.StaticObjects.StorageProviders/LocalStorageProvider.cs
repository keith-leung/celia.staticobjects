using Aliyun.OSS;
using Celia.io.Core.MicroServices.Utilities;
using Celia.io.Core.StaticObjects.Abstractions;
using Celia.io.Core.StaticObjects.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.StorageProviders
{
    public class LocalStorageProvider : IStorageProvider
    {
        private DisconfService _disconfService = null;

        public const string STORAGE_PROVIDER_TYPE_KEY = "STORAGE_PROVIDER_TYPE_LOCAL";
        public const string DEFAULT_CONTAINER = "defaultcontainer";
        public static readonly Type STORAGE_PROVIDER_TYPE_VALUE = typeof(LocalStorageProvider);

        public LocalStorageProvider(DisconfService disconfService)
        {
            _disconfService = disconfService ?? throw new ArgumentNullException(nameof(disconfService));
        }

        public string GetUrlByFormatSizeQuality(IStorageInfo storageInfo, MediaElementUrlType urlType, string filePath, string fileName, string format, int maxWidthHeight, int percentage)
        {
            return GetUrlCustom(storageInfo, urlType, filePath, fileName, "");
        }

        public string GetUrlByStyle(IStorageInfo storageInfo, MediaElementUrlType urlType, string filePath, string fileName, string styleName)
        {
            return "";
        }

        public string GetUrlCustom(IStorageInfo storageInfo, MediaElementUrlType urlType, string filePath, string fileName, string customStyleProcessStr)
        {
            return storageInfo.StorageHost + "/" + storageInfo.StorageAccessKey+ "/" + filePath + "/" + fileName;
        }

        public Stream GetStream(string storageId, string storageAccessKey, StorageMode mode,string downloadHost, string filePath, string fileName)
        {
            return null;
        }

        public async Task RemoveFileAsync(string storageId, string storageAccessKey,StorageMode mode, string downloadHost, string filePath, string fileName)
        {
        }

        public async Task UploadFileAsync(Stream stream, string storageId, string storageAccessKey,StorageMode mode, string downloadHost, string filePath, string fileName)
        {
            if (stream != null) {
                try
                {
                    string path = storageAccessKey + "/" + filePath + "/" + fileName;
                    string DirectoryPath = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(DirectoryPath);
                    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                         stream.CopyTo(fs);
                    }
                }
                catch (Exception ex)
                {
                    int a = 1;
                }
            }
        }
    }
}
