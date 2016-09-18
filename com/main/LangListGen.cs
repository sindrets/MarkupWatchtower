using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupWatchtower.com.main
{
    public class LangListGen
    {
        private static string path = Environment.CurrentDirectory + "\\markupList.json";
        private string text =
@"{
	""markup"": {
		""HAML"": {
			""input"": "".haml"",
			""output"": "".html"",
			""command"": ""haml !input! > !output!""
		},
		""SASS"": {
			""input"": ["".scss"", "".sass""],
			""output"": "".css"",
			""command"": ""sass !input! > !output!""
		},
		""Jade"": {
			""input"": "".jade"",
			""output"": "".html"",
			""command"": ""jade --pretty < !input! > !output!""
		},
		""Pug"": {
			""input"": "".pug"",
			""output"": "".html"",
			""command"": ""pug --pretty !input! !output!""
		},
		""Coffee-Script"": {
			""input"": "".coffee"",
			""output"": "".js"",
			""command"": ""coffee --compile !input!""
		},
		""Babel"": {
			""input"": "".babel"",
			""output"": "".js"",
			""command"": ""babel !input! --out-file !output!""
		} 
	}
}";

        public LangListGen()
        {
            File.WriteAllText(path, text);
        }
    }
}
