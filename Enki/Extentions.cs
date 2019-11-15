using System.Reflection;
using System.IO;
using System.Text;

namespace Enki.Extensions
{
	public static class Ext
	{
		public static Weather Instance(this Weather i){
			return (Weather)typeof(Weather).GetField("inst", BindingFlags.GetField | BindingFlags.Static | BindingFlags.Instance).GetValue(null);
		}
		public static World Instance(this World i){
			return (World)typeof(World).GetField("inst", BindingFlags.GetField | BindingFlags.Static | BindingFlags.Instance).GetValue(null);
		}

		public static Stream StringStream(string input) {
			return new MemoryStream(Encoding.UTF8.GetBytes(input));
		}

		public static UnityEngine.Vector3 ToV3(this Assimp.Vector3D v3) {
			return new UnityEngine.Vector3(v3.X, v3.Y, v3.Z);
		}

		public static UnityEngine.Vector2 ToV2(this Assimp.Vector3D v3) {
			return new UnityEngine.Vector2(v3.X, v3.Y);
		}
	}
}
