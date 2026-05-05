using KendaWeb.Api.Configuration;
using KendaWeb.Api.Repositories;
using KendaWeb.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MachineConfig>(
    builder.Configuration.GetSection("MachineConfig"));

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<IMachineRouter, MachineRouter>();

builder.Services.AddScoped<ISanLuongRepository, SanLuongRepository>();
builder.Services.AddScoped<INguyenLieuRepository, NguyenLieuRepository>();
builder.Services.AddScoped<IInTemRepository, InTemRepository>();
builder.Services.AddScoped<IDoNguocRepository, DoNguocRepository>();
builder.Services.AddScoped<IHoaChatRepository, HoaChatRepository>();

builder.Services.AddScoped<ISanLuongService, SanLuongService>();
builder.Services.AddScoped<INguyenLieuService, NguyenLieuService>();
builder.Services.AddScoped<IInTemService, InTemService>();
builder.Services.AddScoped<IDoNguocService, DoNguocService>();
builder.Services.AddScoped<IHoaChatService, HoaChatService>();
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
