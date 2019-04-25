using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Celia.io.Core.StaticObjects.Abstractions.DTOs
{
    public class GetUrlsBySizeRequest
    {
        [Required]
        public string[] ObjectIds
        {
            get;
            set;
        }

        [Required]
        public int Type
        {
            get; set;
        }

        public string Format
        {
            get;
            set;
        }

        public int? MaxWidthHeight
        {
            get;
            set;
        }

        public int? Percentage
        {
            get;
            set;
        }
    }
}
