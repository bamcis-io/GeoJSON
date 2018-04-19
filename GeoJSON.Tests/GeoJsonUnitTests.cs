using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace GeoJSON.Tests
{
    public class GeoJsonUnitTests
    {
        [Fact]
        public void MultiLineStringTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("multilinestring.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiLineString Geo = JsonConvert.DeserializeObject<MultiLineString>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void LineStringTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("linestring.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            LineString Geo = JsonConvert.DeserializeObject<LineString>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void MultiPolygonTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("multipolygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiPolygon Geo = JsonConvert.DeserializeObject<MultiPolygon>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void FeatureTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature Geo = JsonConvert.DeserializeObject<Feature>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void FeatureCollectionTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("featurecollection.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            FeatureCollection Geo = JsonConvert.DeserializeObject<FeatureCollection>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void PolygonTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("polygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon Geo = JsonConvert.DeserializeObject<Polygon>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void PointTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("point.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Point Geo = JsonConvert.DeserializeObject<Point>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void MultiPointTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("multipoint.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiPoint Geo = JsonConvert.DeserializeObject<MultiPoint>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void GeometryCollectionTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("geometrycollection.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeometryCollection Geo = JsonConvert.DeserializeObject<GeometryCollection>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void PositionTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("position.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Position Geo = JsonConvert.DeserializeObject<Position>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }

        [Fact]
        public void GeoJsonFeatureTest()
        {
            // ARRANGE
            string Content = File.ReadAllText("feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson Geo = JsonConvert.DeserializeObject<GeoJson>(Content);
            string Content2 = JsonConvert.SerializeObject(Geo);

            // ASSERT
            Assert.Equal(Content, Content2, true, true, true);
        }
    }
}
