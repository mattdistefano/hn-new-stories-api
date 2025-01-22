using System.Collections.Immutable;
using HackerNews.WebAPI.Services.HackerNewsApiClient;
using HackerNews.WebAPI.Services.HackerNewsService;
using Moq;

namespace TestProject1;

public class InMemoryHackerNewsNewStoriesServiceTests
{
    [Fact]
    public async Task NewStories_EmptyIds_ReturnsEmpty()
    {
        var mockClient = new Mock<IHackerNewsApiClient>();
        mockClient.Setup(client => client.GetNewStoryIds().Result).Returns(ImmutableList<int>.Empty);

        var service = new InMemoryHackerNewsNewStoriesService(mockClient.Object);

        var res = await service.GetNewStories(1, 5, null);

        Assert.Empty(res.Data);
        Assert.Equal(1, res.Meta.PageNumber);
        Assert.Equal(5, res.Meta.PageSize);
        Assert.Equal(0, res.Meta.TotalCount);
    }

    [Fact]
    public async Task NewStories_ManyIds_ReturnsPage()
    {
        var mockClient = new Mock<IHackerNewsApiClient>();
        mockClient.Setup(client => client.GetNewStoryIds().Result)
            .Returns(ImmutableList.Create(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        mockClient.Setup(client => client.GetItem(It.IsAny<int>()).Result).Returns((int id) =>
            new HackerNewsItem(id, null, "story", "foo", 1, "", null, null, null, null, null, 1, $"Title {id}", null,
                null));

        var service = new InMemoryHackerNewsNewStoriesService(mockClient.Object);

        var res = await service.GetNewStories(1, 5, null);

        Assert.Equal(5, res.Data.Count);
        Assert.Equal(1, res.Data[0].Id);
        Assert.Equal("Title 1", res.Data[0].Title);
        Assert.Equal(2, res.Data[1].Id);
        Assert.Equal("Title 2", res.Data[1].Title);
        Assert.Equal(3, res.Data[2].Id);
        Assert.Equal("Title 3", res.Data[2].Title);
        Assert.Equal(4, res.Data[3].Id);
        Assert.Equal("Title 4", res.Data[3].Title);
        Assert.Equal(5, res.Data[4].Id);
        Assert.Equal("Title 5", res.Data[4].Title);
        Assert.Equal(1, res.Meta.PageNumber);
        Assert.Equal(5, res.Meta.PageSize);
        Assert.Equal(10, res.Meta.TotalCount);
    }
}