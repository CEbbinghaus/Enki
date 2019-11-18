using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Enki
{
	class ModuleInitializer{
		public static void Initialize() {
			try {
				CosturaUtility.Initialize();
				Console.WriteLine("Enki Assembly Loaded");
				//Console.WriteLine("Embedded Resources:");
				//foreach (string name in Assembly.GetExecutingAssembly().GetManifestResourceNames()) {
				//	Console.WriteLine(name + ",");
				//}
				//Resolve All Dependencies into the Embedded DLL
				//AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
				//{
				//	string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

				//	dllName = dllName.Replace(".", "_");

				//	if (dllName.EndsWith("_resources")) return null;

				//	string RequiringType = typeof(ModuleInitializer).Namespace + ".Properties.Resources";

				//	System.Resources.ResourceManager rm = new System.Resources.ResourceManager(RequiringType, System.Reflection.Assembly.GetExecutingAssembly());

				//	Console.WriteLine("Trying to Load: {0}:{1} Through Resources", RequiringType, dllName);
				//	byte[] bytes = (byte[])rm.GetObject(dllName);

				//	return System.Reflection.Assembly.Load(bytes);
				//};

				//LoadAssembly("AssimpNet");
				//Assembly.Load(GetEmbeddedResource("AssimpNet.dll", Assembly.GetExecutingAssembly()));
				//EmbeddedAssembly.Load("AssimpNet.dll");
				//EmbeddedAssembly.Load("System.IO.Compression.dll");

				ModLoader.Initialize();
			} catch (Exception e) {
				Console.WriteLine("There was a Fatal Exception in Enki. {0}: {1}", e.TargetSite, e.StackTrace);
			}
			
		}


		public static void LoadAssembly(string assembly)
		{
			string RequiringType = typeof(ModuleInitializer).Namespace + ".Properties.Resources";
			System.Resources.ResourceManager rm = new System.Resources.ResourceManager(RequiringType, System.Reflection.Assembly.GetExecutingAssembly());



			string dllName = (assembly.Contains(",") ? assembly.Substring(0, assembly.IndexOf(',')) : assembly.Replace(".dll", "")).Replace(".", "_");

			Console.WriteLine("Trying to Load: {0}:{1} Through Resources", RequiringType, dllName);

			byte[] bytes = (byte[])rm.GetObject(dllName);



			Assembly.Load(bytes);
		}
	}
}

//public static class EmbeddedAssembly
//{
//	static Dictionary<string, Assembly> Index = new Dictionary<string, Assembly>();

//	static EmbeddedAssembly()
//	{
//		AppDomain.CurrentDomain.AssemblyResolve += (s, e) =>
//		{
//			if (Index == null || Index.Count == 0) return null;
//			Console.WriteLine("Trying to resolve {0} through Embedded Resources " + (Index.ContainsKey(e.Name) ? "" : "But it Couldnt find it"), e.Name);
//			if (Index.ContainsKey(e.Name)) return Index[e.Name];
//			return null;
//		};
//	}

//	static void Add(Assembly assembly) { Index.Add(assembly.FullName, assembly); }

//	/// <summary>
//	/// Load Assembly, DLL from Embedded Resources into memory.
//	/// </summary>
//	/// <param name="EmbeddedResource">Embedded Resource string. Example: WindowsFormsApplication1.SomeTools.dll</param>
//	/// <param name="FileName">File Name. Example: SomeTools.dll</param>
//	public static void Load(string FileName)
//	{
//		Assembly assembly = Assembly.GetExecutingAssembly();

//		string EmbeddedResource = assembly.GetName().Name + ".Resources." + FileName.Replace(" ", "_")
//																					.Replace("\\", ".")
//																					.Replace("/", ".");

//		Console.WriteLine("Trying to load Embedded Assembly: {0}", EmbeddedResource);
//		byte[] ByteArray = null;

//		using (Stream ResourceStream = assembly.GetManifestResourceStream(EmbeddedResource))
//		{
//			// Either the file is not existed or it is not mark as embedded resource
//			if (ResourceStream == null) throw new Exception(EmbeddedResource + " was not found in Embedded Resources.");

//			// Get byte[] from the file from embedded resource
//			ByteArray = new byte[(int)ResourceStream.Length];
//			ResourceStream.Read(ByteArray, 0, (int)ResourceStream.Length);

//			try
//			{

//				// Add the assembly/dll into dictionary
//				Add(Assembly.Load(ByteArray));

//				return;
//			}
//			catch(Exception e){
//				bool FileNotWritten = true;

//				// Define the temporary storage location of the DLL/assembly
//				string TempFilePath = Path.GetTempPath() + FileName;

//				#if DEBUG
//					Console.WriteLine("Could not Load Assembly from a byte[]\n{0}", e.ToString());
//				#endif

//				using (SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider())
//				{
//					// Get the hash value from embedded DLL/assembly
//					string HashValue = BitConverter.ToString(SHA1.ComputeHash(ByteArray)).Replace("-", string.Empty);

//					// Determines whether the DLL/assembly is existed or not
//					if (File.Exists(TempFilePath))
//					{

//						#if DEBUG
//							Console.WriteLine("File Already Exists at Location: {0}", TempFilePath);
//						#endif

//						// Get the hash value of the existed file
//						string HashValueOfExistingFile = BitConverter.ToString(
//							SHA1.ComputeHash(File.ReadAllBytes(TempFilePath))).Replace("-", string.Empty);

//						// Compare the existed DLL/assembly with the Embedded DLL/assembly
//						if (HashValue == HashValueOfExistingFile) FileNotWritten = false;
//					}
//				}

//				// Create the file on disk
//				if (FileNotWritten) File.WriteAllBytes(TempFilePath, ByteArray);

//				// Add the loaded DLL/assembly into dictionary
//				Add(Assembly.LoadFile(TempFilePath));
//			}
//		}
//	}
//}
