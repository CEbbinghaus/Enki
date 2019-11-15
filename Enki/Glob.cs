using System.Text.RegularExpressions;
using System.IO;

namespace Enki
{
	public class Glob
	{
		static string BaseDir = Directory.GetCurrentDirectory().Replace("\\", "/");
		Regex regex;

		public Glob(string pattern){
			string Escaped = Regex.Escape(pattern);
			Escaped = Escaped.Replace(@"\./", string.Format("({0}|)", BaseDir));
			Escaped = Escaped.Replace(@"\*\*/", @"(.+\/)+");
			Escaped = Escaped.Replace(@"\*", @".*");
			Escaped = Escaped.Replace(@"\?", @".");
			regex = new Regex(Escaped);
		}

		public bool Match(string s){
			return regex.IsMatch(s);
		}
	}
}
