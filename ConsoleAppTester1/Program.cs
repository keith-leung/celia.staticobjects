using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ConsoleAppTester1
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Hello World!");
        }

        static void Main0(string[] args)
        {
            Celia.io.Core.StaticObjects.OpenSDK.ImageManager imgManager
                = new Celia.io.Core.StaticObjects.OpenSDK.ImageManager(
                    "https://imageylt.chinacloudsites.cn/",
                "yltbook", "85959r9wz9r7rni9izo");

            var res0 = imgManager.Publish("5cb43f28aa012613acabbc7d");
            res0.Wait();

            Console.WriteLine(JObject.FromObject(res0.Result).ToString());

            var res2 = imgManager.GetUrlAsync("5cb43f28aa012613acabbc7d",
                Celia.io.Core.StaticObjects.OpenSDK.MediaElementUrlType.DownloadUrl, "webp", 360);
            res2.Wait();
            Console.WriteLine(JObject.FromObject(res2.Result).ToString());

            var res3 = imgManager.RevokePublish("5cb43f28aa012613acabbc7d");
            res3.Wait();


            using (StreamReader reader = new StreamReader(@"D:\OneDrive\TAR.MASSIVE\TIM截图20190311163250.png"))
            {
                var res1 = imgManager.UploadImg(reader.BaseStream, "bzgsoft", "image1", "png");
                res1.Wait();
                Console.WriteLine(JObject.FromObject(res1.Result).ToString());
            }
        }
    }
}
