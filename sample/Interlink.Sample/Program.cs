using Interlink;
using Interlink.Sample.Data;
using Interlink.Sample.Logging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PetsDb"));

builder.Services.AddLogging();

//builder.Services.AddInterlink();
//builder.Services.AddInterlink(typeof(Program).Assembly);
//builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MyLoggingBehavior<,>));

builder.Services.AddInterlink(config =>
{
    config.AddBehavior(typeof(MyLoggingBehavior1<,>));
    config.AddBehavior(typeof(MyLoggingBehavior2<,>));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await SeedData.SeedAsync(context);
    }
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
