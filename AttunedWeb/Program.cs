using System.Text.Json.Serialization;

namespace AttunedWebApi;

internal static class Program
{
    private static string _corsPolicy = "corsPolicy";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(_corsPolicy, policy =>
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(_corsPolicy);

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}