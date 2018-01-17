using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Pets.Core.Domain.ViewModels
{
    public class GroupedByOwnerGenderViewModel
    {
        public GroupedByOwnerGenderViewModel()
        {
            Cats = new List<PetDetailsViewModel>();
        }

        public string OwnerGender { get; set; }
        public List<PetDetailsViewModel> Cats { get; set; }
    }
}
