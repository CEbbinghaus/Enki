using System.Text.RegularExpressions;
using System.IO;

namespace Enki
{
	public class Glob
	{
		static string BaseDir = Directory.GetCurrentDirectory().Replace("\\", "/");
		Regex regex;

		public static implicit operator Glob(string s) {
			return new Glob(s);
		}

		public Glob(string pattern, string RootDir = null){
			string Escaped = Regex.Escape(pattern);
			Escaped = Escaped.Replace(@"\./", string.Format("({0}|)", RootDir != null ? RootDir : BaseDir));
			Escaped = Escaped.Replace(@"\*\*/", @"(.+\/)+");
			Escaped = Escaped.Replace(@"\*", @".*");
			Escaped = Escaped.Replace(@"\?", @".");
			regex = new Regex("^" + Escaped + "$", RegexOptions.Singleline);
		}

		public bool Match(string s){
			return regex.IsMatch(s);
		}
	}
}
