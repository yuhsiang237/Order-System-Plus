namespace OrderSystemPlus.Utils.OrderNumberTool
{
    public static class OrderNumberTool
    {
        public enum Type
        {
            Shipment = 1,
            ReturnShipment = 2,
        }

        /// <summary>
        /// generate ASCII string
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ASCII(int number)
        {
            return (char)number + "";
        }
        /// <summary>
        /// S + 20211122 + AHJZ5
        /// example: S20211122AHJZ5
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GenerateNumber(Type type)
        {
            var typePrefix = "";
            if (type == Type.Shipment)
                typePrefix = "S";
            if (type == Type.ReturnShipment)
                typePrefix = "R";

            string date = DateTime.Now.ToString("yyyyMMdd");
            string randStr = "";
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                randStr += ASCII(rnd.Next(65, 91)); // ASCII 65~90 A~Z
            }
            return typePrefix + date + randStr;
        }
    }
}
