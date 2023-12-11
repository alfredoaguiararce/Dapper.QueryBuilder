using DapperQuery.Builder.Builders.Query;

var builder = WebApplication.CreateBuilder(args);

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/callback",async  () =>
{
    Query MockQuery = new QueryBuilder()
                  .WithSqlConnection("Data Source=TUA-DBERP;Persist Security Info=True;Password=wf123;User ID=wf;Initial Catalog=OPSC_DVTEMP")
                  .WithCommanTypeAsSimpleQuery()
                  .WithSQLCommand("SELECT 5")
                  .WithCallback(results =>
                  {
                      Query MockQuery = new QueryBuilder()
                      .WithSqlConnection("Data Source=TUA-DBERP;Persist Security Info=True;Password=wf123;User ID=wf;Initial Catalog=OPSC_DVTEMP")
                      .WithCommanTypeAsSimpleQuery()
                      .WithSQLCommand("SELECT 6")
                      .WithCallback(results =>
                      {
                          foreach (var row in results.GetListResult<int>().Result)
                          {
                              // Procesar cada fila si es necesario
                              Console.WriteLine($"Column1: {row}");
                          }
                          return "Extra info";
                      })
                      .Build();

                      MockQuery.GetListResult<int>();
                      // Acceder y utilizar los resultados de la primera consulta
                      foreach (var row in results.GetListResult<int>().Result)
                      {
                          // Procesar cada fila si es necesario
                          Console.WriteLine($"Column1: {row}");
                      }

                      // Puedes devolver algún resultado si es necesario
                      return "Alguna información adicional";
                  })
                  .Build();

   await MockQuery.Run();
})
.WithName("TestCallback");


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}