using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Level : Scene
{
    private float itemSpeed, itemCreationTime, itemCreationStartPoint, highscore;
    private bool couldDownTime;
    private GameObject prefabItem, prefabJamv, jamv, prefabBackground, background;
    private PlayerController jamvController;
    private List<GameObject> itens;
    private Texture[] itemTextures;
    private Sprite[] lifeSprites = new Sprite[3];
    private Image lifeImage;
    private Renderer jamvRendererReference, backgroundRendererReference, itemRendererReference;
    private Text scoreText, highscoreText;

    #region Getters and Setters
    public Vector3 GetBackgroundMinReference
    {
        get { return backgroundRendererReference.bounds.min; }
    }

    public Vector3 GetBackgroundMaxReference
    {
        get { return backgroundRendererReference.bounds.max; }
    }

    #endregion

    public Level()
    {
        prefabBackground = (GameObject)Resources.Load("Prefabs/Background");
        background = (GameObject)GameObject.Instantiate(prefabBackground, prefabBackground.transform.localPosition, prefabBackground.transform.localRotation);
        background.tag = "Background";
        backgroundRendererReference = background.GetComponent<Renderer>();

        prefabJamv = (GameObject)Resources.Load("Prefabs/Jamv");   
        jamv = (GameObject)GameObject.Instantiate(prefabJamv, prefabJamv.transform.localPosition, Quaternion.identity);
        jamvRendererReference = jamv.GetComponent<Renderer>();
        jamvController = jamv.GetComponent<PlayerController>();

        prefabItem = (GameObject)Resources.Load("Prefabs/Item");
        itens = new List<GameObject>();
        itemTextures = new Texture[]{(Texture)Resources.Load ("Textures/Hamburguer"), 
								     (Texture)Resources.Load ("Textures/FrenchFries"), 
								     (Texture)Resources.Load ("Textures/Poop"), 
								     (Texture)Resources.Load ("Textures/Ribs"), 
								     (Texture)Resources.Load ("Textures/Rock")};

        itemSpeed = 0.05f;
        itemCreationTime = 2;
        itemCreationStartPoint = Time.time;

        lifeImage = (Image)CreateUI<Image>("Life", new Vector2(0.15f, 0.9f), new Vector2(0.35f, 0.99f));
        for (int i = 0; i < lifeSprites.Length; i++) lifeSprites[i] = Resources.Load<Sprite>("Textures/Life" + (i + 1));

        scoreText = SetText((Text)CreateUI<Text>("Score", new Vector2(0.15f, 0.02f), new Vector2(0.35f, 0.07f)), Color.red);

        highscoreText = SetText((Text)CreateUI<Text>("Highscore", new Vector2(0.6f, 0.02f), new Vector2(0.8f, 0.07f)), Color.red);
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
    }

    public override void Update()
    {
        scoreText.text = "Score: " + jamvController.Score;    
        
        CreateItem();
        UpdateItem();
    }

    public override void FixedUpdate()
    {
        UpdatePlayer();
        PlayerCollisionWithBounds(0.1f);
    }

    /// <summary>
    /// Update Player movement and life sprites.
    /// </summary>
    private void UpdatePlayer()
    {
        if      (Input.GetKey(KeyCode.LeftArrow))  jamv.transform.Translate(Vector3.left * Time.deltaTime * 5.0f);
        else if (Input.GetKey(KeyCode.RightArrow)) jamv.transform.Translate(Vector3.left * Time.deltaTime * -5.0f);


        if (jamvController.Life > 0) lifeImage.sprite = lifeSprites[jamvController.Life - 1];
    }

    /// <summary>
    /// Collision with background bounds
    /// </summary>
    /// <param name="collisionOffset">collision offset</param>
    private void PlayerCollisionWithBounds(float collisionOffset)
    {
        if      (jamvRendererReference.bounds.min.x < backgroundRendererReference.bounds.min.x + collisionOffset) BlockPlayer(collisionOffset);
        else if (jamvRendererReference.bounds.max.x > backgroundRendererReference.bounds.max.x - collisionOffset) BlockPlayer(-collisionOffset);
    }

    private void BlockPlayer(float collisionOffset)
    {
        jamv.transform.localPosition = new Vector3(jamv.transform.localPosition.x + collisionOffset,
                                                   jamv.transform.localPosition.y,
                                                   jamv.transform.localPosition.z);
    }

    /// <summary>
    /// Create the itens.
    /// </summary>
    private void CreateItem()
    {
        if (Time.time - itemCreationStartPoint > itemCreationTime)
        {
            itemCreationStartPoint = Time.time;

            GameObject item = (GameObject)GameObject.Instantiate(prefabItem, prefabItem.transform.position, new Quaternion(0, 0, 180f, 0));

            Renderer itemRendererReference = item.GetComponent<Renderer>();

            item.transform.localPosition = new Vector3((float)Random.Range(backgroundRendererReference.bounds.min.x + (itemRendererReference.bounds.size.x / 2),
                                                                           backgroundRendererReference.bounds.max.x - (itemRendererReference.bounds.size.x / 2)),
                                                                           4.0f,
                                                                           item.transform.localPosition.z);

            item.GetComponent<Renderer>().material.mainTexture = itemTextures[Random.Range(0, 5)];

            if (itemRendererReference.material.mainTexture.name.Equals("Poop") ||
                itemRendererReference.material.mainTexture.name.Equals("Rock"))
            {
                item.gameObject.tag = "BadItem";
            }

            itens.Add(item);
        }        
    }

    /// <summary>
    /// Update the itens.
    /// </summary>
    private void UpdateItem()
    {
        foreach (GameObject i in itens)
        {
            if (i != null)
            {
                i.transform.localPosition = new Vector3(i.transform.position.x,
                                                        i.transform.position.y - itemSpeed,
                                                        i.transform.position.z);

                if (i.transform.position.y < -2.0f)
                {
                    if (!i.transform.tag.Equals("BadItem")) jamv.GetComponent<PlayerController>().Life--;

                    GameObject.Destroy(i);
                    itens.Remove(i);
                    break;
                }
            }
        }

        if (Mathf.Floor(Time.time) % 2.0f == 0 && itemCreationTime > 0.5f)
        {
            if (couldDownTime)
            {                
                couldDownTime = false;
                itemCreationTime = (float)System.Math.Round(itemCreationTime - 0.1f, 1);
            }
        }
        else couldDownTime = true;
    }
}