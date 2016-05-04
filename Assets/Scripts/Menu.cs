using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : Scene 
{
    private Image titleScreen;
    
    public Menu() 
    {
        titleScreen = (Image)CreateUI<Image>("TitleScreen", new Vector2(0,0), new Vector2(1,1));
        titleScreen.sprite = Resources.Load<Sprite>("Textures/MenuBG");
    }
	
	public override void Update() 
    {
        ChangeScene();
	}
}