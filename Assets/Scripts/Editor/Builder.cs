using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Stopwatch = System.Diagnostics.Stopwatch;

public class Builder {
	
	private static string[] levels = new string[] {"Assets/Scenes/MainMenu.unity", "Assets/Scenes/Level_1.unity", "Assets/Scenes/Level_2.unity", "Assets/Scenes/Level_3.unity"};
	//private static string[] levels = new string[] {"Assets/Scenes/TestScene.unity"};
	private static string buildPath = Application.dataPath.Replace("Assets", "/")+"Builds/";
	private static Dictionary<string, BuildTarget> buildTypes = new Dictionary<string, BuildTarget>() {
		{"Windows/TwoTogether_Windows_64bit.exe", BuildTarget.StandaloneWindows64},
		{"Windows/TwoTogether_Windows_32bit.exe", BuildTarget.StandaloneWindows},
		//{"Mac/TwoTogether_OSX_Universal.app", BuildTarget.StandaloneOSXUniversal},
		{"Linux/TwoTogether_Linux_Universal.x86", BuildTarget.StandaloneLinuxUniversal}
	};
	
	[MenuItem("Tools/Build Project", false, 1)]
	private static void BuildProject() {
		Stopwatch timer = new Stopwatch();
		timer.Start();
		foreach (KeyValuePair<string, BuildTarget> entry in buildTypes) {
			if (File.Exists(buildPath + entry.Key)) {
				File.Delete(buildPath + entry.Key);
			}
			Debug.Log("Building " + entry.Key);
			BuildPipeline.BuildPlayer(levels,buildPath + entry.Key, entry.Value, BuildOptions.None);
		}
		timer.Stop();
		Debug.Log("Building took: "+(timer.ElapsedMilliseconds/1000)+"s");
	}
}
