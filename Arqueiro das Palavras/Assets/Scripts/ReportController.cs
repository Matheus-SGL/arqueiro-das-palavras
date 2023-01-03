using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReportController : MonoBehaviour {

	private int collectedArrows;
	private int shootedTimes;
	private int collectedCoins;
	private int enemiesKilled;
	private int boxes;
	private int chests;
	private int goldMedals;
	private int silverMedals;
	private int wrongMedals;

	public TextMeshProUGUI collectedArrowsNumber;
	public TextMeshProUGUI shootedTimesNumber;
	public TextMeshProUGUI collectedCoinsNumber;
	public TextMeshProUGUI enemiesKilledNumber;
	public TextMeshProUGUI boxesNumber;
	public TextMeshProUGUI chestsNumber;
	public TextMeshProUGUI goldMedalsNumber;
	public TextMeshProUGUI silverMedalsNumber;
	public TextMeshProUGUI wrongMedalsNumber;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		collectedArrowsNumber.text = collectedArrows.ToString();
		shootedTimesNumber.text = shootedTimes.ToString();
		collectedCoinsNumber.text = collectedCoins.ToString();
		enemiesKilledNumber.text = enemiesKilled.ToString();
		boxesNumber.text = boxes.ToString();
		chestsNumber.text = chests.ToString();
		goldMedalsNumber.text = goldMedals.ToString();
		silverMedalsNumber.text = silverMedals.ToString();
		wrongMedalsNumber.text = wrongMedals.ToString();
	}

	public void AddAchievement(string nomeAchievement)
	{
		switch(nomeAchievement.ToString())
		{
			case "collectedArrows":
				collectedArrows++;
				break;
			case "shootedTimes":
				shootedTimes++;
				break;
			case "collectedCoins":
				collectedCoins++;
				break;
			case "enemiesKilled":
				enemiesKilled++;
				break;
			case "boxes":
				boxes++;
				break;
			case "chests":
				chests++;
				break;
			case "goldMedals":
				goldMedals++;
				break;
			case "silverMedals":
				silverMedals++;
				break;
			case "wrongMedals":
				wrongMedals++;
				break;
			default:
				Debug.Log("ta errado ai seu bosta");
				break;
		}
	}
}
