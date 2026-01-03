using System.ComponentModel.DataAnnotations;

namespace ChineseAuction.Api.Dtos
{
    public class CategoryDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
