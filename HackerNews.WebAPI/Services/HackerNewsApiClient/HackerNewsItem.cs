namespace HackerNews.WebAPI.Services.HackerNewsApiClient;

/// <summary>
///     Represents an item retrieved from the HackerNewsAPI.
/// </summary>
/// <param name="Id">The item's unique id.</param>
/// <param name="Deleted">true if the item is deleted.</param>
/// <param name="Type">The type of item. One of "job", "story", "comment", "poll", or "pollopt".</param>
/// <param name="By">The username of the item's author.</param>
/// <param name="Time">Creation date of the item, in Unix Time..</param>
/// <param name="Text">The comment, story or poll text. HTML.</param>
/// <param name="Dead">true if the item is dead.</param>
/// <param name="Parent">The comment's parent: either another comment or the relevant story.</param>
/// <param name="Poll">The pollopt's associated poll.</param>
/// <param name="Kids">The ids of the item's comments, in ranked display order.</param>
/// <param name="Url">The URL of the story.</param>
/// <param name="Score">The story's score, or the votes for a pollopt.</param>
/// <param name="Title">The title of the story, poll or job. HTML.</param>
/// <param name="Parts">A list of related pollopts, in display order.</param>
/// <param name="Descendants">In the case of stories or polls, the total comment count.</param>
public record HackerNewsItem(
    int Id,
    bool? Deleted,
    string Type,
    string By,
    int Time,
    string Text,
    bool? Dead,
    int? Parent,
    int? Poll,
    List<int>? Kids,
    string? Url,
    int Score,
    string Title,
    List<int>? Parts,
    int? Descendants
);