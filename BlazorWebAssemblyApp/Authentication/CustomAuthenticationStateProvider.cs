using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
namespace BlazorWebAssemblyApp.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService) { this.localStorageService = localStorageService; }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var authenticationModel = await localStorageService.GetItemAsStringAsync("Authentication");
                if (authenticationModel == null) { return await Task.FromResult(new AuthenticationState(anonymous)); }
                return await Task.FromResult(new AuthenticationState(SetClaims(SerializerOrDeserialize.Deserialize(authenticationModel).Username!)));
            }
            catch { return await Task.FromResult(new AuthenticationState(anonymous)); }
        }


        public async Task UpdateAuthenticationState(AuthenticationModel authenticationModel)
        {
            try
            {
                ClaimsPrincipal claimsPrincipal = new();
                if (authenticationModel is not null)
                {
                    claimsPrincipal = SetClaims(authenticationModel.Username!);
                    await localStorageService.SetItemAsStringAsync("Authentication", SerializerOrDeserialize.Serialize(authenticationModel));
                }
                else
                {
                    await localStorageService.RemoveItemAsync("Authentication");
                    claimsPrincipal = anonymous;
                }
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
            }
            catch {  await Task.FromResult(new AuthenticationState(anonymous)); }
        }


        private ClaimsPrincipal SetClaims(string email) => new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, email)
        }, "CustomAuth"));
    }
}
