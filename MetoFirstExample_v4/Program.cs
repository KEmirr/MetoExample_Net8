using MetoFirstExample_v4_WepAPI;
using MetoFirstExample_v4_WepAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(new DatabaseHelper(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new PLCHelper(
    builder.Configuration["PLC:IPAddress"],
    int.Parse(builder.Configuration["PLC:Port"]),
    int.Parse(builder.Configuration["PLC:WriteRegisterAddress"]),
    int.Parse(builder.Configuration["PLC:ReadRegisterAddress"])
));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

//app.UseHttpsRedirection();

app.UseRouting();

//app.UseAuthorization();

app.MapControllers();

app.Run();

