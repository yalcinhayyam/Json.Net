using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IService, Service>();

var app = builder.Build();

app.MapGet("/text", async (context) =>
{

    var streamReader = new StreamReader(context.Request.Body);
    var text = await streamReader.ReadToEndAsync();

    System.Console.WriteLine(text);
    Results.Ok();
});

app.MapGet("/json", async (context) =>
{
    var result = await context.Request.ReadFromJsonAsync<IDictionary<string, object>>(new JsonSerializerOptions()
    {
        WriteIndented = true
    });
    System.Console.WriteLine(result?["hello"]);
    Results.Ok();
});

app.MapGet("/foo", string (HttpContext context) =>
{
    return "FooBaz";
});

app.MapGet("/service", string (IService service) => service.Call());

app.Run();


public interface IService
{
    string Call();
}


public class Service : IService
{
    public string Call()
    {
        return "Service.Call";
    }
}