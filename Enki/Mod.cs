using System.IO.Compression;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

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
	}

	public class ModData {
		public string Name {
			get { return config.Name; }
		}
		public ZipArchive file;
		public ModConfig config;
		public List<File> LoadedFiles = new List<File>();
		public List<Models.Model> LoadedModels = new List<Models.Model>();
		public bool _enabled = true;

		public ModConfig LoadConfig() {

			if (file == null) throw new System.Exception("The Mod is Missing its Corresponding File.");

			File UnzippedConfig = FileLoader.UnzipConfig(file);

			if (UnzippedConfig == null) throw new System.Exception("Cannot Find Config File.");

			return config = ModConfig.Deserialize(UnzippedConfig);
		}

		public File LoadFile(string path) {
			//Search Through Loaded files and Check if File has been Already Loaded
			int index = LoadedFiles.FindIndex(v => v.path == path);
			if (index != -1) return LoadedFiles[index];
			
			//Load file, Cache Loaded Instance, Return Result
			File f = FileLoader.UnzipFile(file, path);
			LoadedFiles.Add(f);
			return f;
		}
	}

    public class Mod: ModData{

		/// <summary>
		/// Wether the Mod is Enabled and will be Updated
		/// </summary>
		public bool Enabled{
			get { return _enabled; }
			set {
				if (_enabled)
					if (!value)
					{
						BeforeUnload();
						OnUnload();
						_enabled = value;
					}
					else
					if (value)
					{
						BeforeLoad();
						OnLoad();
						_enabled = value;
					}
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
