namespace red.gaius.brightbronze.core.Models
{
    public class UserWallet : Item
    {
        public string userId { get; set; }
        public string verdigry { get; set; }
        public string copper { get; set; }
        public string energy { get; set; }
        public string energyLastUpdate { get; set; }
    }
}