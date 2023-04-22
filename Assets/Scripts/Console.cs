using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Console : MonoBehaviour
{
	public static Console Instance { get; private set; }

	private string[] consoleLines = new string[10];
	private TMP_Text textUI;

	public void Awake()
	{
		Instance = this;
		textUI = GetComponent<TMP_Text>();
	}

	public void Log(string text)
	{
		// move lines down
		for (int i = consoleLines.Length - 1; i > 0; i--)
		{
			consoleLines[i] = consoleLines[i - 1];
		}

		// prepend text line
		consoleLines[0] = (text + "\n");

		textUI.text = "";
		foreach (var line in consoleLines)
		{
			textUI.text += line;
		}
	}
}