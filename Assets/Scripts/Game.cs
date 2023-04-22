using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : Singleton<Game>
{
	public TMP_Text pointsDisplay;
	public TMP_Text scoreDisplay;
	public TMP_Text levelDisplay;

	public Player Player { get => player; private set => player = value; }
	[SerializeField] private Player player;

	public bool IsPaused
	{
		get => isPaused;
		set
		{
			isPaused = value;
			Time.timeScale = value ? 0 : 1;
			//Physics.autoSimulation = !value;
			//print($"Game Paused: {value}");
		}
	}
	private bool isPaused = true;

	public float GameTime { get; private set; } = 0;

	private int score = 0;
	public int Points
	{
		get => points;
		set
		{
			points = Mathf.Max(0, value);
			pointsDisplay.text = value.ToString("D3");
			score = Mathf.Max(score, value);
			scoreDisplay.text = $"Score: {score:D3}";
			if (value <= 0) PlayerLose();
		}
	}
	private int points = 0;

	public const float secondsToMaxDifficulty = 300;
	public const float secondsToMaxDifficultyReciprocal = 1 / secondsToMaxDifficulty;
	public float Difficulty { get => GameTime * secondsToMaxDifficultyReciprocal; }

	private void Update()
	{
		GameTime += Time.deltaTime;
		levelDisplay.text = $"Level: {(int)(GameTime * secondsToMaxDifficultyReciprocal * 1000):D3}";
	}

	private void Awake()
	{
		pointsDisplay.text = Points.ToString("D3");
		scoreDisplay.text = $"Score: {score}";
		highscoreText.text = $"Highscore: {score}";
		IsPaused = true;
	}

	private int highscore = 0;
	public TMP_Text failMessage;
	public TMP_Text highscoreText;
	public GameObject menu;
	public Spawner spawner;
	public void PlayerLose()
	{
		spawner.Reset();
		failMessage.enabled = true;
		menu.SetActive(true);
		highscore = Mathf.Max(highscore, score);
		highscoreText.text = $"Highscore: {highscore:D3}";
		IsPaused = true;
		GameTime = 0;
	}
}
