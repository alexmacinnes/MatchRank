using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MatchRank.Test
{
    internal class ResourceHelper
    {
        public static string ReadEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string fullName = "MatchRank.Test." + name;
            using var stream = assembly.GetManifestResourceStream(fullName);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
