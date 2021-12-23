using GlobalBlue.Assignment;

var builder = WebApplication.CreateBuilder(args);

// Configure the Purchase API services
builder.Services.AddPurchaseApi();

// Add services to the container.
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

// Add the HTTP endpoints
app.MapPurchaseApiRoutes();

app.Run();

