using System.ComponentModel.DataAnnotations;

namespace Celia.io.Core.StaticObjects.Models
{
    public class AddImgRelationsEntity
    {
        //子级ID
        [Required]
        public string ObjectId { get; set; }
        //父级ID
        [Required]
        public string ImgId { get; set; }
    }

    public class GetImgRelationsEntity
    {
        //父级ID
        [Required]
        public string ImgId { get; set; }
    }
}
