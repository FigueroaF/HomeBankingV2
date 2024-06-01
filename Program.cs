using HomeBankingV1.Models;
using HomeBankingV1.Repositories;
using HomeBankingV1.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//add context to the container 
builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection")));

//add repositories to the container
builder.Services.AddScoped<IClientRepository, ClientRepository>();


var app = builder.Build();

//crear un Scope para tomar el Context y asi Initialize la base de datos 
using (var scope = app.Services.CreateScope())
{
    try
    { 
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<HomeBankingContext>();
    DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
