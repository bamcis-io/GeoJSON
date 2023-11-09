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
        private Polygon FetchDefaultPolygon()
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
        private Polygon FetchDefaultRectangle()
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
