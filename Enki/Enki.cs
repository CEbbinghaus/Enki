using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.IO.Compression;

namespace Enki
{
    static class Enki
    {
        public static List<Mod> Mods = new List<Mod>();

		public static void Start() {
		}

		public static void Update() {
		}

		public static void UnpackMods(){

		}

        public static void LoadMods()
        {
            var listOfAllDlls = GetModFilesOf(GetModsPath());

            foreach(var modfile in listOfAllDlls)
            {
                LoadModFile(modfile);
            }
        }

        private static List<string> GetModFilesOf(string path)
        {
            var list = new List<string>();

            //all Files in Directory

			foreach(var file in Directory.GetFiles(path))
            {
                if (file.EndsWith(".dll"))
                {
                    try
                    {
                        list.Add(file);
                    }
                    catch (Exception e)
                    {
						Console.WriteLine("Loaded Mod");
                    }
                }

            }

            //all Directories in Directory

            foreach (var dir in Directory.GetDirectories(path))
            {
                try
                {
                    //recursion
                    foreach(var file in GetModFilesOf(dir))
                    {
                        list.Add(file);
                    }
                }
                catch(Exception e)
                {
					Console.WriteLine("Checked Directory");
                }
            }

            return list;
        }

        private static string GetModsPath()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var rootDir = Directory.GetParent(executingDir).Parent;

            var modDir = Path.Combine(rootDir.FullName, "Mods");

            if (!Directory.Exists(modDir))
                Directory.CreateDirectory(modDir);

            return modDir;
        }

        private static void LoadModFile(string path)
        {
            var assembly = Assembly.LoadFrom(path);
            foreach (var type in FindModTypes(assembly))
            {
                var mod = Activator.CreateInstance(type) as Mod;
                mod.OnLoad();
                Mods.Add(mod);
            }
        }

        private static IEnumerable<Type> FindModTypes(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(Mod).IsAssignableFrom(t));
        }

        public static void Dispatch(Action<Mod> action)
        {
            Mods.ForEach(action);
        }

        public static object GetField<T>(string field, object instance = null)
        {
            return typeof(T).GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).GetValue(instance);
        }

        public static void SetField<T>(object value, string field, object instance = null)
        {
            typeof(T).GetField(field, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).SetValue(instance, value);
        }
    }
}
