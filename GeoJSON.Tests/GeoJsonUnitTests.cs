using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System;
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
            string content = File.ReadAllText("multilinestring.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiLineString geo = MultiLineString.FromJson(content);

            string content2 = geo.ToJson();

            MultiLineString geo2 = MultiLineString.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void LineStringTest()
        {
            // ARRANGE
            string content = File.ReadAllText("linestring.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            LineString geo = JsonConvert.DeserializeObject<LineString>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            LineString geo2 = JsonConvert.DeserializeObject<LineString>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void MultiPolygonTest()
        {
            // ARRANGE
            string content = File.ReadAllText("multipolygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiPolygon geo = JsonConvert.DeserializeObject<MultiPolygon>(content);
            string content2 = JsonConvert.SerializeObject(geo);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

        [Fact]
        public void FeatureTest()
        {
            // ARRANGE
            string content = File.ReadAllText("feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureOutOfRangeTest()
        {
            // ARRANGE
            string content = File.ReadAllText("feature_out_of_range.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT & ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => JsonConvert.DeserializeObject<Feature>(content));         
        }

        [Fact]
        public void FeatureTestNullGeometry()
        {
            // ARRANGE
            string content = File.ReadAllText("feature_null_geometry.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureCollectionTest()
        {
            // ARRANGE
            string content = File.ReadAllText("featurecollection.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            FeatureCollection geo = JsonConvert.DeserializeObject<FeatureCollection>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            FeatureCollection geo2 = JsonConvert.DeserializeObject<FeatureCollection>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void PolygonTest()
        {
            // ARRANGE
            string content = File.ReadAllText("polygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("polygonwithhole.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("polygonwithhole.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            bool result = geo.RemoveInteriorRings();


            // ASSERT
            Assert.True(result);
            Assert.Single(geo.Coordinates);
        }

        [Fact]
        public void PolygonRemoveInnerRingsTestWithoutHole()
        {
            // ARRANGE
            string content = File.ReadAllText("polygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Polygon geo = JsonConvert.DeserializeObject<Polygon>(content);
            bool result = geo.RemoveInteriorRings();


            // ASSERT
            Assert.False(result);
            Assert.Single(geo.Coordinates);
        }

        [Fact]
        public void PointTest()
        {
            // ARRANGE
            string content = File.ReadAllText("point.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("multipoint.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiPoint geo = JsonConvert.DeserializeObject<MultiPoint>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            MultiPoint geo2 = JsonConvert.DeserializeObject<MultiPoint>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void GeometryCollectionTest()
        {
            // ARRANGE
            string content = File.ReadAllText("geometrycollection.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("position.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Position geo = JsonConvert.DeserializeObject<Position>(content);
            string content2 = JsonConvert.SerializeObject(geo);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

        [Fact]
        public void GeoJsonFeatureTest()
        {
            // ARRANGE
            string content = File.ReadAllText("feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("featurebbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("3dlinestringbbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            string content = File.ReadAllText("featurecollectionbbox.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            GeoJson geo = GeoJson.FromJson(content);
            string content2 = geo.ToJson();
            GeoJson geo2 = GeoJson.FromJson(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }
    }
}
