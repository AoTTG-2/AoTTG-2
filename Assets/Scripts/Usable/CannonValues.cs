namespace OldCannon
{
    public class CannonValues
    {
        public string settings = string.Empty;
        public int viewID = -1;

        public CannonValues(int id, string str)
        {
            viewID = id;
            settings = str;
        }
    }
}