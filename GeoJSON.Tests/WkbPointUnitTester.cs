using BAMCIS.GeoJSON;
using BAMCIS.GeoJSON.Wkb;
using Xunit;

namespace GeoJSON.Tests
{
    public class WkbPointUnitTester : WkbBaseUnitTester
    {
        [Fact]
        public void WkbConverterTest_ToBinary_BigEndian()
        {
            // ARRANGE
            byte[] expectedBytes = HexStringToByteArray("000000000140000000000000004010000000000000");
            var point = new Point(new Coordinate(2.0, 4.0));

            // ACT
            byte[] bytes = WkbConverter.ToBinary(point, Endianness.BIG);

            // ASSERT
            Assert.Equal(expectedBytes, bytes);
        }

        [Fact]
        public void WkbConverterTest_FromBinary_BigEndian()
        {
            // ARRANGE
            byte[] bytes = HexStringToByteArray("000000000140000000000000004010000000000000");

            // ACT
            Point point = WkbConverter.FromBinary<Point>(bytes);

            // ASSERT
            Assert.Equal(2.0, point.GetLongitude());
            Assert.Equal(4.0, point.GetLatitude());
        }

        [Fact]
        public void PointTest_Conversion()
        {
            // ARRANGE
            var point = new Point(new Coordinate(10.0, 10.0));

            // ACT
            byte[] bytes = point.ToWkb();
            Geometry geo = Point.FromWkb(bytes);

            // ASSERT
            point = Assert.IsType<Point>(geo);
        }

        [Fact]
        public void PointTest_Conversion2()
        {
            // ARRANGE
            var point = new Point(new Coordinate(10.0, 10.0));

            // ACT
            byte[] bytes = point.ToWkb();
            point = Geometry.FromWkb<Point>(bytes);

            // ASSERT
            point = Assert.IsType<Point>(point);
            Assert.Equal(10.0, point.GetLongitude());
            Assert.Equal(10.0, point.GetLatitude());
        }

        [Fact]
        public void PointTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // POINT(2.0 4.0) BIG ENDIAN
            byte[] bytes = HexStringToByteArray("000000000140000000000000004010000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            Point point = Assert.IsType<Point>(geo);
            Assert.Equal(2.0, point.Coordinates.Longitude);
            Assert.Equal(4.0, point.Coordinates.Latitude);
        }

        [Fact]
        public void PointTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // POINT(1.2345 2.3456) LITTLE ENDIAN
            byte[] bytes = HexStringToByteArray("01010000008D976E1283C0F33F16FBCBEEC9C30240");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            Point point = Assert.IsType<Point>(geo);
            Assert.Equal(1.2345, point.Coordinates.Longitude);
            Assert.Equal(2.3456, point.Coordinates.Latitude);
        }

        [Fact]
        public void PointTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // POINT(2.0 4.0) BIG ENDIAN
            byte[] bytes = HexStringToByteArray("000000000140000000000000004010000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void PointTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // POINT(1.2345 2.3456) LITTLE ENDIAN
            byte[] bytes = HexStringToByteArray("01010000008D976E1283C0F33F16FBCBEEC9C30240");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }
    }
}