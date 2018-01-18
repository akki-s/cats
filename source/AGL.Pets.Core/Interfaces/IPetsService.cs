using AGL.Pets.Core.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Pets.Core.Interfaces
{
    public interface IPetsService
    {
        Task<List<GroupedByOwnerGenderViewModel>> GetCatsGroupedByOwnerGender();
    }
}
