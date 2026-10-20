using RssFeedReader.Api.Subscriptions;

namespace RssFeedReader.Api.Tests.Unit;

public class InMemorySubscriptionStoreTests
{
    [Fact]
    public void NewStore_StartsEmpty()
    {
        var store = new InMemorySubscriptionStore();

        Assert.Empty(store.GetAll());
    }

    [Fact]
    public void Add_RejectsNullEmptyAndWhitespace()
    {
        var store = new InMemorySubscriptionStore();

        Assert.Throws<ArgumentException>(() => store.Add(null!));
        Assert.Throws<ArgumentException>(() => store.Add(string.Empty));
        Assert.Throws<ArgumentException>(() => store.Add("   "));
    }

    [Fact]
    public void Add_PreservesNonBlankValueWithoutNormalization()
    {
        var store = new InMemorySubscriptionStore();
        var value = " https://example.test/feed.xml ";

        var added = store.Add(value);

        var item = Assert.Single(store.GetAll());
        Assert.Equal(value, item.Url);
        Assert.Equal(value, added.Url);
    }

    [Fact]
    public void Add_RejectsExactDuplicate_AndPreservesInsertionOrder()
    {
        var store = new InMemorySubscriptionStore();

        var first = store.Add("https://example.test/feed.xml");
        var duplicate = store.Add("https://example.test/feed.xml");
        var second = store.Add("https://example.test/second.xml");

        Assert.Equal(first.Url, duplicate.Url);
        Assert.Equal(2, store.GetAll().Count);
        Assert.Collection(
            store.GetAll(),
            item => Assert.Equal("https://example.test/feed.xml", item.Url),
            item => Assert.Equal("https://example.test/second.xml", item.Url));
        Assert.Equal("https://example.test/second.xml", second.Url);
    }

    [Fact]
    public void Add_UsesOrdinalCaseSensitiveComparison()
    {
        var store = new InMemorySubscriptionStore();

        store.Add("https://example.test/feed.xml");
        store.Add("HTTPS://EXAMPLE.TEST/FEED.XML");

        Assert.Equal(2, store.GetAll().Count);
        Assert.Equal("https://example.test/feed.xml", store.GetAll()[0].Url);
        Assert.Equal("HTTPS://EXAMPLE.TEST/FEED.XML", store.GetAll()[1].Url);
    }
}
