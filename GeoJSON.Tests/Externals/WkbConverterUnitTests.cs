using BAMCIS.GeoJSON;
using BAMCIS.GeoJSON.Externals;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace GeoJSON.Tests.Externals
{
    public class WkbConverterUnitTests
    {
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


        [Fact]
        public void PointTests()
        {
            // POINT(2.0 4.0)  BIG ENDIAN
            byte[] bytes = HexStringToByteArray("000000000140000000000000004010000000000000");

            // From
            Point geom = Assert.IsType<Point>(WkbConverter.From(bytes));
            Assert.Equal(2.0, geom.Coordinates.Longitude);
            Assert.Equal(4.0, geom.Coordinates.Latitude);

            // To
            var convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // From
            // POINT(1.2345 2.3456) LITTLE ENDIAN
            bytes = HexStringToByteArray("01010000008D976E1283C0F33F16FBCBEEC9C30240");
            geom = Assert.IsType<Point>(WkbConverter.From(bytes));
            Assert.Equal(1.2345, geom.Coordinates.Longitude);
            Assert.Equal(2.3456, geom.Coordinates.Latitude);

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);

            // From
            // POINT(1.2345 2.3456) BIG ENDIAN
            bytes = HexStringToByteArray("00000000013FF3C083126E978D4002C3C9EECBFB16");
            geom = Assert.IsType<Point>(WkbConverter.From(bytes));
            Assert.Equal(1.2345, geom.Coordinates.Longitude);
            Assert.Equal(2.3456, geom.Coordinates.Latitude);

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);

            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }



        [Fact]
        public void LineStringTests()
        {
            // LINESTRING(30 10, 10 30, 40 40)
            byte[] bytes = HexStringToByteArray("000000000200000003403E00000000000040240000000000004024000000000000403E00000000000040440000000000004044000000000000");

            // From
            var geom = Assert.IsType<LineString>(WkbConverter.From(bytes));
            Assert.Equal(3, geom.Coordinates.Count());
            Assert.Equal(new Position(30, 10), geom.Coordinates.ElementAt(0));
            Assert.Equal(new Position(10, 30), geom.Coordinates.ElementAt(1));
            Assert.Equal(new Position(40, 40), geom.Coordinates.ElementAt(2));

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // LINESTRING(30.1234 10.6, 10.77 30.85, 40.1 40.2, 21 07, 19 77)
            bytes = HexStringToByteArray("000000000200000005403E1F972474538F402533333333333340258A3D70A3D70A403ED9999999999A40440CCCCCCCCCCD404419999999999A4035000000000000401C00000000000040330000000000004053400000000000");

            // From
            geom = Assert.IsType<LineString>(WkbConverter.From(bytes));
            Assert.Equal(5, geom.Coordinates.Count());
            Assert.Equal(new Position(30.1234, 10.6), geom.Coordinates.ElementAt(0));
            Assert.Equal(new Position(10.77, 30.85), geom.Coordinates.ElementAt(1));
            Assert.Equal(new Position(19, 77), geom.Coordinates.ElementAt(4));

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);

            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }


        [Fact]
        public void PolygonTests()
        {
            // POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000030000000100000005403E0000000000004024000000000000404400000000000040440000000000004034000000000000404400000000000040240000000000004034000000000000403E0000000000004024000000000000");

            // From
            Polygon geom = Assert.IsType<Polygon>(WkbConverter.From(bytes));
            Assert.True(1 == geom.Coordinates.Count());
            Assert.True(5 == geom.Coordinates.First().Coordinates.Count());

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);



            // POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10), (20 30, 35 35, 30 20, 20 30))
            bytes = HexStringToByteArray("000000000300000002000000054041800000000000402400000000000040468000000000004046800000000000402E00000000000040440000000000004024000000000000403400000000000040418000000000004024000000000000000000044034000000000000403E00000000000040418000000000004041800000000000403E00000000000040340000000000004034000000000000403E000000000000");

            // From
            geom = Assert.IsType<Polygon>(WkbConverter.From(bytes));
            Assert.True(2 == geom.Coordinates.Count());
            Assert.True(5 == geom.Coordinates.First().Coordinates.Count());
            Assert.True(4 == geom.Coordinates.Last().Coordinates.Count());

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }


        [Fact]
        public void MultiPointTests()
        {
            // MULTIPOINT ((21.06 19.77), (03.02 19.54), (40 20), (30 10))
            byte[] bytes = HexStringToByteArray("000000000400000004000000000140350F5C28F5C28F4033C51EB851EB850000000001400828F5C28F5C2940338A3D70A3D70A0000000001404400000000000040340000000000000000000001403E0000000000004024000000000000");

            // From
            MultiPoint geom = Assert.IsType<MultiPoint>(WkbConverter.From(bytes));
            Assert.True(4 == geom.Coordinates.Count());
            Assert.Equal(3.02, geom.Coordinates.ElementAt(1).Longitude);
            Assert.Equal(19.54, geom.Coordinates.ElementAt(1).Latitude);
            Assert.Equal(30, geom.Coordinates.ElementAt(3).Longitude);
            Assert.Equal(10, geom.Coordinates.ElementAt(3).Latitude);

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);



            // MULTIPOINT ((10 40), (40 30), (20 20), (30 10))
            bytes = HexStringToByteArray("00000000040000000400000000014024000000000000404400000000000000000000014044000000000000403E0000000000000000000001403400000000000040340000000000000000000001403E0000000000004024000000000000");

            // From
            geom = Assert.IsType<MultiPoint>(WkbConverter.From(bytes));
            Assert.True(4 == geom.Coordinates.Count());
            Assert.Equal(40, geom.Coordinates.ElementAt(1).Longitude);
            Assert.Equal(30, geom.Coordinates.ElementAt(1).Latitude);
            Assert.Equal(30, geom.Coordinates.ElementAt(3).Longitude);
            Assert.Equal(10, geom.Coordinates.ElementAt(3).Latitude);

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }


        [Fact]
        public void MultiLineStringTests()
        {
            // MULTILINESTRING ((10 10, 20 20, 10 40), (40 40, 30 30, 40 20, 30 10))
            byte[] bytes = HexStringToByteArray("00000000050000000200000000020000000340240000000000004024000000000000403400000000000040340000000000004024000000000000404400000000000000000000020000000440440000000000004044000000000000403E000000000000403E00000000000040440000000000004034000000000000403E0000000000004024000000000000");

            // From
            MultiLineString geom = Assert.IsType<MultiLineString>(WkbConverter.From(bytes));
            Assert.True(2 == geom.Coordinates.Count());

            LineString ls1 = Assert.IsType<LineString>(geom.Coordinates.ElementAt(1));
            Assert.Equal(40, ls1.Coordinates.ElementAt(2).Longitude);
            Assert.Equal(20, ls1.Coordinates.ElementAt(2).Latitude);

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // MULTILINESTRING ((10.21 10, 20.06 20.19, 10.77 40), (40 40, 30 30, 40 20, 30 10))
            bytes = HexStringToByteArray("00000000050000000200000000020000000340246B851EB851EC402400000000000040340F5C28F5C28F403430A3D70A3D7140258A3D70A3D70A404400000000000000000000020000000440440000000000004044000000000000403E000000000000403E00000000000040440000000000004034000000000000403E0000000000004024000000000000");

            // From
            geom = Assert.IsType<MultiLineString>(WkbConverter.From(bytes));
            Assert.True(2 == geom.Coordinates.Count());

            ls1 = Assert.IsType<LineString>(geom.Coordinates.ElementAt(0));
            Assert.Equal(20.19, ls1.Coordinates.ElementAt(1).Latitude);
            Assert.Equal(10.77, ls1.Coordinates.ElementAt(2).Longitude);

            // To
            convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }



        [Fact]
        public void MultiPolygonTests()
        {
            // MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))
            byte[] bytes = HexStringToByteArray("00000000060000000200000000030000000100000004403E00000000000040340000000000004046800000000000404400000000000040240000000000004044000000000000403E000000000000403400000000000000000000030000000100000005402E0000000000004014000000000000404400000000000040240000000000004024000000000000403400000000000040140000000000004024000000000000402E0000000000004014000000000000");

            // From
            MultiPolygon geom = Assert.IsType<MultiPolygon>(WkbConverter.From(bytes));
            Assert.True(2 == geom.Coordinates.Count());

            Polygon item = Assert.IsType<Polygon>(geom.Coordinates.ElementAt(0));
            var childItem = item.Coordinates.ElementAt(0);
            Assert.True(4 == childItem.Coordinates.Count());

            var grantChildItem = childItem.Coordinates.ElementAt(1);
            Assert.Equal(45, grantChildItem.Longitude);
            Assert.Equal(40, grantChildItem.Latitude);

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);
        }


        [Fact]
        public void GeometryCollectionTests()
        {
            // GEOMETRYCOLLECTION(POINT (40 10),LINESTRING(10 10, 20 20, 10 40),POLYGON((40 40, 20 45, 45 30, 40 40)))
            byte[] bytes = HexStringToByteArray("0000000007000000030000000001404400000000000040240000000000000000000002000000034024000000000000402400000000000040340000000000004034000000000000402400000000000040440000000000000000000003000000010000000440440000000000004044000000000000403400000000000040468000000000004046800000000000403E00000000000040440000000000004044000000000000");

            // From
            GeometryCollection geom = Assert.IsType<GeometryCollection>(WkbConverter.From(bytes));
            Assert.True(3 == geom.Geometries.Count());

            Geometry item1 = Assert.IsAssignableFrom<Geometry>(geom.Geometries.ElementAt(0));
            Assert.Equal(GeoJsonType.Point, item1.Type);
            Point point = Assert.IsType<Point>(item1);
            Assert.Equal(40, point.GetLongitude());
            Assert.Equal(10, point.GetLatitude());

            Geometry item2 = Assert.IsAssignableFrom<Geometry>(geom.Geometries.ElementAt(1));
            Assert.Equal(GeoJsonType.LineString, item2.Type);
            LineString lineString = Assert.IsType<LineString>(item2);
            Assert.Equal(10, lineString.Coordinates.ElementAt(2).Longitude);
            Assert.Equal(40, lineString.Coordinates.ElementAt(2).Latitude);

            Geometry item3 = Assert.IsAssignableFrom<Geometry>(geom.Geometries.ElementAt(2));
            Assert.Equal(GeoJsonType.Polygon, item3.Type);
            Polygon polygon = Assert.IsType<Polygon>(item3);
            Assert.Equal(20, polygon.Coordinates.ElementAt(0).Coordinates.ElementAt(1).Longitude);
            Assert.Equal(45, polygon.Coordinates.ElementAt(0).Coordinates.ElementAt(1).Latitude);

            // To
            byte[] convertedBytes = WkbConverter.To(geom, useLittleEndian: bytes[0] == 1);
            Assert.Equal(bytes, convertedBytes);


            // To with geoJson call
            convertedBytes = geom.ToWkb();
            Assert.Equal(bytes, convertedBytes);

            var geomConverted = GeoJson.FromWkb(convertedBytes);
            Assert.Equal(geom, geomConverted);
        }

    }
}
