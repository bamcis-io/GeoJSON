using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace GeoJSON.Tests.Tests
{
    public class GeoJsonBaseUnitTester: WkbBaseUnitTester
    {

        /// <summary>
        /// General method for writing geometryContent into a file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="geometryContent"></param>
        protected static void WriteJsonFile(string filename, string geometryContent )
        {
            var filepath = Path.Join(GetCurrentWorkingDirectory(), filename);
            Debug.WriteLine($"Saving content into {filepath}");

            File.WriteAllText(filepath, geometryContent);
        }

        protected static string ReadJsonFile(string filename)
        {
            var filepath = Path.Join(GetCurrentWorkingDirectory(), filename);
            Debug.WriteLine($"Reading content from {filepath}");

            return File.ReadAllText(filepath);
        }

        protected static string GetCurrentWorkingDirectory()
        {
            var cwddirname = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var ToDirname = cwddirname.Split("bin")[0];

            return ToDirname;


        }
    }
}