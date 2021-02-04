using BAMCIS.GeoJSON;
using BAMCIS.GeoJSON.Wkb;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;

namespace GeoJSON.Tests
{
    public class WkbUnitTests
    {
        #region WkbConverter Tests

        [Fact]
        public void WkbConverterTest_ToBinary_BigEndian()
        {
            // ARRANGE
            byte[] expectedBytes = HexStringToByteArray("000000000140000000000000004010000000000000");
            Point point = new Point(new Position(2.0, 4.0));

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

        #endregion

        #region Point Tests

        [Fact]
        public void PointTest_Conversion()
        {
            // ARRANGE
            Point point = new Point(new Position(10.0, 10.0));

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
            Point point = new Point(new Position(10.0, 10.0));

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

        #endregion

        #region LineString Tests

        [Fact]
        public void LineStringTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // LINESTRING(30 10, 10 30, 40 40)
            byte[] bytes = HexStringToByteArray("000000000200000003403E00000000000040240000000000004024000000000000403E00000000000040440000000000004044000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            LineString lineString = Assert.IsType<LineString>(geo);
            Assert.Equal(3, lineString.Coordinates.Count());
            Assert.Equal(new Position(30, 10), lineString.Coordinates.ElementAt(0));
            Assert.Equal(new Position(10, 30), lineString.Coordinates.ElementAt(1));
            Assert.Equal(new Position(40, 40), lineString.Coordinates.ElementAt(2));
        }

        [Fact]
        public void LineStringTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // LINESTRING(30 10, 10 30, 40 40)
            byte[] bytes = HexStringToByteArray("0102000000030000000000000000003e40000000000000244000000000000024400000000000003e4000000000000044400000000000004440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            LineString lineString = Assert.IsType<LineString>(geo);
            Assert.Equal(3, lineString.Coordinates.Count());
            Assert.Equal(new Position(30, 10), lineString.Coordinates.ElementAt(0));
            Assert.Equal(new Position(10, 30), lineString.Coordinates.ElementAt(1));
            Assert.Equal(new Position(40, 40), lineString.Coordinates.ElementAt(2));
        }

        [Fact]
        public void LineStringTestWithDoubles_FromBinary_BigEndian()
        {
            // ARRANGE

            // LINESTRING(30.1234 10.6, 10.77 30.85, 40.1 40.2, 21 07, 19 77)
            byte[] bytes = HexStringToByteArray("000000000200000005403E1F972474538F402533333333333340258A3D70A3D70A403ED9999999999A40440CCCCCCCCCCD404419999999999A4035000000000000401C00000000000040330000000000004053400000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            LineString lineString = Assert.IsType<LineString>(geo);
            Assert.Equal(5, lineString.Coordinates.Count());
            Assert.Equal(new Position(30.1234, 10.6), lineString.Coordinates.ElementAt(0));
            Assert.Equal(new Position(10.77, 30.85), lineString.Coordinates.ElementAt(1));
            Assert.Equal(new Position(19, 77), lineString.Coordinates.ElementAt(4));
        }

        [Fact]
        public void LineStringTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // LINESTRING(30 10, 10 30, 40 40)
            byte[] bytes = HexStringToByteArray("000000000200000003403E00000000000040240000000000004024000000000000403E00000000000040440000000000004044000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void LineStringTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // LINESTRING(30 10, 10 30, 40 40)
            byte[] bytes = HexStringToByteArray("0102000000030000000000000000003e40000000000000244000000000000024400000000000003e4000000000000044400000000000004440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void LineStringTestWithDoubles_ToBinary_BigEndian()
        {
            // ARRANGE

            // LINESTRING(30.1234 10.6, 10.77 30.85, 40.1 40.2, 21 07, 19 77)
            byte[] bytes = HexStringToByteArray("000000000200000005403E1F972474538F402533333333333340258A3D70A3D70A403ED9999999999A40440CCCCCCCCCCD404419999999999A4035000000000000401C00000000000040330000000000004053400000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region MultiLineString Tests

        [Fact]
        public void MultiLineStringTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000050000000200000000020000000340240000000000004024000000000000403400000000000040340000000000004024000000000000404400000000000000000000020000000440440000000000004044000000000000403E000000000000403E00000000000040440000000000004034000000000000403E0000000000004024000000000000");
            
            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiLineString lineString = Assert.IsType<MultiLineString>(geo);
            Assert.Equal(2, lineString.Coordinates.Count());
            LineString ls1 = Assert.IsType<LineString>(lineString.Coordinates.ElementAt(1));
            Assert.Equal(40, ls1.Coordinates.ElementAt(2).Longitude);
            Assert.Equal(20, ls1.Coordinates.ElementAt(2).Latitude);
        }

        [Fact]
        public void MultiLineStringTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // MULTILINESTRING((10 10,20 20,10 40),(40 40,30 30,40 20,30 10))
            byte[] bytes = HexStringToByteArray("010500000002000000010200000003000000000000000000244000000000000024400000000000003440000000000000344000000000000024400000000000004440010200000004000000000000000000444000000000000044400000000000003e400000000000003e40000000000000444000000000000034400000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiLineString lineString = Assert.IsType<MultiLineString>(geo);
            Assert.Equal(2, lineString.Coordinates.Count());
            LineString ls1 = Assert.IsType<LineString>(lineString.Coordinates.ElementAt(1));
            Assert.Equal(40, ls1.Coordinates.ElementAt(2).Longitude);
            Assert.Equal(20, ls1.Coordinates.ElementAt(2).Latitude);
        }
  
        [Fact]
        public void MultiLineStringTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000050000000200000000020000000340240000000000004024000000000000403400000000000040340000000000004024000000000000404400000000000000000000020000000440440000000000004044000000000000403E000000000000403E00000000000040440000000000004034000000000000403E0000000000004024000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void MultiLineStringTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // MULTILINESTRING((10 10,20 20,10 40),(40 40,30 30,40 20,30 10))
            byte[] bytes = HexStringToByteArray("010500000002000000010200000003000000000000000000244000000000000024400000000000003440000000000000344000000000000024400000000000004440010200000004000000000000000000444000000000000044400000000000003e400000000000003e40000000000000444000000000000034400000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region MultiPoint Tests

        [Fact]
        public void MultiPointTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // MULTIPOINT(10 40,40 30,20 20,30 10)
            byte[] bytes = HexStringToByteArray("010400000004000000010100000000000000000024400000000000004440010100000000000000000044400000000000003e4001010000000000000000003440000000000000344001010000000000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiPoint mp = Assert.IsType<MultiPoint>(geo);
        }

        [Fact]
        public void MultiPointTest_FromBinary_BigEndian()           
        {
            // ARRANGE

            // MULTIPOINT ((21.06 19.77), (03.02 19.54), (40 20), (30 10))
            byte[] bytes = HexStringToByteArray("000000000400000004000000000140350F5C28F5C28F4033C51EB851EB850000000001400828F5C28F5C2940338A3D70A3D70A0000000001404400000000000040340000000000000000000001403E0000000000004024000000000000");
           
            
            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiPoint mp = Assert.IsType<MultiPoint>(geo);
            Assert.Equal(21.06, mp.Coordinates.ElementAt(0).Longitude);
            Assert.Equal(19.77, mp.Coordinates.ElementAt(0).Latitude);
        }

        [Fact]
        public void MultiPointTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // MULTIPOINT ((21.06 19.77), (03.02 19.54), (40 20), (30 10))
            byte[] bytes = HexStringToByteArray("000000000400000004000000000140350F5C28F5C28F4033C51EB851EB850000000001400828F5C28F5C2940338A3D70A3D70A0000000001404400000000000040340000000000000000000001403E0000000000004024000000000000");
            
            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void MultiPointTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // MULTIPOINT(10 40,40 30,20 20,30 10)
            byte[] bytes = HexStringToByteArray("010400000004000000010100000000000000000024400000000000004440010100000000000000000044400000000000003e4001010000000000000000003440000000000000344001010000000000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region MultiPolygon Tests

        [Fact]
        public void MultiPolygonTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))
            byte[] bytes = HexStringToByteArray("00000000060000000200000000030000000100000004403E00000000000040340000000000004046800000000000404400000000000040240000000000004044000000000000403E000000000000403400000000000000000000030000000100000005402E0000000000004014000000000000404400000000000040240000000000004024000000000000403400000000000040140000000000004024000000000000402E0000000000004014000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiPolygon mp = Assert.IsType<MultiPolygon>(geo);
        }

        [Fact]
        public void MultiPolygonTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))
            byte[] bytes = HexStringToByteArray("010600000002000000010300000001000000040000000000000000003e40000000000000344000000000008046400000000000004440000000000000244000000000000044400000000000003e400000000000003440010300000001000000050000000000000000002e4000000000000014400000000000004440000000000000244000000000000024400000000000003440000000000000144000000000000024400000000000002e400000000000001440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            MultiPolygon mp = Assert.IsType<MultiPolygon>(geo);
        }

        [Fact]
        public void MultiPolygonTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))
            byte[] bytes = HexStringToByteArray("010600000002000000010300000001000000040000000000000000003e40000000000000344000000000008046400000000000004440000000000000244000000000000044400000000000003e400000000000003440010300000001000000050000000000000000002e4000000000000014400000000000004440000000000000244000000000000024400000000000003440000000000000144000000000000024400000000000002e400000000000001440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void MultiPolygonTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // MULTIPOLYGON(((30 20, 45 40, 10 40, 30 20)), ((15 5, 40 10, 10 20, 5 10, 15 5)))
            byte[] bytes = HexStringToByteArray("00000000060000000200000000030000000100000004403E00000000000040340000000000004046800000000000404400000000000040240000000000004044000000000000403E000000000000403400000000000000000000030000000100000005402E0000000000004014000000000000404400000000000040240000000000004024000000000000403400000000000040140000000000004024000000000000402E0000000000004014000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region Polygon Tests

        [Fact]
        public void PolygonTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000030000000100000005403E0000000000004024000000000000404400000000000040440000000000004034000000000000404400000000000040240000000000004034000000000000403E0000000000004024000000000000");
            
            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            Polygon polygon = Assert.IsType<Polygon>(geo);
        }

        [Fact]
        public void PolygonTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // POLYGON((30 10,40 40,20 40,10 20,30 10))
            byte[] bytes = HexStringToByteArray("010300000001000000050000000000000000003e4000000000000024400000000000004440000000000000444000000000000034400000000000004440000000000000244000000000000034400000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            Polygon polygon = Assert.IsType<Polygon>(geo);
        }

        [Fact]
        public void PolygonTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // POLYGON((30 10,40 40,20 40,10 20,30 10))
            byte[] bytes = HexStringToByteArray("010300000001000000050000000000000000003e4000000000000024400000000000004440000000000000444000000000000034400000000000004440000000000000244000000000000034400000000000003e400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void PolygonTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000030000000100000005403E0000000000004024000000000000404400000000000040440000000000004034000000000000404400000000000040240000000000004034000000000000403E0000000000004024000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region GeometryCollection Tests

        [Fact]
        public void GeometryCollectionTest_FromBinary_BigEndian()
        {
            // ARRANGE

            // GEOMETRYCOLLECTION(POINT (40 10),LINESTRING(10 10, 20 20, 10 40),POLYGON((40 40, 20 45, 45 30, 40 40)))
            byte[] bytes = HexStringToByteArray("0000000007000000030000000001404400000000000040240000000000000000000002000000034024000000000000402400000000000040340000000000004034000000000000402400000000000040440000000000000000000003000000010000000440440000000000004044000000000000403400000000000040468000000000004046800000000000403E00000000000040440000000000004044000000000000");
            
            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            GeometryCollection geoCollection = Assert.IsType<GeometryCollection>(geo);
            Point point = Assert.IsType<Point>(geoCollection.Geometries.ElementAt(0));
            LineString lineString = Assert.IsType<LineString>(geoCollection.Geometries.ElementAt(1));
            Polygon polygon = Assert.IsType<Polygon>(geoCollection.Geometries.ElementAt(2));
            Assert.Equal(40, point.Coordinates.Longitude);
            Assert.Equal(10, point.Coordinates.Latitude);
        }

        [Fact]
        public void GeometryCollectionTest_FromBinary_LittleEndian()
        {
            // ARRANGE

            // GEOMETRYCOLLECTION(POINT(4 6),LINESTRING(4 6,7 10))
            byte[] bytes = HexStringToByteArray("010700000002000000010100000000000000000010400000000000001840010200000002000000000000000000104000000000000018400000000000001c400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);

            // ASSERT
            GeometryCollection geoCollection = Assert.IsType<GeometryCollection>(geo);
            Point point = Assert.IsType<Point>(geoCollection.Geometries.ElementAt(0));
            LineString lineString = Assert.IsType<LineString>(geoCollection.Geometries.ElementAt(1));
            Assert.Equal(4, point.Coordinates.Longitude);
            Assert.Equal(6, point.Coordinates.Latitude);
        }

        [Fact]
        public void GeometryCollectionTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            // GEOMETRYCOLLECTION(POINT(4 6),LINESTRING(4 6,7 10))
            byte[] bytes = HexStringToByteArray("010700000002000000010100000000000000000010400000000000001840010200000002000000000000000000104000000000000018400000000000001c400000000000002440");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        [Fact]
        public void GeometryCollectionTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // GEOMETRYCOLLECTION(POINT (40 10),LINESTRING(10 10, 20 20, 10 40),POLYGON((40 40, 20 45, 45 30, 40 40)))
            byte[] bytes = HexStringToByteArray("0000000007000000030000000001404400000000000040240000000000000000000002000000034024000000000000402400000000000040340000000000004034000000000000402400000000000040440000000000000000000003000000010000000440440000000000004044000000000000403400000000000040468000000000004046800000000000403E00000000000040440000000000004044000000000000");

            // ACT
            Geometry geo = WkbConverter.FromBinary(bytes);
            byte[] newBytes = WkbConverter.ToBinary(geo, Endianness.BIG);

            // ASSERT
            Assert.Equal(bytes, newBytes);
        }

        #endregion

        #region Private Methods

        private static byte[] HexStringToByteArray(string hexString)
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

        #endregion
    }
}
