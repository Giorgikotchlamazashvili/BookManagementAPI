using System.ComponentModel.DataAnnotations;

namespace BookManagementAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Range(0, int.MaxValue)]
        public int PublicationYear { get; set; }
        [Required]
        [MaxLength(150)]
        public string AuthorName { get; set; }
        public int BookViews { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
    }
}
