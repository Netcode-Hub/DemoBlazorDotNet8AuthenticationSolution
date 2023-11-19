
using System.ComponentModel.DataAnnotations;
namespace SharedLibrary.Models
{
     public record RegisterUser()
    {
        [EmailAddress, Required]
        public string? Email { get; set; }
        [DataType(DataType.Password), Required]
        public string? Password { get; set; }
    }
}
