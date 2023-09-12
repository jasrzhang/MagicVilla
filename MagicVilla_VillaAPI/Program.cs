
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection")));

//Add interface to service
builder.Services.AddScoped<IVillaRepository,VillaRepository>();

//add service for auto mapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Customer SeriLog(Third party) top log information into files by add NugetPackage SeriLog.Sink.File
//Create SeriLog by using Debug(can be verbose,warning, etc), write to assigned folder and file name, also specify interval to create a new file. Finally create Logger

//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day).CreateLogger();

// Specify using Serilog from builder, this will tell the ILogger which logging service is used without changing code in Controller

//builder.Host.UseSerilog();

// add customised ILogger service, ie.e specify what is the implementation for ILogging service. AddSignlton, AddScoped, AddTransient spicfied different lifespan of the service
//builder.Services.AddSingleton<ILogging, Logging>();


// Add NewtonsoftJason for HttpPatch, otherwise couldn't parse Json Object
// options specify to accept Json format only, however can add XML formatting
builder.Services.AddControllers(option => { 
    //option.ReturnHttpNotAcceptable = true; 
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

var app = builder.Build();

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
