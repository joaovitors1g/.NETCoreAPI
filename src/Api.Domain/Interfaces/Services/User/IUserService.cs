using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.DTOs.User;

namespace Api.Domain.Interfaces.Services.User
{
   public interface IUserService
   {
      Task<UserDTO> Get(Guid id);
      Task<IEnumerable<UserDTO>> GetAll();
      Task<UserDTOCreateResult> Post(UserDTO user);
      Task<UserDTOUpdateResult> Put(UserDTO user);
      Task<bool> Delete(Guid id);
   }
}
