using HamlWatchtowerApp;
using System;
using System.IO;

namespace MarkupWatchtower.com.main
{
    public class LangListGen
    {
        private static string path = Environment.CurrentDirectory + "\\markupList.json";
        private string text =
@"{
    ""version"": """+WatcherWindow.VERSION+@""",
	""markup"": {
		""HAML"": {
			""input"": "".haml"",
			""output"": "".html"",
			""command"": ""haml !input! > !output!""
		},
		""SASS"": {
			""input"": ["".scss"", "".sass""],
			""output"": "".css"",
			""command"": ""sass !input! --style expanded > !output!""
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
