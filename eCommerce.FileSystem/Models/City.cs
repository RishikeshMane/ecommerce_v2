using System.ComponentModel.DataAnnotations;

namespace FileSystem.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public int CityLinkId { get; set; }
        public int StateLinkId { get; set; }
        public int CountryLinkId { get; set; }
        public string Name { get; set; }
    }
}
