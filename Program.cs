using PremierLeague_Api.Data;
using PremierLeague_Api.Startup;
using static PremierLeague_Api.Startup.DependenciesConfig;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.ReisterServices();

        var app = builder.Build();
        AppDbContext.Initialize(app.Configuration);
        app.Lifetime.ApplicationStopped.Register(() =>
        {
            try
            {
                AppDbContext.Instance.Dispose();
            }
            catch { }
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseRouting();

        app.UseCorsPolicyServices();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}