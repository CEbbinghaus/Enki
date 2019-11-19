using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Enki{
	class ModuleInitializer{
		public static void Initialize() {
			CosturaUtility.Initialize();
			try
			{
				Console.WriteLine("Enki Assembly Loaded");
				ModLoader.Initialize();
			}
			catch (Exception e)
			{
				Console.WriteLine("There was a Fatal Exception in Enki. {0}: {1}", e.TargetSite, e.StackTrace);
			}

		}
	}
}