using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.DTOs.CEP
{
    public class CEPDTOCreate
    {
       [Required(ErrorMessage = "CEP é campo obrigatório")] 
       public string CEP { get; set; }


        [Required(ErrorMessage = "Logradouro é campo obrigatório")]
       public string Logradouro { get; set; }

       public string Numero { get; set; }

        [Required(ErrorMessage = "Municipio é campo obrigatório")]
        public Guid MunicipioId { get; set; }
    }
}
