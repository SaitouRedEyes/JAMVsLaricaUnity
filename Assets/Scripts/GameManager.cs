using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    private SceneManager sceneManager;

	void Start () 
    {
        sceneManager = SceneManager.GetInstance();

        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            case (int)SceneManager.Scenes.Menu: sceneManager.CurrScene = new Menu(); break;
            case (int)SceneManager.Scenes.Game: sceneManager.CurrScene = new Level(); break;
            case (int)SceneManager.Scenes.GameOver: sceneManager.CurrScene = new GameOver(); break;
        }
	}
	
	void Update ()     { sceneManager.CurrScene.Update(); }
    void FixedUpdate() { sceneManager.CurrScene.FixedUpdate(); }
}