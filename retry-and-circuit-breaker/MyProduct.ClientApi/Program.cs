using MyProduct.ClientApi.Abstractions;
using MyProduct.ClientApi.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Download", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
    })
    .AddPolicyHandler(GetCircuitBreakerPolicy())
    .AddPolicyHandler(GetRetryPolicy());

builder.Services.AddTransient<IDownloadService, DownloadService>();


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();


static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(2));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(11));
}