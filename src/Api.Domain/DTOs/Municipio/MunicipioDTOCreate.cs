using System;
using System.ComponentModel.DataAnnotations;
using Api.Domain.DTOs.UF;

namespace Api.Domain.DTOs.Municipio
{
    public class MunicipioDTOCreate
    {

        [Required(ErrorMessage = "Nome do município é obrigatório")]
        [StringLength(60, ErrorMessage = "Nome do Municipio deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "Código do IBGE inválido")]
        public int CodIBGE { get; set; }

        [Required(ErrorMessage = "Código da UF é obrigatório")]
        public Guid UFId { get; set; }

    }
}
