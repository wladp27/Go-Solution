using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GoWebBlazor;
using GoWebBlazor.Service;
using GoWeb.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using GoWeb.Shared.Service;
using GoWeb.Shared.Features.Auth.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();


builder.RootComponents.Add<GoWeb.Shared.Features.Navigation.AppRoute>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBootstrapBlazor();
builder.Services.AddScoped<AuthenticationStateProviderCustom>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<AuthenticationStateProviderCustom>());
builder.Services.AddScoped<AuthorizationMessageHandler>();
builder.Services.AddHttpClient("TokenAPIClient", client => client.BaseAddress = new Uri("http://201.34.131.23:8080")).AddHttpMessageHandler<AuthorizationMessageHandler>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://201.34.131.23:8080") });
builder.Services.AddMediatR(cfg =>
{
    MediatRServiceConfiguration mediatRServiceConfiguration = cfg.RegisterServicesFromAssembly(typeof(GoWeb.Shared.Service.CityService).Assembly);
});
builder.Services.AddScoped<CityService>();


builder.Services.AddScoped<ITokenService, TokenService>();



await builder.Build().RunAsync();
