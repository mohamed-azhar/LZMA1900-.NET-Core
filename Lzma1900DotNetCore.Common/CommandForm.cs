namespace Lzma1900DotNetCore.Common
{
	public class CommandForm
	{
		public CommandForm(string idString, bool postStringMode)
		{
			IDString = idString;
			PostStringMode = postStringMode;
		}

		public string IDString { get; set; } = string.Empty;
		public bool PostStringMode { get; set; } = false;
	}
}
