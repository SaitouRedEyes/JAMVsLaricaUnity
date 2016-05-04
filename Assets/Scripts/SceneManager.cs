using UnityEngine;
using System.Collections;

public class SceneManager 
{
    private static SceneManager instance;
    private Scene currScene;
    public enum Scenes {Menu = 0, Game = 1, GameOver = 2};

    private SceneManager() 
    {}

    public static SceneManager GetInstance()
    {
        if (instance == null) instance = new SceneManager();

        return instance;
    }

    public Scene CurrScene
    {
        get { return this.currScene; }
        set { this.currScene = value; }
    }
}