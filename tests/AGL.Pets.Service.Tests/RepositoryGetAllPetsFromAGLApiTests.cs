using AGL.Pets.Service.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net.Http;
using AGL.Pets.Service.Tests.Repositories.Fakes;
using AGL.Pets.Core.Domain.Models;
using Newtonsoft.Json;
using System.Net;

namespace AGL.Pets.Service.Tests.Repositories
{
    public class RepositoryGetAllPetsFromAGLApiTests
    {
        [Fact]
        public async Task ResultsReturnedWhenApiIsUp()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Name = "Selina",
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                        new Pet { Type = "Cat", Name = "Abe" }
                    }
                },
                new Owner {
                    Name = "Bruce",
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Jasper" },
                        new Pet { Type = "Dog", Name = "Scooby" },
                        new Pet { Type = "Cat", Name = "Garry" }
                    }
                }
            };

            var mockJson = JsonConvert.SerializeObject(mockDataForGrouping);
            var repo = new PetsRepository(new HttpClient(new MockHttpHandler(mockJson)));
            var results = await repo.GetAllOwnersAndPets();
            results.Should().NotBeNull();
        }

        [Fact]
        public async Task NullReturnedWhenApiResponseIsOkAndDataIsEmpty()
        {
            
            var repo = new PetsRepository(new HttpClient(new MockHttpHandler(string.Empty, HttpStatusCode.OK)));
            var results = await repo.GetAllOwnersAndPets();
            results.Should().BeNull();
        }

        [Fact]
        public async Task NullReturnedWhenApiResponseIsOkAndDataIsArbitrary()
        {
            var repo = new PetsRepository(new HttpClient(new MockHttpHandler("null", HttpStatusCode.OK)));
            var results = await repo.GetAllOwnersAndPets();
            results.Should().BeNull();
        }

        [Fact]
        public async Task NullReturnedWhenApiIsDown()
        {
            var repo = new PetsRepository(new HttpClient(new MockHttpHandler(string.Empty, HttpStatusCode.BadRequest)));
            var results = await repo.GetAllOwnersAndPets();
            results.Should().BeNull();
        }
    }

}
