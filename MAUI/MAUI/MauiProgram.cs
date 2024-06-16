using Microsoft.Extensions.Logging;

namespace MAUI
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
                builder.Services.AddSingleton<IDeck,Deck();
            builder.Services.AddSingleton<IPlayer, PlayerImplementation>();
            builder.Services.AddSingleton<IDealer, DealerImplementation>();

            return builder.Build();
        }
    }
}
