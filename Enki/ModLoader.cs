using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Enki
{
	public class ModLoader{
		public static ModLoader Instance;
		public static List<Mod> Mods = new List<Mod>();
		internal static string ModDir = Path.GetFullPath("Mods/");
		internal static string ConfigDir = Path.GetFullPath("Mods/Configs/");
		internal static string DataDir = Path.GetFullPath("Mods/Data");
		internal static string DisabledDir = Path.GetFullPath("Mods/Disabled");


		public static Version Version = Assembly.GetExecutingAssembly().GetName().Version;
		public ModLoader() {
			Console.WriteLine("Enki.ModLoader Loaded");

			//World world = World.inst;

			//world.GetType().GetMethod("Setup", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(world, new object[] { 1000, 1000 });
			////TerrainGen.inst.Generate(1000, 1000);
		}

		public static void Initialize() {
			Console.WriteLine("Enki.ModLoader Initialized");

			Directory.CreateDirectory(ModDir);
			Directory.CreateDirectory(ConfigDir);
			Directory.CreateDirectory(DataDir);
			Directory.CreateDirectory(DisabledDir);

			try
			{
				GameObject gm = new GameObject();
				gm.AddComponent<MonoModLoader>();
			}
			catch (Exception e) {
				Console.Error.WriteLine("Unity DLL is not Loaded. Could not Spawn GameObject");
			}
			//return Instance = new ModLoader();
		}

		public void LoadMods() {

		}

		//public static T GetField<T>(this object o, string field) {
		//	return (T)o.GetType().GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).GetValue(o);
		//}
	}
}
