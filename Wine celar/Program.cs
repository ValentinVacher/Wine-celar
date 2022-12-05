using Microsoft.EntityFrameworkCore;
using Wine_celar.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WineContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("WineCellarDbCS"));
});

var app = builder.Build();

//Forcer les migrations en attentes (�vite de faire le update-database)
//using (var serviceScope = app.Services.CreateScope())
//{
//    var services = serviceScope.ServiceProvider;
//    var wikyContext = services.GetRequiredService<WineContext>();
//    //wikyContext.Database.Migrate();

//    wikyContext.Database.EnsureDeleted();
//    wikyContext.Database.EnsureCreated();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
