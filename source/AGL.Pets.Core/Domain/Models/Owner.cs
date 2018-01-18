using System;
using System.Collections.Generic;
using System.Text;

namespace AGL.Pets.Core.Domain.Models
{
    public class Owner
    {
        public Owner()
        {
            Pets = new List<Pet>();
        }
        public string Name { get; set; }
        public string Gender { get; set; }
        public IList<Pet> Pets { get; set; }
    }

}
