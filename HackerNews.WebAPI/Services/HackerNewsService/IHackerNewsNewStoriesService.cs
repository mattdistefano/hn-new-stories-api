using System.Collections.Immutable;

namespace HackerNews.WebAPI.Services.HackerNewsService;

/// <summary>
///     Minimal Hacker News Story.
/// </summary>
/// <param name="Id">Id of story.</param>
/// <param name="Title">Title of story.</param>
/// <param name="Url">Url of story, if any.</param>
public record HackerNewsNewStory(int Id, string Title, string? Url);

/// <summary>
///     Metadata for an API response using page-based pagination.
/// </summary>
/// <param name="PageNumber">The requested page number.</param>
/// <param name="PageSize">The requested page size.</param>
/// <param name="TotalCount">The total count of all items in all pages.</param>
public record PaginatedApiResponseMeta(int PageNumber, int PageSize, int TotalCount);

/// <summary>
///     Generic page-paginated API response wrapper.
/// </summary>
/// <param name="Data">The returned page of items.</param>
/// <param name="Meta">The pagination metadata.</param>
/// <typeparam name="TData">Type of Data</typeparam>
public record PaginatedApiResponseWrapper<TData>(ImmutableList<TData> Data, PaginatedApiResponseMeta Meta);

public interface IHackerNewsNewStoriesService
{
    /// <summary>
    ///     Gets the most recent stories with the specified pagination and search params.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="search">Search term, if any.</param>
    /// <returns></returns>
    public Task<PaginatedApiResponseWrapper<HackerNewsNewStory>> GetNewStories(int pageNumber, int pageSize,
        string? search);
}