using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System;
using Enki;
using Enki.Models;
using Enki.Extensions;
using System.Linq;
using System.Drawing;

namespace Test
{
	class Program
	{
		private static bool CompareMemoryStreams(MemoryStream ms1, MemoryStream ms2)
		{
			if (ms1.Length != ms2.Length)
				return false;
			ms1.Position = 0;
			ms2.Position = 0;

			var msArray1 = ms1.ToArray();
			var msArray2 = ms2.ToArray();

			return msArray1.SequenceEqual(msArray2);
		}

		static void Main(string[] args)
		{
			//Console.WriteLine("Mod loader Test");
			//string CurrentDirectory = System.AppContext.BaseDirectory;
			//string testoutput = Path.Combine(CurrentDirectory, "test.xml").Replace("\\", "/");
			//string testInput = Path.Combine(CurrentDirectory, "testMod.zip").Replace("\\", "/");
			//ModConfig c = new ModConfig();
			//XmlSerializer serializer = new XmlSerializer(typeof(ModConfig));
			//using (StreamWriter writer = new StreamWriter(testFile)) {
			//	serializer.Serialize(writer, c);
			//}

			//ModData mod = FileLoader.LoadMod(testInput);
			//Console.WriteLine(mod.Name);
			//foreach (var e in mod.LoadedFiles) {

			//}

			//Loads the Zip file into the Zip Archive


			//ZipArchive a = FileLoader.LoadZipFile("testMod.zip");
			//if (a == null) Console.Error.WriteLine("Could not find Zip");

			//ZipArchiveEntry e = a.GetEntry("t.fbx");


			//var ms = new MemoryStream();

			//var ZipOutputStream = e.Open();

			//e.Open().CopyTo(ms);

			//ms.Seek(0, SeekOrigin.Begin);;













			//Enki.File fbxFile = loadedMod.LoadFile("t.fbx");

			//AssimpContext ctx = new AssimpContext();
			//Scene s = ctx.ImportFileFromStream(fbxFile.StreamData);
			//Console.WriteLine(s.Meshes[0].Vertices.Count);

			//string code = @"
			//	using System;
			//	using Enki;

			//	public class Program: Mod
			//	{
			//		public override void OnLoad()
			//		{
			//		" +
			//			"Console.WriteLine(\"Hello, world!\");"
			//			+ @"
			//		}
			//	}
			//";

			//string path = Path.GetFullPath("Enki.dll");
			//if (File.Exists(path))
			//	Assembly.LoadFile(path).CreateInstance("EnkiModLoader");











			ModData loadedMod = FileLoader.LoadMod("../Mods/TestMod.zip");
			if (loadedMod == null) Console.WriteLine("Mod could not be Found");
			else {
				Enki.File f = loadedMod.files["House.fbx"];
				int ModelCount = ModelLoader.MeshCount(f);
				Console.WriteLine("Loaded {0} Models", ModelCount);

				var fl = FileLoader.UnzipFile(loadedMod.file, "./Images/Image.png");

				Enki.File i = loadedMod.LoadFile("./Images/Image.png");

				if (i == null) Console.WriteLine("Image could not be Loaded");
				Console.WriteLine("Found Image: " + i.ImageData.Width);
				//Console.WriteLine("Found {0} Models", ModelCount);
			}










			//var t = loadedMod.files;
			//Enki.File code = loadedMod.LoadFile(loadedMod.config.Entry);
			//Mod m = Enki.Compiling.Compiler.CompileFile(code);


			/*
			 		if (File.Exists("X:\\Projects\\C#\\Enki\\build\\Debug\\enki.dll"))
		{
			Assembly.LoadFile("X:\\Projects\\C#\\Enki\\build\\Debug\\enki.dll").GetType("ModLoader").GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
		}
			 
			 */

			//m.OnLoad();

			//Mod output = Enki.Compiling.Compiler.CompileMod(code, loadedMod);

			//output ^= loadedMod;

			//output.OnLoad();

			//Console.WriteLine();























			////Quick Sanity Check
			//if (ms.Length != fs.Length) throw new Exception("Streams dont Match");

			////Loops Over all Bytes and does a One to One Comparrison. Always Fails Immdeiately because b1 is -1
			//long i = fs.Length;
			//while (i > 0) {
			//	int b1 = fs.ReadByte();
			//	int b2 = nms.ReadByte();
			//	if (b1 != b2) {
			//		throw new Exception("Streams dont Match");
			//	}
			//}

			//Parses FBX data
			//Console.WriteLine("{0}, {1}, {2}", fs.Length, fs.Length, MemoryStream.Equals(fs, nms));

			//System.IO.File.WriteAllBytes("./Test.fbx", ms.ToArray());



			Console.ReadLine();
			//tfs.Close();
			//var fs = System.IO.File.OpenRead(Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Test.bin"));
		}
	}
}
