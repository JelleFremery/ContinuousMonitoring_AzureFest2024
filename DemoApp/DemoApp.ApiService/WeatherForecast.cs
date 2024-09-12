
// Add service defaults & Aspire components.

// Add services to the container.


// Configure the HTTP request pipeline.





record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
