using System.Text.Json.Serialization;
using iTunesSmartParser.Xml;

namespace AttunedWebApi;

internal static class Program
{
    private const string CORS_POLICY = "corsPolicy";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CORS_POLICY, policy =>
            {
                //policy.AllowAnyOrigin();
                policy
                    .WithOrigins("http://localhost:5173", "https://localhost:5173", "http://127.0.0.1:5173",
                        "https://127.0.0.1:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        // Add services to the container.
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var xmlPath = builder.Configuration["LibraryXml"];

        if (xmlPath == null)
        {
            throw new Exception("LibraryXml prop in appsettings.json needs to be set to the library xml location.");
        }

        builder.Services.AddSingleton<ITrackListParser, TrackListParser>();
        builder.Services.AddSingleton<IPlaylistsParser, PlaylistParser>();
        builder.Services.AddSingleton<IXmlParser>(provider =>
            new XmlParserCacheProxy(new XmlParser(provider.GetService<ITrackListParser>()!,
                provider.GetService<IPlaylistsParser>()!, xmlPath)));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(CORS_POLICY);

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}