using AGL.Pets.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;


namespace AGL.Pets.Service.Tests.Repositories
{
    public class GetAllPetsFromAGLApiTests
    {
        [Fact]
        public async Task ResultsReturnedWhenApiIsUp()
        {
            var repo = new PetsRepository();
            var results = await repo.GetAllOwnersAndPets();
            results.Should().NotBeNull();
        }

        [Fact]
        public async Task NullReturnedWhenApiIsDown()
        {
            var repo = new PetsRepository();
            var results = await repo.GetAllOwnersAndPets();
            results.Should().BeNull();
        }
    }

}
