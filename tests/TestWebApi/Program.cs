using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Neftm.TelegramMiniApp.Authorization;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);


// Logging
Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Logger(c =>
		c.WriteTo.File(Path.Combine("logs", "_.log"),
			rollingInterval: RollingInterval.Day,
			restrictedToMinimumLevel: LogEventLevel.Information))
	.CreateLogger();

builder.Services.AddSpaStaticFiles(x => x.RootPath = "wwwroot");
builder.Services.AddEndpointsApiExplorer();

//swagger
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Name = "Authorization"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddAuthorization(options =>
{
	options.AddDefaultTmaPolicy();
	options.AddTmaPremiumPolicy();
});

builder.Services.AddTelegramMiniAppInHeader(options =>
{
	options.BotToken = builder.Configuration.GetValue<string>("BotToken")!;
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseSpaStaticFiles();
app.UseSpa(c =>
{
	c.Options.DefaultPageStaticFileOptions = new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider(builder.Environment.WebRootPath),
	};
});
app.MapFallbackToFile("index.html");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();