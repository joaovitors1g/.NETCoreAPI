using System.ComponentModel.DataAnnotations;

namespace Api.Domain.DTOs
{
   public class LoginDTO
   {
      [Required(ErrorMessage = "O email é obrigatório!")]
      [EmailAddress(ErrorMessage = "O email digitado é inválido!")]
      public string Email { get; set; }
   }
}
