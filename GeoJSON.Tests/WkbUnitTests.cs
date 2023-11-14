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
    public class WkbUnitTests : WkbBaseUnitTester
    {
       

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
            Assert.True(lineString.LineSegments.Count() == 2, "Length of the LineSegment does not match");
            Assert.Equal(new Coordinate(30, 10), lineString.LineSegments.ElementAt(0).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(10, 30), lineString.LineSegments.ElementAt(1).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(40, 40), lineString.LineSegments.ElementAt(1).ElementAt(1).Coordinates);
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
            Assert.True(lineString.LineSegments.Count() == 2, "Length of the LineSegment does not match");
            Assert.Equal(new Coordinate(30, 10), lineString.LineSegments.ElementAt(0).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(10, 30), lineString.LineSegments.ElementAt(1).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(40, 40), lineString.LineSegments.ElementAt(1).ElementAt(1).Coordinates);
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
            Assert.Equal(4, lineString.LineSegments.Count());
            Assert.Equal(new Coordinate(30.1234, 10.6), lineString.LineSegments.ElementAt(0).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(10.77, 30.85), lineString.LineSegments.ElementAt(1).ElementAt(0).Coordinates);
            Assert.Equal(new Coordinate(19, 77), lineString.LineSegments.ElementAt(3).ElementAt(1).Coordinates);
        }

        [Fact]
        public void LineStringTest_ToBinary_BigEndian()
        {
            // ARRANGE

            var geo = new LineString(new List<Point>{ new Point(new Coordinate(30, 10)), new Point(new Coordinate(10, 30)), new Point(new Coordinate(40, 40)) } );

            // ACT
            byte[] bytes = WkbConverter.ToBinary(geo, Endianness.BIG);
            Geometry geoReconstituted = WkbConverter.FromBinary(bytes);

            // ASSERT
            Assert.True(geo.Equals(geoReconstituted), "Geometries are not equivalent.");
        }

        [Fact]
        public void LineStringTest_ToBinary_LittleEndian()
        {
            // ARRANGE

            var geo = new LineString(new List<Point> { new Point(new Coordinate(30, 10)), new Point(new Coordinate(10, 30)), new Point(new Coordinate(40, 40)) });

            // ACT
            byte[] bytes = WkbConverter.ToBinary(geo, Endianness.LITTLE);
            Geometry geoReconstituted = WkbConverter.FromBinary(bytes);

            // ASSERT
            Assert.True(geo.Equals(geoReconstituted), "Geometries are not equivalent.");
        }

        [Fact]
        public void LineStringTestWithDoubles_ToBinary_BigEndian()
        {
            // ARRANGE

            // LINESTRING(30.1234 10.6, 10.77 30.85, 40.1 40.2, 21 07, 19 77)
            var geo = new LineString(new List<Point> { new Point(new Coordinate(30, 10)), new Point(new Coordinate(10, 30)), new Point(new Coordinate(40, 40)) });

            // ACT
            byte[] bytes = WkbConverter.ToBinary(geo, Endianness.BIG);
            Geometry geoReconstituted = WkbConverter.FromBinary(bytes);

            // ASSERT
            Assert.True(geo.Equals(geoReconstituted), "Geometries are not equivalent.");
        }


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
            Assert.Equal(2, lineString.LineStrings.Count());
            LineString ls1 = Assert.IsType<LineString>(lineString.LineStrings.ElementAt(1));
            Assert.Equal(40, ls1.LineSegments.ElementAt(2).ElementAt(0).Coordinates.Longitude);
            Assert.Equal(20, ls1.LineSegments.ElementAt(2).ElementAt(0).Coordinates.Latitude);
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
            Assert.Equal(2, lineString.LineStrings.Count());
            LineString ls1 = Assert.IsType<LineString>(lineString.LineStrings.ElementAt(1));
            Assert.Equal(40, ls1.LineSegments.ElementAt(2).ElementAt(0).Coordinates.Longitude);
            Assert.Equal(20, ls1.LineSegments.ElementAt(2).ElementAt(0).Coordinates.Latitude);
        }
  
        [Fact]
        public void MultiLineStringTest_ToBinary_BigEndian()
        {
            // ARRANGE

            // MULTILINESTRING((10 10,20 20,10 40),(40 40,30 30,40 20,30 10))
            byte[] bytes = HexStringToByteArray("0000000005000000020000000002000000044024000000000000402400000000000040340000000000004034000000000000403400000000000040340000000000004024000000000000404400000000000000000000020000000640440000000000004044000000000000403E000000000000403E000000000000403E000000000000403E0000000000004044000000000000403400000000000040440000000000004034000000000000403E0000000000004024000000000000");

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
            Assert.Equal(21.06, mp.Points.ElementAt(0).GetLongitude());
            Assert.Equal(19.77, mp.Points.ElementAt(0).GetLatitude());
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


    }
}
