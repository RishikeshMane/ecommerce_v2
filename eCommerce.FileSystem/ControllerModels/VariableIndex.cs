using System.ComponentModel.DataAnnotations.Schema;

namespace FileSystem.ControllerModels
{
    [NotMapped]
    public class VariableIndex
    {
        public string ProductId { get; set; }
        public int Index { get; set; }
        public List<string> ImagesUrls { get; set; } = new List<string>();
    }
}
