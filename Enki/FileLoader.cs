using System.IO.Compression;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using Enki.Models;
using System.Drawing;
using UnityEngine;

namespace Enki{

	public class File{
		public enum Type {
			Undefined,
			Config,
			Script,
			Libary,
			Model,
			Texture
		};

		Dictionary<string, Type> extentions = new Dictionary<string, Type>() {
			{".cs", Type.Script},
			{".dll", Type.Libary },
			{".xml", Type.Config },
			{".fbx", Type.Model },
			{".obj", Type.Model },
			{".jpg", Type.Texture },
			{".png", Type.Texture },
			{".bmp", Type.Texture }
		};

		public Type FileType = Type.Undefined;
		public string Name;
		public string Path;
		public byte[] Data;

		public File(string path, Stream data) {
			Name = System.IO.Path.GetFileName(path);
			Path = path;
			StreamData = data;

			string ext = System.IO.Path.GetExtension(path).ToLower();
			FileType = extentions[ext];
		}

		public string StringData {
			get {
				return Encoding.Default.GetString(Data);
			}
			set {
				Data = Encoding.Default.GetBytes(value);
			}
		}

		public Stream StreamData {
			get {
				return new MemoryStream(Data);
			}
			set {
				var ms = new MemoryStream();
				value.CopyTo(ms);
				ms.Seek(0, SeekOrigin.Begin);
				Data = ms.ToArray();
			}
		}

		public Assimp.Mesh[] MeshData {
			get {
				return ModelLoader.LoadModels(this);
			}
		}

		public Assimp.Scene SceneData {
			get {
				return ModelLoader.LoadScene(this);
			}
		}

		public Image ImageData {
			get {
				Image img = Image.FromStream(StreamData);
				return img;
			}
		}

		public Texture2D TextureData {
			get {
				Image imgData = ImageData;
				Texture2D texture = new Texture2D(imgData.Width, imgData.Height);
				if (ImageConversion.LoadImage(texture, Data))
					return texture;
				else
					return null; 
			}
		}
	}

	public static class FileLoader{
		public static ModData LoadMod(string file) {
			string path = Path.GetFullPath(file);
			string fileName = Path.GetFileName(path);
			 ModData d = new ModData();

			ZipArchive ModFile = LoadZipFile(path);
			if (ModFile == null)return null;

			//Gets the Case Sensitive Filename By Searching through the Directory
			string RootName = Path.GetFileNameWithoutExtension(Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)).FirstOrDefault());
			d.rootDir = RootName + "/";

			d.file = ModFile;

			ModConfig config = d.LoadConfig();

			Glob[] patterns = Array.ConvertAll(config.Include, v => new Glob(v, d.rootDir));

			foreach(Glob g in patterns){
				foreach (var ZippedFile in d.file.Entries){
					if (g.Match(ZippedFile.FullName))
						d.LoadPathFile(ZippedFile.FullName);
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
