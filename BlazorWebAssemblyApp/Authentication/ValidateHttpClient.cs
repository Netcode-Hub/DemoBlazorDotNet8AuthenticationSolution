using Blazored.LocalStorage;
namespace BlazorWebAssemblyApp.Authentication
{
    public class ValidateHttpClient
    {
        public string? AccessToken { get; set; }
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;

        public ValidateHttpClient(ILocalStorageService localStorageService, HttpClient HttpClient)
        {
            this.localStorageService = localStorageService;
            httpClient = HttpClient;
        }


        public HttpClient GetPublicHttpClient()
            
        {   // Check if the injected HttpClient has a section call Authorization, then remove cause the API is not protected.
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            return httpClient;
        }

        public async Task<HttpClient> GetPrivateHttpClient()
        {
                // Return, don't get from localstorage when AccessToken has the token, this iwll retian it memeory when any enpoint is called ones for the first time.
 
            var token = await GetTokenFromLocalStorage();
            if (!string.IsNullOrEmpty(AccessToken) && token.Token!.Equals(AccessToken)) return httpClient;

            if (token is null || token.Token is null) return httpClient;
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            AccessToken = token.Token;
            return httpClient;
        }


        public async Task<AuthenticationModel> GetTokenFromLocalStorage()
        {
            string tokenString = await localStorageService.GetItemAsStringAsync("Authentication");
            if (string.IsNullOrEmpty(tokenString)) return null!;
            return SerializerOrDeserialize.Deserialize(tokenString);
        }


        public async Task<bool> SetTokenToLocalStorage(AuthenticationModel tokenModel)
        {
            await localStorageService.SetItemAsStringAsync("Authentication", SerializerOrDeserialize.Serialize(tokenModel));
            AccessToken = string.Empty;
            await GetPrivateHttpClient();
            return true;
        }
    }
}