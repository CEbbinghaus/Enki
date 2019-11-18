using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System;

namespace Enki.Compiling
{
	public static class Compiler{
		public static Mod CompileFile(File data) {
			var provider = new Microsoft.CSharp.CSharpCodeProvider();
			var parameters = new System.CodeDom.Compiler.CompilerParameters();

			parameters.ReferencedAssemblies.Add("Enki.dll");

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			Console.WriteLine("Finished Compalation Setup");
			
			var res = provider.CompileAssemblyFromSource(parameters, data.StringData);

			Console.WriteLine("Finished Compalation {0}", res.Errors.HasErrors ? "With errors" : "");

			if (res.Errors.HasErrors) {
				foreach (Exception e in res.Errors)
					throw e;
			}

			Assembly assembly = res.CompiledAssembly;
			System.Type program = assembly.GetType(Path.GetFileNameWithoutExtension(data.Name));

			var m = Activator.CreateInstance(program) as Mod;
			return m;
		}
	}
}
