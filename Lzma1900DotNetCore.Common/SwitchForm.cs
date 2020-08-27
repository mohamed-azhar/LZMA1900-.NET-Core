using Lzma1900DotNetCore.Common.Enums;

namespace Lzma1900DotNetCore.Common
{
	public class SwitchForm
	{
		public string IDString { get; set; }
		public SwitchType Type { get; set; }
		public bool Multi { get; set; }
		public int MinLen { get; set; }
		public int MaxLen { get; set; }
		public string PostCharSet { get; set; }
		

		public SwitchForm(string idString, SwitchType type, bool multi, int minLen, int maxLen, string postCharSet)
		{
			IDString = idString;
			Type = type;
			Multi = multi;
			MinLen = minLen;
			MaxLen = maxLen;
			PostCharSet = postCharSet;
		}
		public SwitchForm(string idString, SwitchType type, bool multi, int minLen) : this(idString, type, multi, minLen, 0, "")
		{
		}
		public SwitchForm(string idString, SwitchType type, bool multi) : this(idString, type, multi, 0)
		{
		}
	}
}
