using BlazorWebAssemblyApp.Authentication;
using SharedLibrary.Models;
using System.Net.Http.Json;

namespace BlazorWebAssemblyApp.Services
{
    public class ClientService : IClientService
    {
        private readonly ValidateHttpClient validateHttpClient;
        public ClientService(ValidateHttpClient validateHttpClient)
        {
            this.validateHttpClient = validateHttpClient;
        }

        public async Task<WeatherForecast[]> GetWeatherForecasts()
        {
            var result = await validateHttpClient.GetPrivateHttpClient();
            var response = await result.GetAsync("api/WeatherForecast");

            bool checkResponseIfUnAuthorized = CheckResponse(response);
            if (!checkResponseIfUnAuthorized)
                return await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

            if (!await RequestAndSetNewToken())
                return null!;

            return await GetWeatherForecasts();
        }


        private async Task<bool> RequestAndSetNewToken()
        {
           
            var getToken = await validateHttpClient.GetTokenFromLocalStorage();
            var result = await validateHttpClient.GetPublicHttpClient().PostAsJsonAsync("refresh", new PostToken() { RefreshToken = getToken.RefreshToken });
            var response = await result.Content.ReadFromJsonAsync<LoginResponse>();
            if (response is null) return false;
            await validateHttpClient.SetTokenToLocalStorage(new AuthenticationModel() { RefreshToken = response.RefreshToken, Token = response.AccessToken, Username =getToken.Username });
            return true;
        }


        private bool CheckResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return true;
            else
                return false;
        }
    }
}
