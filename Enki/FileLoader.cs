using System.IO.Compression;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Linq;
using System.Text;

namespace Enki{

	public class File{
		public enum Type {
			Undefined,
			Script,
			Model,
			Texture
		};

		public Type type = Type.Undefined;
		public string name;
		public string path;
		public byte[] data;

		public string StringData {
			get {
				return Encoding.Default.GetString(data);
			}
			set {
				data = Encoding.Default.GetBytes(value);
			}
		}

		public Stream StreamData {
			get {
				return new MemoryStream(data);
			}
			set {
				var ms = new MemoryStream();
				value.CopyTo(ms);
				ms.Seek(0, SeekOrigin.Begin);
				data = ms.ToArray();
			}
		}

	}

	public static class FileLoader{

		public static ModData LoadMod(string path) {
			ModData d = new ModData();

			ZipArchive ModFile = LoadZipFile(path);
			if (ModFile == null) throw new System.Exception("Cannot find ZIP Archive");

			d.file = ModFile;

			ModConfig config = d.LoadConfig();

			Glob[] patterns = Array.ConvertAll(config.Include, v => new Glob(v));

			foreach (var ZippedFile in d.file.Entries){
				foreach(Glob g in patterns){
					if (g.Match(ZippedFile.FullName))
						d.LoadFile(ZippedFile.FullName);
				}
			}

			return d;
		}

		public static ZipArchive LoadZipFile(string path) {
			if(!System.IO.File.Exists(path)) return null;
			if(!path.EndsWith(".zip")) return null;
			ZipArchive loadedZip = new ZipArchive(new FileStream(path, FileMode.Open), ZipArchiveMode.Read);
			return loadedZip;
		}

		public static File UnzipFile(ZipArchive mod, string file){
			if (mod == null) return null;
			ZipArchiveEntry loaded = mod.GetEntry(file);
			File f = new File();
			f.name = loaded.Name;
			f.path = loaded.FullName;
			f.StreamData = loaded.Open();
			return f;
		}

		public static File UnzipConfig(ZipArchive mod)  {
			return UnzipFile(mod, "Config.xml");
		}
	}
}
