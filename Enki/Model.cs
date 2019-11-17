using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Enki.Extensions;
using Assimp;

namespace Enki.Models
{
	public class Model{
		File file;
		UnityEngine.Vector3[] Verticies;
		int[] Indicies;
		UnityEngine.Vector2[] UVs;

		public static implicit operator Model(Assimp.Mesh s) {
			Model m = new Model();
			m.Verticies = s.Vertices.ConvertAll( v => v.ToV3()).ToArray();
			m.Indicies = s.GetIndices();
			m.UVs = s.TextureCoordinateChannels[0].ConvertAll(v => v.ToV2()).ToArray();
			return m;
		}

		public static implicit operator UnityEngine.Mesh(Model s) {
			UnityEngine.Mesh m = new UnityEngine.Mesh();
			m.vertices = s.Verticies;
			m.triangles = s.Indicies;
			m.uv = s.UVs;
			return m;

		}
	}

	public static class ModelLoader {
		public static Scene LoadScene(File model) {
			if (model.FileType != File.Type.Model) return null;

			Assimp.AssimpContext ctx = new Assimp.AssimpContext();
			Assimp.Scene s = ctx.ImportFileFromStream(model.StreamData);
			return s;
		}

		public static Assimp.Mesh LoadModel(File model) {
			if (model.FileType != File.Type.Model) return null;

			Assimp.AssimpContext ctx = new Assimp.AssimpContext();
			Assimp.Scene s = ctx.ImportFileFromStream(model.StreamData);
			if (!s.HasMeshes) return null;
			return s.Meshes[0];
		}

		public static Assimp.Mesh[] LoadModels(File model) {
			if (model.FileType != File.Type.Model) return null;

			Assimp.AssimpContext ctx = new Assimp.AssimpContext();
			Assimp.Scene s = ctx.ImportFileFromStream(model.StreamData);
			if (!s.HasMeshes) return null;
			return s.Meshes.ToArray();
		}

		public static int MeshCount(File model) {
			if (model.FileType != File.Type.Model) return -1;

			Assimp.AssimpContext ctx = new Assimp.AssimpContext();
			Assimp.Scene s = ctx.ImportFileFromStream(model.StreamData);
			return s.MeshCount;
		}
	}
}
