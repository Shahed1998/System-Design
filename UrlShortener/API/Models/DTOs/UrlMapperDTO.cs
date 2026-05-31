using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UrlMapperDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? ActualUrl { get; set; }

        [Required]
        public string? ShortenedUrl { get; set; }
    }
}
