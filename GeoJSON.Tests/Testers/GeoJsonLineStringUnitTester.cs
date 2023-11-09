using BAMCIS.GeoJSON;
using GeoJSON.Tests.Tests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonLineStringUnitTester : GeoJsonBaseUnitTester
    {

        private LineString FetchDefaultLineString()
        {
            var p1 = new Point(1, 1);

            var p2 = new Point(2, 5);

            var p3 = new Point(5, 5);

            var p4 = new Point(8, 5);

            var lineSegment = new LineSegment(p1, p2);

            var lineSegment2 = new LineSegment(p2, p3);

            var lineSegment3 = new LineSegment(p3, p4);

            var lineString = new LineString(new List<LineSegment> { lineSegment, lineSegment2, lineSegment3 });

            return lineString;
        }

        [Fact]
        public void LineStringTouchesPoint()
        {
            // ARRANGE
            var lineString = FetchDefaultLineString();

            var p1 = new Point(2, 5);
            var p2 = new Point(6, 5);
            // ACT


            // ASSERT
            Assert.False(lineString.Equals(p1));
            Assert.True(lineString.Touches(p1));
            Assert.False(lineString.Intersects(p1));
            Assert.True(lineString.Touches(p2));
            Assert.False(lineString.Intersects(p1));
        }

        [Fact]
        public void LineStringEqualityEvaluation()
        {
            // ARRANGE
            var lineString = FetchDefaultLineString();
            var lineString2 = FetchDefaultLineString();

            // ASSERT
            Assert.True(lineString.Equals(lineString2));
        }

    }
}
