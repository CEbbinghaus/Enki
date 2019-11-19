using System.IO.Compression;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Reflection;
using Enki.Extensions;

namespace Enki {
	[System.Serializable]
	public class ModConfig {
		public string Name = "_";
		public string Author = "_";
		public string Version = "_";
		public string Entry = "_";

		[XmlArrayItem("Path")]
		public string[] Include = new string[] { "_", "_" };

		public static ModConfig Deserialize(File data) {
			if (data == null) return null;
			XmlSerializer deserializer = new XmlSerializer(typeof(ModConfig));
			ModConfig LoadedConfig = (ModConfig)deserializer.Deserialize(data.StreamData);
			return LoadedConfig;
		}

		public override int GetHashCode() {
			return string.Format("{0}.{1}.{2}", Author, Name, Version).GetHashCode();
		}
	}

	public class Lookup<T> {

		List<T> items;
		string key;

		internal Lookup(List<T> f, string key) {
			this.key = key;
			var field = typeof(T).GetField(key);
			if (field == null || field.FieldType != typeof(string))
				throw new System.Exception("Lookup Type must Specify The Key " + key);
			items = f;
		}

		public T this[string name]{
			get {
				foreach (T item in items) {
					if (item.GetProperty<string>(key) == name) return item;
				}
				return default(T);
			}
		}
	}

	public class ModData {
		public string Name {
			get { return config.Name; }
		}
		internal string rootDir;
		public ZipArchive File;
		public ModConfig config;
		private List<File> LoadedFiles = new List<File>();
		public Lookup<File> files;
		public List<Models.Model> LoadedModels = new List<Models.Model>();
		public bool _enabled = true;
		public Mod mod;

		internal ModData(ZipArchive file, string name) {
			rootDir = name + "/";
			this.File = file;
			files = new Lookup<File>(LoadedFiles, "Name");

			LoadConfig();


			Glob[] patterns = Array.ConvertAll(config.Include, v => new Glob(v, rootDir));

			foreach (Glob g in patterns)
			{
				foreach (var ZippedFile in File.Entries)
				{
					if (g.Match(ZippedFile.FullName)) {
						File f = LoadPathFile(ZippedFile.FullName);
						f.HandleData();
					}
				}
			}

			LoadMod();
			
			//Registers itself with the Mod Loaders
			ModLoader.Mods.Add(mod);
		}


		public void LoadConfig() {

			if (File == null) throw new System.Exception("The Mod is Missing its Corresponding File.");

			File UnzippedConfig = LoadFile("Config.xml");

			if (UnzippedConfig == null) throw new System.Exception("Cannot Find Config File.");

			config = ModConfig.Deserialize(UnzippedConfig);
			
		}

		public File LoadFile(string path) {

			string finalPath = rootDir + path;

			//Search Through Loaded files and Check if File has been Already Loaded
			int index = LoadedFiles.FindIndex(v => v.Path == finalPath);
			if (index != -1) return LoadedFiles[index];
			
			//Load file, Cache Loaded Instance, Return Result
			File f = FileLoader.UnzipFile(File, finalPath);
			if (f == null)return null;
			LoadedFiles.Add(f);
			return f;
		}

		public File LoadPathFile(string path) {

			//Search Through Loaded files and Check if File has been Already Loaded
			int index = LoadedFiles.FindIndex(v => v.Path == path);
			if (index != -1) return LoadedFiles[index];
			
			//Load file, Cache Loaded Instance, Return Result
			File f = FileLoader.UnzipFile(File, path);
			if (f == null)return null;
			LoadedFiles.Add(f);
			return f;
		}

		public Mod LoadMod() {
			File entry = LoadFile(config.Entry);
			if (entry == null) throw new System.Exception("No Entry could be Found. Please ensure the File exists");
			if (entry.FileType != Enki.File.Type.Libary && entry.FileType != Enki.File.Type.Script) throw new System.Exception("Entry must be a .cs or .dll file");

			Assembly file = (Assembly)entry.HandleData();
			if (file == null) throw new System.Exception("Assembly could not be Loaded");
			mod = (Mod)file.CreateInstance(Path.GetFileNameWithoutExtension(config.Entry));
			if (mod == null) throw new System.Exception("Could not Load Mod. Ensure the Class is in the Global scope and named the same as the File");

			mod.BeforeLoad();

			return mod;
		}
	}

    public class Mod{

		internal ModData data = null;

		public override int GetHashCode(){
			if (data == null) return -1;
			return data.GetHashCode();
		}

		/// <summary>
		/// Wether the Mod is Enabled and will be Updated
		/// </summary>
		public bool Enabled{
			get { return data._enabled; }
			set {
				if (data._enabled)
					if (!value)
					{
						BeforeUnload();
						OnUnload();
						data._enabled = value;
					}
					else
					if (value)
					{
						BeforeLoad();
						OnLoad();
						data._enabled = value;
					}
			}
		}

		public string Name {
			get {
				return data.Name;
			}
		}


		/// <summary>
		/// Gets Called Immediately Before Any mod is Loaded. NOT GUARANTEED TO HAVE INITIALIZED DATA
		/// </summary>
		public virtual void BeforeLoad() { }

		/// <summary>
		/// Gets Called When all Mods are Loaded. Use this Call to Access any Data or Initialize any Variables.
		/// </summary>
		public virtual void OnLoad() { }

		/// <summary>
		/// Gets Called once per Frame
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Gets Called before Mod gets Unloaded. Use this to Deregister from handlers/Other Mods or Save Settings
		/// </summary>
		public virtual void BeforeUnload() { }

		/// <summary>
		/// Gets called When the Mod gets unloaded. Use this for Saving of Nessecary Data
		/// </summary>
		public virtual void OnUnload() { }

    }
}
