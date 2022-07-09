namespace red.gaius.brightbronze.core.Models
{
    public class UserWallet : UserData
    {
        public string brightbronze { get; set; }
        public string copper { get; set; }
        public string verdigry { get; set; }

        public override string structure
        {
            get { return "wallet"; }
        }
    }
}