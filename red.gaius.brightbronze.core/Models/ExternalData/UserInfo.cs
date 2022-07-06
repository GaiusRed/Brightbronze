namespace red.gaius.brightbronze.core.Models
{
    public class UserInfo : UserData
    {
        public string name { get; set; }
        public string discriminator { get; set; }

        public override string structure
        {
            get { return "info"; }
        }
    }
}