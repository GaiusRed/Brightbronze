namespace red.gaius.brightbronze.core.Models
{
    public class UserWallet : UserData
    {
        public string verdigry { get; set; }
        public string copper { get; set; }
        public string energy { get; set; }
        public string energyLastUpdate { get; set; }

        public override string structure
        {
            get { return "wallet"; }
        }
    }
}