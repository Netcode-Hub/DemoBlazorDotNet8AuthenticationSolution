using SharedLibrary.Models;

namespace BlazorWebAssemblyApp.Services
{
    public interface IClientService
    {
        Task<WeatherForecast[]> GetWeatherForecasts();
    }
}
