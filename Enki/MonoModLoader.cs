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
			TextOutput.text = "Running Enki on Version: 0.1.723";
			RectTransform t = TextOutput.transform as RectTransform;

			Rect rect = t.rect;
			t.anchorMin = new Vector2(0f, 1f);
			t.anchorMax = new Vector2(0f, 1f);
			t.pivot = new Vector2(0f, 1f);
			t.sizeDelta = new Vector2(200, 50);
			t.anchoredPosition = new Vector2(50f, -50f);

			try
			{
				//Assimp.CompileFlags test = Assimp.CompileFlags.Debug;
				//Console.WriteLine("CompileFlag Test: {0}", test);
				System.IO.Compression.ZipArchiveEntry test = null;

				Console.WriteLine("ZipEntry Test: {0}", test);

				//ModData loadedMod = FileLoader.LoadMod("./Mods/TestMod.zip");
				//if (loadedMod == null) Console.WriteLine("Mod could not be Found");
				//else
				//{
				//	Enki.File f = loadedMod.files["House.fbx"];
				//	Enki.File i = loadedMod.LoadFile("./Images/Image.png");

				//	if (i == null) Console.WriteLine("Image could not be Loaded");

				//	Console.WriteLine(i.TextureData.graphicsFormat);
				//}
			}
			catch (Exception e) {
				Console.Error.WriteLine(e.ToString());
 			}

		}

		//void Update() {
		//	if (Input.GetKeyDown(KeyCode.K)) {
		//		Debug.Log((TextOutput.transform as RectTransform).rect);
		//	}
		//}
	}
}
