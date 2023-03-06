namespace OrderSystemPlus.Utils.GUIDSerialTool
{
    public class GUIDSerialTool
    {
        /// <summary>
        /// Generate a new GUID
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Generate a new GUID and get first N chars with length
        /// </summary>
        /// <returns></returns>
        public static string Generate(int length)
        {
            return Guid.NewGuid()
                .ToString("N")
                .Substring(0, length);
        }
    }
}
