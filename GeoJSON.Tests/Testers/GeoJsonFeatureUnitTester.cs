using BAMCIS.GeoJSON;
using GeoJSON.Tests.Tests;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonFeatureUnitTester : GeoJsonBaseUnitTester
    {

        [Fact]
        public void FeatureTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureIdNotEqualTest()
        {
            // ARRANGE
            string content1 = File.ReadAllText("ReferenceFiles/feature_id_number.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");
            string content2 = File.ReadAllText("ReferenceFiles/feature_id_num_as_string.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geoWithNumId = JsonConvert.DeserializeObject<Feature>(content1);
            Feature geoWithStringId = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.Equal(geoWithNumId.Id.Value, geoWithStringId.Id.Value);
            Assert.NotEqual(geoWithNumId.Id, geoWithStringId.Id);
        }

        [Fact]
        public void FeatureIdEqualTest()
        {
            Debug.WriteLine(GetCurrentWorkingDirectory());
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature_id_number.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo1 = JsonConvert.DeserializeObject<Feature>(content);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content);

            // ASSERT
            Assert.Equal(geo1.Id.Value, geo2.Id.Value);
            Assert.Equal(geo1.Id, geo2.Id);
        }

        [Fact]
        public void FeatureTestStringId()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature_id_string.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestNumberId()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature_id_number.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

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
            GeoJsonConfig.EnforcePositionValidation();
            string content = File.ReadAllText("ReferenceFiles/feature_out_of_range.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT & ASSERT
            Assert.Throws<ArgumentOutOfRangeException>(() => JsonConvert.DeserializeObject<Feature>(content));
        }

        [Fact]
        public void FeatureOutOfRangeTestIgnoreValidation()
        {
            // ARRANGE
            GeoJsonConfig.IgnorePositionValidation();
            string content = File.ReadAllText("ReferenceFiles/feature_out_of_range.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

        [Fact]
        public void FeatureTestNullGeometry()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/feature_null_geometry.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            Feature geo = JsonConvert.DeserializeObject<Feature>(content);
            string content2 = JsonConvert.SerializeObject(geo);
            Feature geo2 = JsonConvert.DeserializeObject<Feature>(content2);

            // ASSERT
            Assert.True(geo.Equals(geo2));
        }

    }
}
