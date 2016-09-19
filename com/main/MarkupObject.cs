
using System.Collections.Generic;

namespace MarkupWatchtower.com.main
{
    public class MarkupObject
    {
        public string Name { get; set; }
        public List<string> Input = new List<string>();
        public string Output { get; set; }
        public string Subdirectories { get; set; }
        public string Command { get; set; }
    }
}
