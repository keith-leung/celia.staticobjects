using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Celia.io.Core.StaticObjects.OpenSDK
{
    public class MediaElementActionRequest
    {
        [Required]
        public string ObjectId { get; set; }
    }
}
