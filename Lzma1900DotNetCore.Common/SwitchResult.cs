using System.Collections;

namespace Lzma1900DotNetCore.Common
{
    public class SwitchResult
    {
        public bool ThereIs { get; set; } = false;
        public bool WithMinus { get; set; }
        public int PostCharIndex { get; set; }
        public ArrayList PostStrings { get; set; } = new ArrayList();
    }
}
