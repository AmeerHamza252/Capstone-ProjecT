using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public GameObject gameOverPanel;
	public GameObject gameFinishPanel;
	public GameObject startPanel;
	public GameObject audioPanel;
	public GameObject aboutPanel;
	public GameObject healthBarUI;



	public GameObject playerPrefab;
	public GameObject keyPrefab;
	public GameObject keyPic;
	public GameObject bossPrefab;
	
	PlayerController playerController;
	Boss boss;

	public bool isGameActive;
	public bool startCamera = true;

	public Animator sceneTransitioner;


	public static GameManager instance;

	void Awake()
	{
		//Static instance
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			
		}

		//Play game theme on start
		AudioManager.instance.Play("Theme");

		isGameActive = false;
		
		//Refrences
		playerController =playerPrefab.GetComponent<PlayerController>();
		boss =bossPrefab.GetComponent<Boss>();

		//UI Panels
		healthBarUI.gameObject.SetActive(false);
		startPanel.gameObject.SetActive(true);
		audioPanel.gameObject.SetActive(true);
	
	}
    private void Update()
    {
	
		GameOver();
		
		//Spawn Key
		if(boss.currentHealth <= 0)
        {
			keyPrefab.SetActive(true);
        }
		
    }

    public void GameOver()
	{
		if(playerController.currentHealth <= 0)
        {
			gameOverPanel.SetActive(true);
        }
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void StartGame()
    {
		isGameActive = true;
		startCamera = !startCamera;
		healthBarUI.gameObject.SetActive(true);
		
		startPanel.gameObject.SetActive(false);
		audioPanel.gameObject.SetActive(false);
		

	}

	public void QuitGame()
    {
		Application.Quit();
    }

	public void ShowAboutPanel()
	{
		startPanel.SetActive(false);
		audioPanel.SetActive(false);
		aboutPanel.SetActive(true);
	}

	public void ReturnButtonFromAbout()
	{
		startPanel.SetActive(true);
		audioPanel.SetActive(true);
		aboutPanel.SetActive(false);
	}

	//Camera transition in game


	public void PlayTheme()
	{
		AudioManager.instance.Play("Theme");
	}

	public void StopTheme()
	{
		AudioManager.instance.StopSound("Theme");
	}

}
