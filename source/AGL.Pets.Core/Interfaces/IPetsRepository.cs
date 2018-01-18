using AGL.Pets.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Pets.Core.Interfaces
{
    public interface IPetsRepository
    {

        Task<IEnumerable<Owner>> GetAllOwnersAndPets();
    }
}
