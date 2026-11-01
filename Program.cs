using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container including a logger.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpLogging(o => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Running in development environment.");
    app.MapOpenApi();
}

app.UseHttpLogging();
app.UseHttpsRedirection();

IResult returnFileIfItExists(string path, string mimeType) {
  
  // Throw and catch file not found errors.
  // TODO Handle permissions exception?
  try {
    if (!File.Exists(path)) {
      throw new FileNotFoundException();
    } else {
      return Results.File(path, contentType: mimeType);
    }
  }
  catch (FileNotFoundException ex) {
    return Results.BadRequest(ex.Message);
  }
}

// Handle reference letter GET requests.
app.MapGet("{id}/referenceletter", (int id) => {
  string filePath = $"{Directory.GetCurrentDirectory()}/files/patients/{id}/ref.txt";
  var mimeType = "text/plain";

  return returnFileIfItExists(filePath, mimeType);
})
.WithName("GetReferenceLetter");

// Handle allergies information GET requests.
app.MapGet("{id}/allergies", (int id) => {
  string filePath = $"{Directory.GetCurrentDirectory()}/files/patients/{id}/allergies.txt";
  var mimeType = "text/plain";

  return returnFileIfItExists(filePath, mimeType);
})
.WithName("GetAllergies");

// Handle medication usage GET requests.
app.MapGet("{id}/meds", (int id) => {
  string filePath = $"{Directory.GetCurrentDirectory()}/files/patients/{id}/meds.txt";
  var mimeType = "text/plain";

  return returnFileIfItExists(filePath, mimeType);
})
.WithName("GetMedicationReport");

app.Run();