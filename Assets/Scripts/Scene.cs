using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public abstract class Scene
{
    private Transform[] sceneUIs;

    public abstract void Update();
    public virtual void FixedUpdate(){}

    protected Component CreateUI<myComponent>(string name, Vector2 anchorMin, Vector2 anchorMax) where myComponent : Component
    {
        GameObject ui = new GameObject(name);
        ui.AddComponent<myComponent>();
        ui.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
        ui.layer = LayerMask.NameToLayer("UI");

        RectTransform transform = ui.GetComponent<RectTransform>();

        transform.anchorMin = anchorMin;
        transform.anchorMax = anchorMax;
        transform.offsetMin = transform.offsetMax = Vector2.zero;

        return ui.GetComponent<myComponent>();
    }

    protected Text SetText(Text text, Color color)
    {
        Text tempText = text;
        tempText.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        tempText.resizeTextForBestFit = true;
        tempText.color = color;

        return tempText;
    }

    protected void ChangeScene()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                case (int)SceneManager.Scenes.Menu: UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneManager.Scenes.Game); break;
                case (int)SceneManager.Scenes.GameOver: UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneManager.Scenes.Menu); break;
            }
        }
    }
}