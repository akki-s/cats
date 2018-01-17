using AGL.Pets.Core.Domain.ViewModels;
using AGL.Pets.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AGL.Pets.Service
{
    public class PetsService : IPetsService
    {
        private readonly IPetsRepository _petsRepository;

        public PetsService(IPetsRepository petsRepository)
        {
            _petsRepository = petsRepository;
        }

        public async Task<List<GroupedByOwnerGenderViewModel>> GetCatsGroupedByOwnerGender()
        {
            var ownersAndPetsResult = await _petsRepository.GetAllOwnersAndPets();

            if (null == ownersAndPetsResult)
                return null;

            //first expand the items, and then group by owner gender
            var groupedResults = ownersAndPetsResult                
                .Where(a => a.Pets != null) //exclude owners with no pets
                .SelectMany(a => a.Pets.Select(b => new
                {
                    //ASSUMPTION: if owner gender, owner name or cat name are null or empty, they would be returned as empty string.
                    //This behaviour can be confirmed with business, but as of now taking this assumption.

                    OwnerName = string.IsNullOrEmpty(a.Name) ? string.Empty : a.Name,
                    OwnerGender = string.IsNullOrEmpty(a.Gender) ? string.Empty : a.Gender,
                    PetName = string.IsNullOrEmpty(b.Name) ? string.Empty : b.Name,
                    PetType = string.IsNullOrEmpty(b.Type) ? string.Empty : b.Type,
                }))
                                                .GroupBy(x => x.OwnerGender)
                                                .Select(g => new GroupedByOwnerGenderViewModel
                                                {
                                                    OwnerGender = g.Key,
                                                    Cats = g
                                                    .Where(o => o.PetType.Equals("cat", StringComparison.OrdinalIgnoreCase))
                                                    .Select(o => new PetDetailsViewModel
                                                    {
                                                        Name = o.PetName,
                                                        OwnerName = o.OwnerName,
                                                    })
                                                    .OrderBy(c => c.Name).ThenBy(c => c.OwnerName) // cats are first sorted by their name, and then by their owner name
                                                    .ToList()
                                                }).ToList();



           
            var results = groupedResults.Where(g => g.Cats.Count > 0) //if owner has pets other than cats, this should be 0
                .OrderBy(g => g.OwnerGender); //order by owner gender

            //ASSUMPTION: if there are no pets for a gender, that gender wont be returned from our API.             
            //This can be confirmed with business if requirement is to show an empty section for that gender.
            return results.ToList();

        }

    }
}
