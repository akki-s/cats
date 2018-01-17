using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AGL.Pets.Service.Tests.Repositories.Fakes
{
    /// <summary>
    /// derives from HttpMessageHandler and overrides SendAsync method of HttpMessageHandler for unit tests
    /// </summary>

    public class MockHttpHandler : HttpMessageHandler

    {
        private string _stringContent;
        private HttpStatusCode _statusCode;

        public MockHttpHandler(string stringContent = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _stringContent = stringContent;
            _statusCode = statusCode;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(_statusCode);

            if (_statusCode == HttpStatusCode.OK)
                responseMessage.Content = new StringContent(_stringContent);
            else
                responseMessage.ReasonPhrase = "Unit test With status code not OK";

            return await Task.FromResult(responseMessage);
        }


    }
}
