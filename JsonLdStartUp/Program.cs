using System;
using System.IO;
using JsonLD.Core;
using JsonLD.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonLdStartUp
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = GetJson("TestCases\\event-input.jsonld");
            var expected = File.ReadAllText("TestCases\\event-output.nq").Replace("\\", "");

            var options = new JsonLdOptions("http://json-ld.org/test-suite/tests/startup");
            options.format = "application/nquads";
            var result = new JValue(((string)JsonLdProcessor.ToRDF(input, options)).Replace("\n", "\r\n"));
            Console.WriteLine(JSONUtils.ToPrettyString(result));
            Console.WriteLine(JSONUtils.ToPrettyString(result) == JSONUtils.ToPrettyString(expected));
            bool isSame = JsonLdUtils.DeepCompare(result, expected);
            Console.WriteLine(JsonLdUtils.DeepCompare(result, expected));
        }

         static JToken GetJson(JToken j)
        {
            try
            {
                if (j.Type == JTokenType.Null) return null;
                using (Stream manifestStream = File.OpenRead((string)j))
                using (TextReader reader = new StreamReader(manifestStream))
                using (JsonReader jreader = new Newtonsoft.Json.JsonTextReader(reader)
                {
                    //Date formatted strings are not parsed to a date type and are read as strings.
                    DateParseHandling = DateParseHandling.None
                })
                {
                    return JToken.ReadFrom(jreader);
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
