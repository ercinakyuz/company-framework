namespace Company.Framework.Core.Logging
{
    //public record Log(DateTime At, string By)
    //{
    //    public static Log Load(string by)
    //    {
    //        return new Log(DateTime.UtcNow, by);
    //    }
    //}

    public class Log
    {
        public DateTime At { get; set; }

        public string By { get; set; }

        public static Log Load(string by)
        {
            return new Log { At = DateTime.UtcNow, By = by };
        }
    }

}
