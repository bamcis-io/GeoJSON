using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Tests
{
    public class GeoJsonUnitTests : GeoJsonMultiLineUnitTester
    {
        
        [Fact]
        public void PolygonTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/polygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Polygon geo2 = JsonConvert.DeserializeObject<Polygon>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonWithHoleTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/polygonwithhole.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Polygon geo2 = JsonConvert.DeserializeObject<Polygon>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonRemoveInnerRingsTestWithHole()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/polygonwithhole.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            geo.RemoveInteriorRings();

            // ASSERT
            Assert.Single(geo.LinearRings);


            Assert.True(true);

            
            
        }

        [Fact]
        public void PolygonRemoveInnerRingsTestWithoutHole()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/polygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            geo.RemoveInteriorRings();


            // ASSERT
            Assert.Single(geo.LinearRings);
        }

        [Fact]
        public void PointTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/point.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Point geo = JsonConvert.DeserializeObject<Point>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Point geo2 = JsonConvert.DeserializeObject<Point>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void MultiPointTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/multipoint.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            var geo = MultiPoint.FromJson(content);
            // ACT
            // MultiPoint geo = JsonConvert.DeserializeObject<MultiPoint>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            MultiPoint geo2 = JsonConvert.DeserializeObject<MultiPoint>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeometryCollectionTest()
        {
            // ARRANGE

            var multiLineString = FetchDefaultGeometry();

            var geometryCollection = new GeometryCollection(multiLineString.ToList());

            var content = geometryCollection.ToJson();

            WriteJsonFile("ReferenceFiles/geometrycollection.json", content);

            content = ReadJsonFile("ReferenceFiles/geometrycollection.json");

            // ACT
            GeometryCollection geo = JsonConvert.DeserializeObject<GeometryCollection>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            GeometryCollection geo2 = JsonConvert.DeserializeObject<GeometryCollection>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PositionTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/position.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Coordinate geo = JsonConvert.DeserializeObject<Coordinate>(content);
            string content2 = JsonConvert.SerializeObject(geo);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

        [Fact]
        public void GeoJsonFeatureTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson geo = GeoJson.FromJson(content);
            string content2 = geo.ToJson();
            GeoJson geo2 = GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonFeatureTestWithBbox()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/featurebbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson geo = GeoJson.FromJson(content);
            string content2 = geo.ToJson();
            GeoJson geo2 = GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJson3DLineStringTestWithBbox()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/3dlinestringbbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson geo = GeoJson.FromJson(content);
            string content2 = geo.ToJson();
            GeoJson geo2 = GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeoJsonFeatureCollectionTestWithBbox()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/featurecollectionbbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson geo = GeoJson.FromJson(content);
            string content2 = geo.ToJson();
            GeoJson geo2 = GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }
    }
}
