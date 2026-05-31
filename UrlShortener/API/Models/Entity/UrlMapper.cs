using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entity
{
    [Table("UrlMapper")]
    public class UrlMapper
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ActualUrl { get; set; }

        [Required]
        public string? ShortenedUrl { get; set; }
    }
}
