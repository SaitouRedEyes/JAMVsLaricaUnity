using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : Scene
{
    private Image GameOverScreen;

    public GameOver()
    {
        GameOverScreen = (Image)CreateUI<Image>("GameOverScreen", new Vector2(0, 0), new Vector2(1, 1));

        if (PlayerPrefs.GetInt("Highscore") < PlayerPrefs.GetInt("CurrScore"))
        {
            PlayerPrefs.SetInt("Highscore", PlayerPrefs.GetInt("CurrScore"));
            GameOverScreen.sprite = Resources.Load<Sprite>("Textures/GameOverBG2");
        }
        else GameOverScreen.sprite = Resources.Load<Sprite>("Textures/GameOverBG");
    }

    public override void Update()
    {
        ChangeScene();
    }
}