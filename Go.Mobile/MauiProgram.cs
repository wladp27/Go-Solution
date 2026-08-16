using Go.Mobile.Services;
using GoWeb.Shared.Features.Auth.Handlers;
using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace Go.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddAuthorizationCore();
            builder.Services.AddBootstrapBlazor();
            builder.Services.AddScoped<AuthenticationStateProviderCustom>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<AuthenticationStateProviderCustom>());
            builder.Services.AddScoped<AuthorizationMessageHandler>();
            builder.Services.AddHttpClient("TokenAPIClient", client => client.BaseAddress = new Uri("https://localhost:7065")).AddHttpMessageHandler<AuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7065") });
            builder.Services.AddMediatR(cfg =>
            {
                MediatRServiceConfiguration mediatRServiceConfiguration = cfg.RegisterServicesFromAssembly(typeof(GoWeb.Shared.Service.CityService).Assembly);
            });
            builder.Services.AddScoped<CityService>();


            builder.Services.AddScoped<ITokenService, MauiTokenService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
