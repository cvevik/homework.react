using RichardSzalay.MockHttp;

public static class MockHttpMessageHandlerExtensions
{
    public static IHttpClientFactory ToHttpClientFactory(this MockHttpMessageHandler mockHttpMessageHandler)
    {
        return new MockHttpClientFactory(mockHttpMessageHandler);
    }
}
