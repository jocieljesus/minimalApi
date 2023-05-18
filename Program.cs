using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); // unifica, app builder e router build

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Clients"));

var app = builder.Build();


if ( app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();

app.MapGet("/Clients", async(AppDbContext dbContext) => await dbContext.Clients.ToListAsync());

app.MapGet("/Client/{id}", async(int id, AppDbContext dbContext) => await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id));

app.MapPost("/Client", async (Client client, AppDbContext dbContext) =>
{
    dbContext.Clients.Add(client);
    await dbContext.SaveChangesAsync();
    return client;
});

app.MapPut("/Client/{id}", async (int id, Client client, AppDbContext dbContext) =>
{
    dbContext.Entry(client).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return client;
});

app.MapDelete("/Client/{id}", async( int id, AppDbContext dbContext) =>
{
    var client = await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == id);
    if( client != null)
    {
        dbContext.Clients.Remove(client);
        await dbContext.SaveChangesAsync();
    }
    return client;
});

app.UseSwaggerUI();

app.Run();

public class Client
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }

}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Client> Clients { get; set; }

 }