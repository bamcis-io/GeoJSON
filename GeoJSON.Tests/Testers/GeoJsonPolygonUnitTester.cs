using BAMCIS.GeoJSON;
using GeoJSON.Tests.Tests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonPolygonUnitTester : GeoJsonBaseUnitTester
    {
        #region Helper Methods
        private static Polygon FetchDefaultPolygon()
        {
            var coordinates = new List<Coordinate> { new Coordinate(40, 40) ,
                                                    new Coordinate(20,45) ,
                                                    new Coordinate(45, 30) ,
                                                    new Coordinate(40,40)
                                                    };

            var linearRing = new LinearRing(coordinates);

            var polygon = new Polygon(linearRing);

            return polygon;
        }

        /// <summary>
        /// This method generates a regular rectangle (therefore, a polygon) 
        /// ranging from the coordinate 1,1 (lower left corner) up to the coordinate 4,4 (Upper right corner)
        /// </summary>
        /// <returns></returns>
        private static Polygon FetchDefaultRectangle()
        {
            var coordinates = new List<Coordinate> { new Coordinate(1, 1) ,
                                                     new Coordinate(1, 4) ,
                                                     new Coordinate(4, 4) ,
                                                     new Coordinate(4, 1) ,
                                                     new Coordinate(1, 1)
                                                    };

            var linearRing = new LinearRing(coordinates);

            var polygon = new Polygon(linearRing);

            return polygon;
        }


        private static Polygon FetchIrregularPolygon()
        {
            var coordinates = new List<Coordinate> { new Coordinate(1, 1) ,
                                                     new Coordinate(1, 4) ,
                                                     new Coordinate(4, 4) ,
                                                     new Coordinate(6, 4) ,
                                                     new Coordinate(5, -2) ,
                                                     new Coordinate(3.5, 2.5) ,
                                                     new Coordinate(3.25, 2.4) ,
                                                     new Coordinate(2, -2),
                                                     new Coordinate(1, 1)
                                                    };

            var linearRing = new LinearRing(coordinates);

            var polygon = new Polygon(linearRing);

            return polygon;
        }

        #endregion Helper Methods

        #region Test Methods

        [Fact]
        public void SerializePolygon()
        {
            // ARRANGE

            var polygon = FetchDefaultPolygon();
            string polygonContent = JsonConvert.SerializeObject(polygon);
            WriteJsonFile("ReferenceFiles/Polygon.json", polygonContent);

            string content = ReadJsonFile("ReferenceFiles/Polygon.json");

            // ACT
            var PolygonReconstituted = JsonConvert.DeserializeObject<Polygon>(content);
            

            // ASSERT
            Assert.True(polygon.Equals(PolygonReconstituted));
        }

        [Fact]
        public void PolygonContainsPoint()
        {

            // ARRANGE

            var regularPolygon = FetchDefaultRectangle();

            var irregularPolygon = FetchIrregularPolygon();

            var point = new Point(2, 2);

            var point2 = new Point(5, 2);

            var point3 = new Point(3, -2.5);     // this point must be outside of the irregularpolygon, and also outside of its bounding box

            var point4 = new Point(3.5, 0.5);      // this point must be outside of the irregularpolygon, though inside of its bounding box

            var point5 = new Point(3.5, 2.5);    // this points must touch the irregularPolygon, not be inside of the polygon, nor touch its bounding box

            // ASSERT

            #region Regular Polygon Validations

            Assert.True(regularPolygon.Contains(point));
            Assert.False(regularPolygon.Touches(point));

            Assert.False(regularPolygon.Contains(point2));
            Assert.False(regularPolygon.Touches(point2));

            #endregion Regular Polygon Validations

            #region Irregular Polygon Validations
            Assert.True(!irregularPolygon.Contains(point3) &&
                        !irregularPolygon.BoundingBox.Contains(point3) &&
                        !irregularPolygon.Intersects(point3) &&
                        !irregularPolygon.BoundingBox.Touches(point3));

            Assert.True(!irregularPolygon.Contains(point4));

            Assert.True(irregularPolygon.BoundingBox.Contains(point4));
            Assert.False(irregularPolygon.Intersects(point4));
            Assert.False(irregularPolygon.BoundingBox.Touches(point4));



            Assert.False(irregularPolygon.Contains(point5));
            
            Assert.True(irregularPolygon.BoundingBox.Contains(point5));

            Assert.True(irregularPolygon.Touches(point5));

            Assert.False(irregularPolygon.BoundingBox.Touches(point5));

            #endregion Irregular Polygon Validations

        }

        [Fact]
        public void PolygonIntersectsPoint()
        {

            // ARRANGE

            var polygon = FetchDefaultRectangle();

            var point = new Point(1, 1);


            // ASSERT
            Assert.True(polygon.Intersects(point));

        }



        [Fact]
        public void PolygonContainsLineSegment()
        {

            // ARRANGE

            var polygon = FetchDefaultRectangle();

            var point = new Point(2, 2);

            var point2 = new Point(5, 2);

            // ASSERT
            Assert.True(polygon.Contains(point));
            Assert.False(polygon.Touches(point));

            Assert.False(polygon.Contains(point2));
            Assert.False(polygon.Touches(point2));

        }

        [Fact]
        public void IntersectsLineSegment()
        {

            // ARRANGE

            var polygon = FetchDefaultRectangle();

            var point = new Point(1, 1);

            var lineSegment = new LineSegment(point, new Point(5, 5));


            // ASSERT
            Assert.True(polygon.Intersects(lineSegment));

        }

        #endregion Test Methods

    }
}
