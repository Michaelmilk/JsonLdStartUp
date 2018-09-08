using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            var expected = File.ReadAllText("TestCases\\event-output.nq");

            var options = new JsonLdOptions("http://json-ld.org/test-suite/tests/startup");
            options.format = "application/nquads";
            var result = new JValue((string)JsonLdProcessor.ToRDF(input, options));
            Console.Write(JSONUtils.ToPrettyString(result));
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
