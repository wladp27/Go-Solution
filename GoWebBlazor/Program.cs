using GoWeb.Shared.Features.Auth.Handlers;
using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Security;
using GoWeb.Shared.Service;
using GoWebBlazor;
using GoWebBlazor.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://201.34.131.23:8080") }); //http://localhost:5186/http://201.34.131.23:8080
builder.Services.AddMediatR(cfg =>
{
    MediatRServiceConfiguration mediatRServiceConfiguration = cfg.RegisterServicesFromAssembly(typeof(GoWeb.Shared.Service.CityService).Assembly);
});
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthorizationHandler, CheckAdminHandler>();
builder.Services.AddScoped<IAuthorizationHandler, CheckOrganizerHandler>();


await builder.Build().RunAsync();
