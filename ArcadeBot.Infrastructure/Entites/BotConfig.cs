namespace ArcadeBot.Infrastructure.Entities
{
    public class BotConfig
    {
        public string Token { get; set; } = null!;
        public string MemeFolder { get; set; } = null!;
        public int? MaxShards { get; set; } = null!;

        #region Proxy
        public bool UseProxy { get; set; }
        public string ProxyHost { get; set; } = null!;
        public int? ProxyPort { get; set; }
        #endregion
    }
}