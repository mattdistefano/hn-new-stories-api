using System.Collections.Immutable;
using LazyCache;
using Microsoft.Extensions.Options;

namespace HackerNews.WebAPI.Services.HackerNewsApiClient;

/// <summary>
///     Configuration for HackerNewsCachingApiClient.
/// </summary>
public class HackerNewsCachingApiClientConfiguration
{
    public int StoryIdCacheDuration { get; init; }

    public int StoryCacheDuration { get; init; }
}

/// <summary>
///     Cache-enabled IHackerNewsApiClient decorator.
/// </summary>
/// <param name="client">IHackerNewsApiClient instance to decorate.</param>
/// <param name="cache">ICache instance to use.</param>
/// <param name="options">Cache configuration options.</param>
public class HackerNewsCachingApiClient(
    IHackerNewsApiClient client,
    IAppCache cache,
    IOptions<HackerNewsCachingApiClientConfiguration> options
) : IHackerNewsApiClient
{
    public async Task<ImmutableList<int>?> GetNewStoryIds()
    {
        return await cache.GetOrAdd("HackerNewsCachingApiClient:storyIds", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.Value.StoryIdCacheDuration);
            return client.GetNewStoryIds();
        });
    }

    public async Task<HackerNewsItem?> GetItem(int id)
    {
        return await cache.GetOrAdd($"HackerNewsCachingApiClient:items:{id}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(options.Value.StoryCacheDuration);
            return client.GetItem(id);
        });
    }
}