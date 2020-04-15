using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using YASDM.Client.Services;
using System.Net.Http;

namespace YASDM.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddOptions();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddSingleton<State>();

            var host = builder.Build();
            var httpClient = host.Services.GetRequiredService<HttpClient>();
            // httpClient.BaseAddress = new Uri("http://localhost:5002");

            await host.RunAsync();
        }
    }
}
