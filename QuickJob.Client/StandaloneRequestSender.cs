using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuickJob.Client.Models;

namespace QuickJob.Client;

internal class StandaloneRequestSender : IRequestSender
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = {new JsonStringEnumConverter()},
    };
    
    private readonly HttpClient standaloneClient;
    private readonly Newtonsoft.Json.JsonSerializer serializer;

    public StandaloneRequestSender(HttpClient standaloneClient)
    {
        this.standaloneClient = standaloneClient;
        serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
        
    }

    public async Task<ApiResult<TResponse>> SendRequestAsync<TResponse>(string httpMethod, string uri, object requestEntity = null) where TResponse : class
    {
        var response = await SendAsync(httpMethod, uri, requestEntity);
    
        if (response.IsSuccessStatusCode)
            return ApiResult<TResponse>.CreateSuccessful((int)response.StatusCode, await Deserialize<TResponse>(response));

        var errorResponse = await Deserialize<ErrorResult>(response);
        return ApiResult<TResponse>.CreateError(errorResponse);
    }

    public async Task<ApiResult> SendRequestAsync(string httpMethod, string uri, object requestEntity = null)
    {
        var response = await SendAsync(httpMethod, uri, requestEntity);
    
        if (response.IsSuccessStatusCode)
            return ApiResult.CreateSuccessful((int)response.StatusCode);
    
        var errorResponse = await Deserialize<ErrorResult>(response);
        return ApiResult.CreateError(errorResponse);
    }
    
    private Task<HttpResponseMessage> SendAsync(string httpMethod, string uri, object requestEntity = null)
    {
        var request = new HttpRequestMessage(new HttpMethod(httpMethod), uri)
        {
            Content = new StringContent(JsonConvert.SerializeObject(requestEntity), Encoding.UTF8, "application/json")
        };

        return standaloneClient.SendAsync(request, HttpCompletionOption.ResponseContentRead);
    }
    
    private async Task<TResponse> Deserialize<TResponse>(HttpResponseMessage responseMessage)
    {
        var responseString = await responseMessage.Content.ReadAsStringAsync();
        return serializer.Deserialize<TResponse>(new JsonTextReader(new StringReader(responseString)));
    }
}
