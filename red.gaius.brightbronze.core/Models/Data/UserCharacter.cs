namespace red.gaius.brightbronze.core.Models
{
    public class UserCharacter : UserData
    {
        public string nickname { get; set; }
        public ulong seed { get; set; }

        public override string structure
        {
            get { return "character"; }
        }
    }
}