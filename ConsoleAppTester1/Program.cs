﻿using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ConsoleAppTester1
{
    class Program
    {
        static void Main0(string[] args)
        {


            Console.WriteLine("Hello World!");
        }

        static void Main(string[] args)
        {
            Celia.io.Core.StaticObjects.OpenSDK.ImageManager imgManager
                = new Celia.io.Core.StaticObjects.OpenSDK.ImageManager(
                    //"http://139.198.122.46:40020",
                    //"http://localhost:63980/", 
                    "https://open.celia.io:9021/",
                "yltbook", "85959r9wz9r7rni9izo");

//            var res0 = imgManager.Publish("5ca489dbab57e0625c8bc133");
//            res0.Wait();
//
//            Console.WriteLine(JObject.FromObject(res0.Result).ToString());

            //var res2 = imgManager.GetUrlAsync("5ca4878a20467252689f6c96",
            //    Celia.io.Core.StaticObjects.OpenSDK.MediaElementUrlType.PublishOutputUrl, "webp", 360);
            //res2.Wait();
            //Console.WriteLine(JObject.FromObject(res2.Result).ToString());

            //res2 = imgManager.GetUrlAsync("5ca4878a20467252689f6c96",
            //    Celia.io.Core.StaticObjects.OpenSDK.MediaElementUrlType.OutputUrl, "webp", 360);
            //res2.Wait();
            //Console.WriteLine(JObject.FromObject(res2.Result).ToString());
            
            //var res3 = imgManager.RevokePublish("5ca4878a20467252689f6c96");
            //res3.Wait();


            using (StreamReader reader = new StreamReader(@"d:\Pictures\代煎数据库日志.png"))
            {
                var res1 = imgManager.UploadImg(reader.BaseStream, "bzgsoft", "T代煎数据库日志", "png");
                res1.Wait();
                Console.WriteLine(JObject.FromObject(res1.Result).ToString());
            }
        }
    }
}
