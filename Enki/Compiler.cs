using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System;

namespace Enki.Compiling
{
	public static class Compiler{
		public static Mod CompileFile(File data) {
			var provider = new CSharpCodeProvider();
			var parameters = new CompilerParameters();

			parameters.ReferencedAssemblies.Add("Enki.dll");

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			Console.WriteLine("Finished Compalation Setup");
			
			var res = provider.CompileAssemblyFromSource(parameters, data.StringData);

			Console.WriteLine("Finished Compalation {0}", res.Errors.HasErrors ? "With errors" : "");

			if (res.Errors.HasErrors) {
				foreach (System.Exception e in res.Errors)
					throw e;
			}

			System.Reflection.Assembly assembly = res.CompiledAssembly;
			Type program = assembly.GetType(System.IO.Path.GetFileNameWithoutExtension(data.Name));

			var m = Activator.CreateInstance(program) as Mod;
			return m;
		}
	}
}
