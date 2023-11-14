using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace GeoJSON.Tests.Tests
{
    public class GeoJsonMultiLineUnitTester : GeoJsonBaseUnitTester
    {
        protected MultiLineString FetchDefaultGeometry()
        {
            var line1 = new LineString(new List<Point>{ new Point(0,0), new Point(0,1) });

            var line2 = new LineString(new List<Point> { new Point(1, 0), new Point(1, 1) });


            return new MultiLineString(new List<LineString>{ line1, line2 });
        }

        [Fact]
        public void MultiLineStringTest()
        {
            // ARRANGE
            var multiLineString = FetchDefaultGeometry();
            var content = multiLineString.ToJson();

            WriteJsonFile("ReferenceFiles/multilinestring.json", content);

            content = ReadJsonFile("ReferenceFiles/multilinestring.json");

            // ACT
            MultiLineString geo = MultiLineString.FromJson(content);

            string content2 = geo.ToJson();

            MultiLineString geo2 = MultiLineString.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }


        [Fact]
        public void MultiLineStringWritingTest()
        {
            // ACT

            LineString lineString1 = GeometriesDefaultGenerator.GenerateDefaultLineStringForUnitTests();

            LineString lineString2 = GeometriesDefaultGenerator.GenerateDefaultLineStringForUnitTests(3, 5, 2);


            var multiLineString = new MultiLineString(new List<LineString> { lineString1, lineString2 });


            string content2 = JsonConvert.SerializeObject(multiLineString, Formatting.Indented);

            string ToDirname = GetCurrentWorkingDirectory();

            File.WriteAllText(Path.Combine(ToDirname, "multilinestring.json"), content2);

            // File.Delete(ToDirname, "multilinestring.json"));

        }
    }
}