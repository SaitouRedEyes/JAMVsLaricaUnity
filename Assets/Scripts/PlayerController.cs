using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float fps, currFrame;
    private int numFrames, life, score;
    private Texture[] textures;
    private Renderer myRenderer;

    void Awake()
    {
        life = 3;
        score = 0; 
    }

    void Start()
    {
        myRenderer = gameObject.GetComponent<Renderer>(); 
        fps = 5;
        numFrames = 2;        
        textures = new Texture[numFrames];
        currFrame = 0;
        
        for (int i = 0; i < numFrames; i++) this.textures[i] = (Texture)Resources.Load("Textures/Jamv" + (i + 1));
    }

    void FixedUpdate() 
    {
        GameOver();
        AnimatePlayer(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("BadItem")) life--;
        else score += 1;

        Destroy(other.gameObject);
    }

    public int Life
    {
        get { return life; }
        set { life = value; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    private void AnimatePlayer()
    {
        currFrame = Mathf.Floor(Time.time * fps);
        myRenderer.material.mainTexture = textures[(int)(currFrame % numFrames)];
    }

    private void GameOver()
    {
        if (life == 0)
        {
            PlayerPrefs.SetInt("CurrScore", score);
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneManager.Scenes.GameOver);
        }
    }
}