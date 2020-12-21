using System;
using Api.Domain.DTOs.UF;

namespace Api.Domain.DTOs.Municipio
{
    public class MunicipioDTOCompleto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public int CodIBGE { get; set; }

        public Guid UFId { get; set; }

        public UFDTO UF { get; set; }
    }
}
