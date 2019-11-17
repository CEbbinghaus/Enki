using System;
using System.Reflection;

namespace Enki
{
	class ModuleInitializer{
		public static void Initialize() {
			Console.WriteLine("Enki Assembly Loaded");

			//Resolve All Dependencies into the Embedded DLL
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

				dllName = dllName.Replace(".", "_");

				if (dllName.EndsWith("_resources")) return null;

				string RequiringType = typeof(ModuleInitializer).Namespace + ".Properties.Resources";

				System.Resources.ResourceManager rm = new System.Resources.ResourceManager(RequiringType, System.Reflection.Assembly.GetExecutingAssembly());

				Console.WriteLine("Trying to Load: {0}:{1} Through Resources", RequiringType, dllName);
				byte[] bytes = (byte[])rm.GetObject(dllName);

				return System.Reflection.Assembly.Load(bytes);
			};

			//LoadAssembly("AssimpNet");


			ModLoader.Initialize();

		}

		//public static void LoadAssembly(string assembly) {
		//	string RequiringType = typeof(ModuleInitializer).Namespace + ".Properties.Resources";
		//	System.Resources.ResourceManager rm = new System.Resources.ResourceManager(RequiringType, System.Reflection.Assembly.GetExecutingAssembly());



		//	string dllName = (assembly.Contains(",") ? assembly.Substring(0, assembly.IndexOf(',')) : assembly.Replace(".dll", "")).Replace(".", "_");

		//	Console.WriteLine("Trying to Load: {0}:{1} Through Resources", RequiringType, dllName);

		//	byte[] bytes = (byte[])rm.GetObject(dllName);



		//	Assembly.Load(bytes);
		//}

		//private static string FormatResourceName(Assembly assembly, string resourceName)
		//{
		//	return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
		//													   .Replace("\\", ".")
		//													   .Replace("/", ".");
		//}
	}
}
