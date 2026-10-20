using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RssFeedReader.Api.Subscriptions;

namespace RssFeedReader.Api.Tests.Integration;

public class SubscriptionsApiTests
{
    private readonly HttpClient _client;

    public SubscriptionsApiTests()
    {
        _client = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<InMemorySubscriptionStore>();
                });
            })
            .CreateClient();
    }

    [Fact]
    public async Task Get_WhenEmpty_ReturnsEmptyList()
    {
        var response = await _client.GetAsync("/api/subscriptions");

        response.EnsureSuccessStatusCode();
        var subscriptions = await response.Content.ReadFromJsonAsync<List<FeedSubscription>>();

        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions!);
    }

    [Fact]
    public async Task Post_WhenNewUrl_ReturnsCreatedSubscription()
    {
        var payload = new { url = "https://example.test/feed.xml" };

        var response = await _client.PostAsJsonAsync("/api/subscriptions", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<FeedSubscription>();
        Assert.NotNull(content);
        Assert.Equal("https://example.test/feed.xml", content!.Url);
    }

    [Fact]
    public async Task Post_WhenDuplicateUrl_ReturnsOkWithoutAddingAnotherRow()
    {
        var payload = new { url = "https://example.test/feed.xml" };

        var first = await _client.PostAsJsonAsync("/api/subscriptions", payload);
        var second = await _client.PostAsJsonAsync("/api/subscriptions", payload);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.OK, second.StatusCode);

        var list = await _client.GetFromJsonAsync<List<FeedSubscription>>("/api/subscriptions");

        Assert.NotNull(list);
        Assert.Single(list!);
        Assert.Equal("https://example.test/feed.xml", list[0].Url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Post_WhenUrlIsMissingOrInvalid_ReturnsBadRequest(string? value)
    {
        HttpContent content;
        if (value is null)
        {
            content = new StringContent("null", Encoding.UTF8, "application/json");
        }
        else
        {
            content = JsonContent.Create(new { url = value });
        }

        var response = await _client.PostAsync("/api/subscriptions", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
