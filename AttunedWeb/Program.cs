using System.Text.Json.Serialization;
using AttunedWebApi.Converters;
using AttunedWebApi.Dtos.Playlists;
using AttunedWebApi.Dtos.Tracks;
using AttunedWebApi.Repositories;
using iTunesSmartParser.Xml;
using Microsoft.OpenApi.Models;

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
            options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter());
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "number"
            });
        });

        var xmlPath = builder.Configuration["LibraryXml"];

        if (xmlPath == null)
        {
            throw new Exception("LibraryXml prop in appsettings.json needs to be set to the library xml location.");
        }

        builder.Services.AddSingleton<ITrackListParser, TrackListParser>();
        builder.Services.AddSingleton<IPlaylistParser, PlaylistParser>();
        builder.Services.AddSingleton<IXmlSource>(_ => new XmlSource(xmlPath));

        builder.Services.AddSingleton<IRepository<TrackDto, TrackDetailsDto>>(provider =>
            new CachingRepository<TrackDto, TrackDetailsDto>(
                new TrackRepository(provider.GetRequiredService<IXmlSource>(),
                    provider.GetRequiredService<ITrackListParser>()),
                new TimeSpan(0, 1, 0)
            )
        );
        builder.Services.AddSingleton<IRepository<PlaylistDto, PlaylistDetailsDto>>(provider =>
            new CachingRepository<PlaylistDto, PlaylistDetailsDto>(
                new PlaylistRepository(provider.GetRequiredService<IXmlSource>(), provider.GetRequiredService<IPlaylistParser>()),
                new TimeSpan(0, 1, 0)
            )
        );

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