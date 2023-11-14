using System.Globalization;
using System;

namespace GeoJSON.Tests
{
    public class WkbBaseUnitTester
    {

        #region Convertion Methods

        protected static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];

            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        protected static string ByteArrayTohexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        #endregion Convertion Methods

    }
}