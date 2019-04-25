using Celia.io.Core.MicroServices.Utilities; 
using Celia.io.Core.StaticObjects.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.Abstractions.DTOs
{
    public class ImageElementResponseResult : ResponseResult<ImageElement>
    { 
        public string OutputUrl { get; set; }

        public string PublishOutputUrl { get; set; }
    }
}
