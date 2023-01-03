using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWords {
    public static string[] nome = new string[5];
    public static bool[] alreadyUse = new bool[5];
	public static Vector3[] posWord = new Vector3[5];
}

public class GameData {
	public static bool hasNewWord;
	public static string wordsName0;
	public static string wordsName1;
	public static string wordsName2;
	public static string wordsName3;
	public static string wordsName4;

	public static Vector3 posWord0;
	public static Vector3 posWord1;
	public static Vector3 posWord2;
	public static Vector3 posWord3;
	public static Vector3 posWord4;
}
