using BrewIoT.Client.Devices;
using BrewIoT.Client.Recipes;
using Microsoft.Extensions.Logging;

namespace BrewIoT.Client;

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
		
		const string apiUrl = "https://localhost:7207";

		builder.Services.AddDevices(apiUrl);
		builder.Services.AddRecipes(apiUrl);

		return builder.Build();
	}
}
