using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System;

namespace Enki.Compiling
{
	public static class Compiler{
		public static Mod CompileFile(File data) {
			CSharpCodeProvider provider = new CSharpCodeProvider();
			CompilerParameters parameters = new CompilerParameters();

			parameters.ReferencedAssemblies.Add("Enki.dll");

			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;

			CompilerResults res = provider.CompileAssemblyFromSource(parameters, data.StringData);

			if (res.Errors.HasErrors) throw new System.Exception(res.Errors[0].ToString());

			Assembly assembly = res.CompiledAssembly;
			System.Type program = assembly.GetType(Path.GetFileNameWithoutExtension(data.Name));

			var m = Activator.CreateInstance(program) as Mod;
			return m;
		}
	}
}
