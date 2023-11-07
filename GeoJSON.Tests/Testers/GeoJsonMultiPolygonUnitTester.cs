using BAMCIS.GeoJSON;
using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace GeoJSON.Tests.Testers
{
    public class GeoJsonMultiPolygonUnitTester
    {

        [Fact]
        public void MultiPolygonTest()
        {
            // ARRANGE
            string content = File.ReadAllText("ReferenceFiles/multipolygon.json").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", "");

            // ACT
            MultiPolygon geo = JsonConvert.DeserializeObject<MultiPolygon>(content);
            string content2 = JsonConvert.SerializeObject(geo);

            // ASSERT
            Assert.Equal(content, content2, true, true, true);
        }

    }
}
