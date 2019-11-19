using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Enki
{
	class MonoModLoader : MonoBehaviour
	{
		//int lastCount = 0;
		Canvas ModCanvas;

		Text TextOutput;

		bool hasStarted = false;

		bool ShouldLoad = false;

		void Awake() {


			DontDestroyOnLoad(this);

			ModCanvas = gameObject.AddComponent<Canvas>();
			ModCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			ModCanvas.worldCamera = Camera.main;
			gameObject.transform.parent = null;
			TextOutput = (new GameObject()).AddComponent<Text>();
			TextOutput.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
			TextOutput.transform.SetParent(ModCanvas.transform);
			TextOutput.text = "Running Enki on Version: " + ModLoader.Version;
			RectTransform t = TextOutput.transform as RectTransform;

			Rect rect = t.rect;
			t.anchorMin = new Vector2(0f, 1f);
			t.anchorMax = new Vector2(0f, 1f);
			t.pivot = new Vector2(0f, 1f);
			t.sizeDelta = new Vector2(200, 50);
			t.anchoredPosition = new Vector2(50f, -50f);

			if (!hasStarted)
				Initialize();

			hasStarted = true;
		}

		void Start() {
			if (!hasStarted){
				Console.WriteLine("Start called Before Awake.");
				Initialize();
				hasStarted = true;
			}

			foreach (Mod mod in ModLoader.Mods)
			{
				mod.OnLoad();
			}
		}

		void Initialize() {

			string[] mods = Array.FindAll(Directory.GetFiles(ModLoader.ModDir), path => path.ToLower().EndsWith(".zip"));

			Console.WriteLine("Found {0} Mod{1}.", mods.Length, (mods.Length == 1 ? "" : "s"));

			foreach (string modPath in mods) {
				try{
					ModData loadedMod = FileLoader.LoadModData(modPath);
					if(loadedMod == null) Console.WriteLine("Mod Couldnt Load for unknown Reasons");
				}catch (Exception e) {
					Console.WriteLine("Failed to Load Mod {0}, {1}", e.Message, e.StackTrace);
				}

			}

			Console.WriteLine("Managed to load {0} Mod{1}.", ModLoader.Mods.Count, (ModLoader.Mods.Count == 1 ? "" : "s"));
		}

		void Update()
		{
			foreach (Mod mod in ModLoader.Mods) {
				mod.Update();
			}
		}
	}
}
