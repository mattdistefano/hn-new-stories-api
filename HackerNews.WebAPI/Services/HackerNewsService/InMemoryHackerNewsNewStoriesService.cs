using System.Collections.Immutable;
using HackerNews.WebAPI.Services.HackerNewsApiClient;

namespace HackerNews.WebAPI.Services.HackerNewsService;

/// <summary>
///     Implementation of IHackerNewsNewStoriesService performing search functions in memory.
/// </summary>
/// <param name="client">The IHackerNewsApiClient to use to retrieve data.</param>
public class InMemoryHackerNewsNewStoriesService(
    IHackerNewsApiClient client
) : IHackerNewsNewStoriesService
{
    public async Task<PaginatedApiResponseWrapper<HackerNewsNewStory>> GetNewStories(int pageNumber, int pageSize,
        string? search)
    {
        var newStoryIds = await client.GetNewStoryIds();

        if (newStoryIds is null || newStoryIds.Count == 0) return GetEmptyResponse(pageNumber, pageSize);

        if (string.IsNullOrWhiteSpace(search)) return await GetPageOfItems(newStoryIds, pageNumber, pageSize);

        return await GetPageOfSearchedItems(newStoryIds, pageNumber, pageSize, search);
    }

    private static PaginatedApiResponseWrapper<HackerNewsNewStory> GetEmptyResponse(int pageNumber, int pageSize)
    {
        var meta = new PaginatedApiResponseMeta(pageNumber, pageSize, 0);
        var data = ImmutableList<HackerNewsNewStory>.Empty;
        return new PaginatedApiResponseWrapper<HackerNewsNewStory>(data, meta);
    }

    private async Task<PaginatedApiResponseWrapper<HackerNewsNewStory>> GetPageOfSearchedItems(
        ImmutableList<int> allItemIds,
        int pageNumber, int pageSize, string search)
    {
        var allItems = await GetItems(allItemIds);

        // TODO title can supposedly contain HTML; we may want to exclude tags from the search
        var matchingItems = allItems
            .Where(item => item.Title.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) > -1)
            .ToList();

        var meta = new PaginatedApiResponseMeta(pageNumber, pageSize, matchingItems.Count);

        var data = matchingItems
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(item => new HackerNewsNewStory(item.Id, item.Title, item.Url))
            .ToImmutableList();

        return new PaginatedApiResponseWrapper<HackerNewsNewStory>(data, meta);
    }

    private async Task<PaginatedApiResponseWrapper<HackerNewsNewStory>> GetPageOfItems(ImmutableList<int> allItemIds,
        int pageNumber,
        int pageSize)
    {
        var itemIdPage = allItemIds
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        var items = await GetItems(itemIdPage);

        var meta = new PaginatedApiResponseMeta(pageNumber, pageSize, allItemIds.Count);

        var data = items
            .Select(item => new HackerNewsNewStory(item.Id, item.Title, item.Url))
            .ToImmutableList();

        return new PaginatedApiResponseWrapper<HackerNewsNewStory>(data, meta);
    }

    private async Task<List<HackerNewsItem>> GetItems(IEnumerable<int> itemIds)
    {
        var tasks = itemIds.Select(storyId => client.GetItem(storyId));

        var items = await Task.WhenAll(tasks);

        return items
            .Where(item => item is not null && item.Deleted != true)
            .Select(item => item!)
            .ToList();
    }
}