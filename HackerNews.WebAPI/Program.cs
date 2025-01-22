using HackerNews.WebAPI.Services.HackerNewsApiClient;
using HackerNews.WebAPI.Services.HackerNewsService;
using LazyCache;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddLazyCache();

// Configure HackerNewsApiClient's HttpClient
builder.Services.AddHttpClient<HackerNewsApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("HackerNewsApiBasePath")!);
});

// Configure HackerNewsCachingApiClient caching
builder.Services.Configure<HackerNewsCachingApiClientConfiguration>(
    builder.Configuration.GetSection("HackerNewsCachingConfiguration"));

// Use HackerNewsCachingApiClient-decorated HackerNewsApiClient
builder.Services.AddTransient<IHackerNewsApiClient, HackerNewsCachingApiClient>(provider =>
    new HackerNewsCachingApiClient(
        provider.GetRequiredService<HackerNewsApiClient>(),
        provider.GetRequiredService<IAppCache>(),
        provider.GetRequiredService<IOptions<HackerNewsCachingApiClientConfiguration>>()
    )
);

builder.Services.AddTransient<IHackerNewsNewStoriesService, InMemoryHackerNewsNewStoriesService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi().CacheOutput();

app.UseCors("AllowAnyOrigin");

// for development purposes, no HTTPS is set up

app.MapGet("/newstories",
        (IHackerNewsNewStoriesService service,
                int pageNumber = 1,
                int pageSize = 20,
                string? search = null) =>
            service.GetNewStories(pageNumber, pageSize, search))
    .WithName("GetNewStories");

app.Run();