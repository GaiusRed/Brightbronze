namespace red.gaius.brightbronze.core.Models
{
    public class ServerInfo : ServerData
    {
        public string name { get; set; }
        public string ownerUserId { get; set; }

        public override string structure
        {
            get { return "info"; }
        }
    }
}