using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacWallpaper
{
    public class Emoji
    {
        public string cldr { get; set; }
        public string fromVersion { get; set; }
        public string glyph { get; set; }
        public string group { get; set; }
        public string[] keywords { get; set; }
        public string[] mappedToEmoticons { get; set; }
        public string tts { get; set; }
        public string unicode { get; set; }
    }

}
