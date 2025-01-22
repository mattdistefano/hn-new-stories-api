using System.Collections.Immutable;

namespace HackerNews.WebAPI.Services.HackerNewsApiClient;

public interface IHackerNewsApiClient
{
    /// <summary>
    ///     Gets a single item from the Hacker News API.
    /// </summary>
    /// <param name="id">Id of the item to retrieve.</param>
    /// <returns></returns>
    Task<HackerNewsItem?> GetItem(int id);

    /// <summary>
    ///     Gets list of the newest 500 stories from the Hacker News API.
    /// </summary>
    /// <returns></returns>
    Task<ImmutableList<int>?> GetNewStoryIds();
}