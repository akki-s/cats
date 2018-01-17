using AGL.Pets.Core.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using AGL.Pets.Core.Domain.Models;
using Newtonsoft.Json;

namespace AGL.Pets.Service.Tests
{
    public class GroupedCatResultsTests
    {
        [Fact]
        public async Task GetListOfCats_When_Pets_Data_Is_Null_Should_Return_Null()
        {
            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(() => null);

            var catService = new PetsService(mockRepository.Object);
            var actualResult = await catService.GetCatsGroupedByOwnerGender();
            actualResult.Should().BeNull();
        }

        [Fact]
        public async Task Get_Grouped_Results_Returns_Data_GroupedByOwnerGender_Sorted_By_Name()
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

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(2);
            actualResult[0].OwnerGender.Should().Be("Female");
            actualResult[1].OwnerGender.Should().Be("Male");

            //cats are sorted by their name
            actualResult[0].Cats[0].Name.Should().Be("Abe");
            actualResult[0].Cats[1].Name.Should().Be("Garfield");

            actualResult[1].Cats[0].Name.Should().Be("Garry");
            actualResult[1].Cats[1].Name.Should().Be("Jasper");
        }

        [Fact]
        public async Task Get_Grouped_Results_Should_Return_Only_Cats()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Name = "Selina",
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                },
                new Owner {
                    Name = "Bruce",
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Jasper" },
                        new Pet { Type = "Dog", Name = "Scooby" },
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(2);
            actualResult[0].OwnerGender.Should().Be("Female");
            actualResult[1].OwnerGender.Should().Be("Male");

            //only cats should be there
            actualResult[0].Cats.Any(c => !c.PetType.Equals("cat", StringComparison.OrdinalIgnoreCase)).Should().BeFalse();
            actualResult[1].Cats.Any(c => !c.PetType.Equals("cat", StringComparison.OrdinalIgnoreCase)).Should().BeFalse();

        }

        [Fact]
        public async Task Get_Grouped_Results_Should_Not_Return_Gender_With_No_Cats()
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
                new Owner
                {
                    Name = "Bruce",
                    Gender = "Male",
                    Pets = new List<Pet>
                    {
                        new Pet {Type = "Dog", Name = "Scooby"}
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(1);
            actualResult[0].OwnerGender.Should().Be("Female");
        }

        [Fact]
        public async Task Get_Grouped_Results_Owner_Has_No_Pets()
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
                },
                new Owner {
                    Name = "Damian",
                    Gender = "Male",
                    Pets = null
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(1);
            actualResult[0].OwnerGender.Should().Be("Female");

            // cats are sorted by their name
            actualResult[0].Cats[0].Name.Should().Be("Abe");
            actualResult[0].Cats[1].Name.Should().Be("Garfield");
        }

        [Fact]
        public async Task Get_Grouped_Results_Two_Owners_Of_Same_Gender_Have_Pet_With_Same_Name_Should_Sort_By_Owner()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Name = "Damian",
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                },
                new Owner {
                    Name = "Bruce",
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(1);
            actualResult[0].OwnerGender.Should().Be("Male");

            // cats are sorted by their name
            actualResult[0].Cats[0].OwnerName.Should().Be("Bruce");
            actualResult[0].Cats[1].OwnerName.Should().Be("Damian");
        }

        [Fact]
        public async Task Get_Grouped_Results_If_Owner_Name_Blank_Should_Show_Empty()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();

            var pet = actualResult[0].Cats[0];
            pet.OwnerName.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Get_Grouped_Results_If_Pet_Name_BlankOrNull_Should_Return_Blank()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Name = "Selina",
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat" },
                        new Pet { Type = "Cat", Name = string.Empty },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult[0].Cats[0].Name.Should().Be(string.Empty);
            actualResult[0].Cats[1].Name.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Get_Grouped_Results_If_Pet_Type_Null_Should_Not_Include_It()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Name = "Selina",
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Name = "Abe" },
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult[0].Cats.Count.Should().Be(1);
            actualResult[0].Cats[0].Name.Should().Be("Garfield");
        }

        [Fact]
        public async Task Get_Grouped_Results_If_Owner_Name_Blank_Or_Null_Should_Return_As_Empty()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Abe" },
                    }
                },
                new Owner {
                    Name = string.Empty,
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(2);
            actualResult[0].Cats[0].OwnerName.Should().Be(string.Empty);
            actualResult[1].Cats[0].OwnerName.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Get_Grouped_Results_If_Cats_With_Owner_Gender_Empty_Or_Null_Should_be_Grouped_Under_Empty_Owner()
        {
            var mockDataForGrouping = new List<Owner>
            {
                new Owner {
                    Gender = "Female",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Abe" },
                    }
                },
                new Owner {
                    Name = "Robin",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                },
                new Owner {
                    Name = "Bruce",
                    Gender = "Male",
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Jasper" },
                        new Pet { Type = "Cat", Name = "Garfield" },
                    }
                },

                new Owner {
                    Name = "Alfred",
                    Gender = string.Empty,
                    Pets = new List<Pet> {
                        new Pet { Type = "Cat", Name = "Garry" },
                    }
                }
            };

            var mockRepository = new Mock<IPetsRepository>();
            mockRepository.Setup(x => x.GetAllOwnersAndPets())
                .ReturnsAsync(mockDataForGrouping);

            var catService = new PetsService(mockRepository.Object);

            var actualResult = await catService.GetCatsGroupedByOwnerGender();

            actualResult.Should().NotBeNull();
            actualResult.Count.Should().Be(3);
            actualResult[0].OwnerGender.Should().Be(string.Empty);
            actualResult[0].Cats[0].OwnerName.Should().Be("Robin");
            actualResult[0].Cats[1].OwnerName.Should().Be("Alfred");
            actualResult[0].Cats[0].Name.Should().Be("Garfield");
            actualResult[0].Cats[1].Name.Should().Be("Garry");
        }
    }
}
