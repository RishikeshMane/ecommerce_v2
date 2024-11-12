using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class SizeDetail
    {
        public int sizeId { get; set; }
        public int SizeLinkId { get; set; }
        public string Sizecode { get; set; }
        public string Description { get; set; }
    }
}
