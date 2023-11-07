using BAMCIS.GeoJSON;
using BAMCIS.GeoJSON.Wkb;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class WkbGeometryCollectionUnitTester : WkbBaseUnitTester
    {
        #region geometry generator
        /// <summary>
        /// GEOMETRYCOLLECTION(POINT (40 10),LINESTRING(10 10, 20 20, 10 40),POLYGON((40 40, 20 45, 45 30, 40 40)))
        /// </summary>
        /// <param name="point"></param>
        /// <param name="linearRing"></param>
        /// <param name="polygon"></param>
        /// <param name="geomColl"></param>
        private static void FetchDefaultGeometryCollectionTest(out Point point, out LinearRing linearRing, out Polygon polygon, out GeometryCollection geomColl)
        {
            point = new Point(40, 10);
            var lineString = new LineString(new List<Point> { new Point(10, 10), new Point(20, 20), new Point(10, 40) });


            linearRing = new LinearRing(new List<List<Coordinate>>{ new List<Coordinate> { new Coordinate(40, 40) ,
                                                                                           new Coordinate(20,45) ,
                                                                                           new Coordinate(45, 30) ,
                                                                                           new Coordinate(40,40)
                                                                                         }

                                                                    }
                                           );
            polygon = new Polygon(linearRing);
            geomColl = new GeometryCollection(new List<Geometry> { point, lineString, polygon });
        }


        #endregion geometry generator


        #region Tests

        [Fact]
        public void GeometryCollectionTest_FromBinary_BigEndian()
        {
            // ARRANGE


            FetchDefaultGeometryCollectionTest(out Point point,
                                               out LinearRing linearRing,
                                               out Polygon polygon,
                                               out GeometryCollection geomColl);

            // ACT
            byte[] bytes = WkbConverter.ToBinary(geomColl);

            Geometry geo = WkbConverter.FromBinary(bytes);

            GeometryCollection geoCollection = Assert.IsType<GeometryCollection>(geo);
            Point pointReconstituted = Assert.IsType<Point>(geoCollection.Geometries.ElementAt(0));
            LineString lineStringReconstituted = Assert.IsType<LineString>(geoCollection.Geometries.ElementAt(1));
            Polygon polygonReconstituted = Assert.IsType<Polygon>(geoCollection.Geometries.ElementAt(2));


            // ASSERT

            Assert.True(lineStringReconstituted.Equals(linearRing), "Reconstituted LinearRing is not equivalent to the original one");
            Assert.True(polygonReconstituted.Equals(polygon), "Reconstituted Polygon is not equivalent to the original one");
            Assert.True(pointReconstituted.Equals(point), "Reconstituted Point is not equivalent to the original one");
            Assert.Equal(40, point.Coordinates.Longitude);
            Assert.Equal(10, point.Coordinates.Latitude);
        }

        

        [Fact]
        public void GeometryCollectionTest_ToBinary_BigEndian()
        {
            // ARRANGE

            WkbGeometryCollectionUnitTester.FetchDefaultGeometryCollectionTest(out Point point,
                                                                               out LinearRing linearRing,
                                                                               out Polygon polygon,
                                                                               out GeometryCollection geomColl);

            // ACT
            byte[] bytes = WkbConverter.ToBinary(geomColl, Endianness.BIG);


            Geometry geo = WkbConverter.FromBinary(bytes);


            GeometryCollection geoCollection = Assert.IsType<GeometryCollection>(geo);
            Point pointReconstituted = Assert.IsType<Point>(geoCollection.Geometries.ElementAt(0));
            LineString lineStringReconstituted = Assert.IsType<LineString>(geoCollection.Geometries.ElementAt(1));
            Polygon polygonReconstituted = Assert.IsType<Polygon>(geoCollection.Geometries.ElementAt(2));


            // ASSERT

            Assert.True(lineStringReconstituted.Equals(linearRing), "Reconstituted LinearRing is not equivalent to the original one");
            Assert.True(polygonReconstituted.Equals(polygon), "Reconstituted Polygon is not equivalent to the original one");
            Assert.True(pointReconstituted.Equals(point), "Reconstituted Point is not equivalent to the original one");
            Assert.Equal(40, point.Coordinates.Longitude);
            Assert.Equal(10, point.Coordinates.Latitude);
        }

        #endregion Tests
    }
}