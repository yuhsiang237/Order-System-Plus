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

            DateTime now = DateTime.Now;
            Guid guid = Guid.NewGuid();

            string timePart = now.ToString("yyyyMMdd");
            string guidPart = guid.ToString().Substring(0, 6).ToUpper();
            string orderNumber = typePrefix + timePart + guidPart;
            return orderNumber;
        }
    }
}
