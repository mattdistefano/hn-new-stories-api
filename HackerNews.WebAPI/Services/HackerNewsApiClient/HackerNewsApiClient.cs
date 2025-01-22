using System.Collections.Immutable;

namespace HackerNews.WebAPI.Services.HackerNewsApiClient;

/// <summary>
///     Implementation of IHackerNewsApiClient integrating with Hacker News API via HTTP.
/// </summary>
/// <param name="client">HttpClient instance to use</param>
public class HackerNewsApiClient(HttpClient client) : IHackerNewsApiClient
{
    public async Task<HackerNewsItem?> GetItem(int id)
    {
        return await client.GetFromJsonAsync<HackerNewsItem>($"v0/item/{id}.json");
    }


    public async Task<ImmutableList<int>?> GetNewStoryIds()
    {
        return await client.GetFromJsonAsync<ImmutableList<int>>("v0/newstories.json");
    }
}