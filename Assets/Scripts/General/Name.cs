namespace Assets.Scripts.General
{
    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Name()
        {
            FirstName = "";
            LastName = "";
        }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }
    }
}