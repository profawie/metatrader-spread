

namespace MT4Price.Server.Model
{
    public class PltPlatform
    {
        public int PlatformId
        { get; set; }

        public int AccountId
        { get; set; }

        public string PlatformType
        { get; set; }

        public string Host
        { get; set; }

        public int Port
        { get; set; }

        public int Login
        { get; set; }

        public string Password
        { get; set; }
    }
}
