using System.Text.Json;

namespace BlazorWebAssemblyApp.Authentication
{
    public static class SerializerOrDeserialize
    {
        public static AuthenticationModel Deserialize(string serializeString) => JsonSerializer.Deserialize<AuthenticationModel>(serializeString)!;
        public static string Serialize(AuthenticationModel model) => JsonSerializer.Serialize(model);
    }
}
