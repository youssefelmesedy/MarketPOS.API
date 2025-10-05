public partial class RateLimitingMiddleware
{
    private class RateLimitEntry
    {
        public int Count { get; set; }
        public DateTime WindowStartUtc { get; set; }
    }
}