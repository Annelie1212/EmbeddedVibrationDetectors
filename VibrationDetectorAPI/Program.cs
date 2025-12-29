var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddGrpcClient<GrpcShared.VDStatusHandler.VDStatusHandlerClient>(o =>
{
    o.Address = new Uri("http://localhost:5001");
}).ConfigureChannel(options =>
{
    options.HttpHandler = new HttpClientHandler();
});

builder.Services.AddOpenApi();

builder.Services.AddCors(x =>
{
    x.AddPolicy("AllowAll", x =>
    {
        x.AllowAnyOrigin();
        x.AllowAnyHeader();
        x.AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Console.WriteLine("--------------------TEST Vibration Detector API is running... AAAAAAAAAAAAAAA");



//var testClient = new VibrationDetectorAPI.TestClientGrpc();
//testClient.Testserver();

app.Run();


