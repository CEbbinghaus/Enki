using System.IO;
using System.Collections.Generic;
using System.Text;
using Enki.Models;
using System.Drawing;
using UnityEngine;

namespace Enki
{
	public class File
	{
		public enum Type
		{
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

		private object HandledObj = null;

		public File(string path, Stream data)
		{
			Name = System.IO.Path.GetFileName(path);
			Path = path;
			StreamData = data;

			string ext = System.IO.Path.GetExtension(path).ToLower();
			FileType = extentions[ext];
		}

		public object HandleData() {
			if (HandledObj != null) return HandledObj;
			string Extention = System.IO.Path.GetExtension(Path).ToLower();
			if (Extention == ".dll") return HandledObj = System.Reflection.Assembly.Load(Data);
			if (Extention == ".cs") return HandledObj = null; //TODO: Find a way to Runtime compile
			if (Extention == ".xml") return HandledObj = null; //TODO: Dont know what to do with this yet...
			if (Extention == ".fbx") return HandledObj = SceneData;
			if (Extention == ".obj") return HandledObj = SceneData;
			if (Extention == ".jpg") return HandledObj = ImageData;
			if (Extention == ".png") return HandledObj = ImageData;
			if (Extention == ".bmp") return HandledObj = ImageData;
			return null;
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
				Texture2D texture = new Texture2D(0, 0);
				texture.LoadRawTextureData(Data);
				if (texture.width > 0 && texture.height > 0)
					return texture;
				else
					return null;
			}
		}
	}

}
