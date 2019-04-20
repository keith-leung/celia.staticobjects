using Celia.io.Core.StaticObjects.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Celia.io.Core.StaticObjects.DataAccess
{
    public class EfCoreStaticObjectsRepository : IStaticObjectsRepository
    {
        private StaticObjectsDbContext _dbContext;
        private ILogger _logger;

        public EfCoreStaticObjectsRepository(ILogger<EfCoreStaticObjectsRepository> logger,
            StaticObjectsDbContext dbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public ImageElement FindImageElementById(string objectId)
        {
            return this._dbContext.ImageElements.Find(objectId);
        }

        public Storage FindStorageById(string storageId)
        {
            return this._dbContext.Storages.Find(storageId);
        }

        public async Task<ServiceAppStorageRelation> FindStorageRelationByIdAsync(
            string appId, string storageId)
        {
            _logger.LogWarning(
                $"EfCoreStaticObjectsRepository.FindStorageRelationByIdAsync appId={appId}, storageId={storageId}");
            //var temp = this._dbContext.ServiceAppStorageRelations.FirstOrDefault();
            //_logger.LogWarning($"FirstOrDefault {temp.Id}");
            var result = this._dbContext.ServiceAppStorageRelations.FirstOrDefault(
                m => m.AppId.Equals(appId, StringComparison.InvariantCultureIgnoreCase)
                && m.StorageId.Equals(storageId, StringComparison.InvariantCultureIgnoreCase));
            _logger.LogWarning(
                $"EfCoreStaticObjectsRepository.FindStorageRelationByIdAsync appId={appId}, storageId={storageId} result={result?.Id}");
            return result;
        }

        public async Task<ImageElementTranItem[]> GetImageTranItemsByObjectIdAsync(string objectId)
        {
            var result = this._dbContext.ImageElementTranItems.Where(
                m => m.ImageId.Equals(objectId, StringComparison.InvariantCultureIgnoreCase));

            return result.ToArray();
        }

        public void DeleteImageTranItems(string ImageId)
        {
            try
            {
                var result = this._dbContext.ImageElementTranItems.Where(
                    m => m.ImageId.Equals(ImageId, StringComparison.InvariantCultureIgnoreCase)).ToList();

                this._dbContext.ImageElementTranItems.RemoveRange(result);
                var i = _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                int a = 1;
            }

        }

        public Task PublishAsync(ImageElement element)
        {
            return this._dbContext.ImageElements.FindAsync(element.ObjectId)
                .ContinueWith(m2 =>
                {
                    if (!m2.IsFaulted && m2.Result != null)
                    {
                        m2.Result.UTIME = DateTime.Now;
                        m2.Result.IsPublished = true;

                        _dbContext.Update(m2.Result);
                        _dbContext.SaveChanges();
                    }
                });
        }

        public Task RevokePublishAsync(ImageElement element)
        {
            return this._dbContext.ImageElements.FindAsync(element.ObjectId)
                .ContinueWith(m2 =>
                {
                    if (!m2.IsFaulted && m2.Result != null)
                    {
                        m2.Result.UTIME = DateTime.Now;
                        m2.Result.IsPublished = false;

                        _dbContext.Update(m2.Result);
                        _dbContext.SaveChanges();
                    }
                });
        }

        public Task UpsertElementAsync(ImageElement element)
        {
            //try
            //{
            //    var data = this._dbContext.ImageElements.FindAsync(element.ObjectId);
            //    if (!data.IsFaulted && data.Result != null)
            //    {
            //        data.Result.UTIME = DateTime.Now;
            //        data.Result.StoreWithSrcFileName = element.StoreWithSrcFileName;
            //        data.Result.StorageId = element.StorageId;
            //        data.Result.SrcFileName = element.SrcFileName;
            //        data.Result.IsPublished = element.IsPublished;
            //        data.Result.FilePath = element.FilePath;
            //        data.Result.Extension = element.Extension;

            //        _dbContext.Update(data.Result);
            //        _dbContext.SaveChanges();
            //    }
            //    else if ((!data.IsFaulted))
            //    {
            //        ImageElement entity = new ImageElement()
            //        {
            //            ObjectId = element.ObjectId,
            //            CTIME = DateTime.Now,
            //            Extension = element.Extension,
            //            FilePath = element.FilePath,
            //            IsPublished = element.IsPublished,
            //            SrcFileName = element.SrcFileName,
            //            StorageId = element.StorageId,
            //            StoreWithSrcFileName = element.StoreWithSrcFileName,
            //        };

            //        _dbContext.ImageElements.Add(entity);
            //        _dbContext.SaveChanges();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    int a = 1;
            //}
            return this._dbContext.ImageElements.FindAsync(element.ObjectId)
                .ContinueWith(m2 =>
                {
                    if (!m2.IsFaulted && m2.Result != null)
                    {
                        m2.Result.UTIME = DateTime.Now;
                        m2.Result.StoreWithSrcFileName = element.StoreWithSrcFileName;
                        m2.Result.StorageId = element.StorageId;
                        m2.Result.SrcFileName = element.SrcFileName;
                        m2.Result.IsPublished = element.IsPublished;
                        m2.Result.FilePath = element.FilePath;
                        m2.Result.Extension = element.Extension;

                        _dbContext.Update(m2.Result);
                        _dbContext.SaveChanges();
                    }
                    else if (!m2.IsFaulted)
                    {
                        ImageElement entity = new ImageElement()
                        {
                            ObjectId = element.ObjectId,
                            CTIME = DateTime.Now,
                            Extension = element.Extension,
                            FilePath = element.FilePath,
                            IsPublished = element.IsPublished,
                            SrcFileName = element.SrcFileName,
                            StorageId = element.StorageId,
                            StoreWithSrcFileName = element.StoreWithSrcFileName,
                        };
                        _dbContext.ImageElements.Add(entity);
                        _dbContext.SaveChanges();
                    }
                });
        }
        public Task UpsertElementItemAsync(ImageElementTranItem elementItem)
        {
            //try
            //{
            //    var j = this._dbContext.ImageElementTranItems.FindAsync(elementItem.ObjectId);

            //    if (!j.IsFaulted && j.Result != null)
            //    {
            //        j.Result.UTIME = DateTime.Now;
            //        j.Result.Extension = elementItem.Extension;
            //        j.Result.FilePath = j.Result.FilePath;
            //        j.Result.ImageId = elementItem.ImageId;
            //        j.Result.ObjectId = elementItem.ObjectId;
            //        j.Result.TranItemType = elementItem.TranItemType;

            //        _dbContext.Update(j.Result);
            //        _dbContext.SaveChanges();
            //    }
            //    else if (!j.IsFaulted)
            //    {
            //        ImageElementTranItem entity = new ImageElementTranItem()
            //        {
            //            CTIME = DateTime.Now,
            //            Extension = elementItem.Extension,
            //            FilePath = elementItem.FilePath,
            //            ImageId = elementItem.ImageId,
            //            ObjectId = elementItem.ObjectId,
            //            TranItemType = elementItem.TranItemType,
            //        };
            //        _dbContext.ImageElementTranItems.Add(entity);
            //        _dbContext.SaveChanges();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    int a = 1;
            //}
            return this._dbContext.ImageElementTranItems.FindAsync(elementItem.ObjectId).
                ContinueWith(t =>
                {
                    if (!t.IsFaulted && t.Result != null)
                    {
                        t.Result.UTIME = DateTime.Now;
                        t.Result.Extension = elementItem.Extension;
                        t.Result.FilePath = elementItem.FilePath;
                        t.Result.ImageId = elementItem.ImageId;
                        t.Result.ObjectId = elementItem.ObjectId;
                        t.Result.TranItemType = elementItem.TranItemType;

                        _dbContext.Update(t.Result);
                        _dbContext.SaveChanges();
                    }
                    else if (!t.IsFaulted)
                    {
                        ImageElementTranItem entity = new ImageElementTranItem()
                        {
                            CTIME = DateTime.Now,
                            Extension = elementItem.Extension,
                            FilePath = elementItem.FilePath,
                            ImageId = elementItem.ImageId,
                            ObjectId = elementItem.ObjectId,
                            TranItemType = elementItem.TranItemType,
                        };
                        _dbContext.ImageElementTranItems.Add(entity);
                        _dbContext.SaveChanges();
                    }
                });
        }

        public ImageElement GetImageElementById(string objectId)
        {
            return _dbContext.ImageElements.Find(objectId);
        }
    }
}
