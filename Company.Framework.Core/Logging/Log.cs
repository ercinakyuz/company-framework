namespace Company.Framework.Core.Logging
{
    public record Log(DateTime At, string By)
    {
        public static Log Load(string by)
        {
            return new Log(DateTime.UtcNow, by);
        }
    }

}
