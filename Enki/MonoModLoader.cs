using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Enki
{
	class MonoModLoader : MonoBehaviour
	{
		//int lastCount = 0;
		Canvas ModCanvas;

		Text TextOutput;
		void Start() {
			DontDestroyOnLoad(this);

			ModCanvas = gameObject.AddComponent<Canvas>();
			ModCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
			ModCanvas.worldCamera = Camera.main;
			gameObject.transform.parent = null;
			TextOutput = (new GameObject()).AddComponent<Text>();
			TextOutput.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
			TextOutput.transform.SetParent(ModCanvas.transform);
			TextOutput.text = "Running Enki on Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			RectTransform t = TextOutput.transform as RectTransform;

			Rect rect = t.rect;
			t.anchorMin = new Vector2(0f, 1f);
			t.anchorMax = new Vector2(0f, 1f);
			t.pivot = new Vector2(0f, 1f);
			t.sizeDelta = new Vector2(200, 50);
			t.anchoredPosition = new Vector2(50f, -50f);

			//try {
			//} catch (exception e) {
			//	console.writeline("there was an error at {0}", e.stacktrace);
			//	console.error.writeline(e.);
			//}
			Initialize();
		}

		void Initialize() {
			ModData loadedMod = FileLoader.LoadModData("./Mods/TestMod.zip");

			if (loadedMod == null)
				Console.WriteLine("Mod could not be Found");
			else
			{
				Console.WriteLine("Loading Model");
				Enki.File f = loadedMod.files["House.fbx"];
				Console.WriteLine("Loading Image");
				Enki.File i = loadedMod.LoadFile("./Images/Image.png");
				Console.WriteLine("Loading Code");
				Enki.File c = loadedMod.LoadFile("./Index.cs");

				if (f == null) Console.WriteLine("Model could not be Loaded");
				if (i == null) Console.WriteLine("Image could not be Loaded");
				if (c == null) Console.WriteLine("Code could not be Loaded");

				Mod m = Compiling.Compiler.CompileFile(f);
				Console.WriteLine("Compiled Code from Zipped Mod file: {0}", m);
				Console.WriteLine("Loaded {0} Models from Zipped Mod file", Models.ModelLoader.MeshCount(f));
				Console.WriteLine("Loaded Image from Zipped Mod file with a Width of: {0}", i.ImageData.Width);
			}
		}

		//void Update() {
		//	if (Input.GetKeyDown(KeyCode.K)) {
		//		Debug.Log((TextOutput.transform as RectTransform).rect);
		//	}
		//}
	}
}
