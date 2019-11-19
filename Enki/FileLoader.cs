using System.IO.Compression;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using Enki.Models;
using System.Drawing;
using UnityEngine;

namespace Enki{

	public static class FileLoader{
		public static ModData LoadModData(string file) {

			string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file.Replace("./", ""));

			//Gets the Case Sensitive Filename By Searching through the Directory
			string RootName = Path.GetFileNameWithoutExtension(Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)).FirstOrDefault());

			ModData data = new ModData(LoadZipFile(path), RootName);

			Glob[] patterns = Array.ConvertAll(data.config.Include, v => new Glob(v, data.rootDir));

			foreach(Glob g in patterns){
				foreach (var ZippedFile in data.File.Entries){
					if (g.Match(ZippedFile.FullName))
						data.LoadPathFile(ZippedFile.FullName);
				}
			}

			return data;
		}

		public static ZipArchive LoadZipFile(string path) {
			if(!System.IO.File.Exists(path)) return null;
			if(!path.EndsWith(".zip")) return null;

			ZipArchive loadedZip = new ZipArchive(new FileStream(path, FileMode.Open), ZipArchiveMode.Read);
			return loadedZip;
		}

		public static File UnzipFile(ZipArchive mod, Glob path){
			if (mod == null) return null;
			try{
				ZipArchiveEntry loaded = mod.Entries.SingleOrDefault((ZipArchiveEntry e) => path.Match(e.FullName));
				if(loaded == null) return null;
				File f = new File(loaded.FullName, loaded.Open());
				return f;
			}
			catch (Exception e) {
				throw new System.Exception("Multiple Files Matching that Path found");
			}
			return null;
		}

		public static File UnzipConfig(ZipArchive mod)  {
			return UnzipFile(mod, "Config.xml");
		}
	}
}
