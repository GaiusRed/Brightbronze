namespace red.gaius.brightbronze.core.Models
{
    public class UserInfo : Item
    {
        public string userId { get; set; }
        public string name { get; set; }
        public string discriminator { get; set; }
    }
}