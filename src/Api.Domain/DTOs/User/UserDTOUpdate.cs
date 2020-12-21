using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.DTOs.User
{
    public class UserDTOUpdate
    {
        [Required(ErrorMessage = "O id é obrigatório")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]

        public string Name { get; set; }

        [Required(ErrorMessage = "O campo e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail inserido é inválido")]
        [StringLength(130, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
