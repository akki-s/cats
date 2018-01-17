using AGL.Pets.Core.Domain.Models;
using AGL.Pets.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AGL.Pets.Service.Repositories
{
    public class PetsRepository : IPetsRepository
    {
        //Microsoft patterns and practices suggest not to use using block with HttpClient as it implements IDisposable indirectly.
        //Instead Microsoft suggests to use this anti pattern to share single instance of HttpClient.
        private static HttpClient _httpClient;

        public PetsRepository()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Owner>> GetAllOwnersAndPets()
        {
            var response = await _httpClient.GetAsync("http://agl-developer-test.azurewebsites.net/people.json")
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<Owner>>(data);
            }
            else
                return null;

        }
    }
}
