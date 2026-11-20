namespace Shared.Redis
{
    public class RedisOptions
    {
        public const string SectionName = "Redis";
        public string ConnectionString { get; set; } = null!;
        public int Database { get; set; } = 0;
    }
}
