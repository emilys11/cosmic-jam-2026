using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] Image blackBackground;
    private bool fadeStart;
    [SerializeField] float fadeTime;
    private float transparency = 0;
    private float fadeSpeed;
    void Awake()
    {
        Color color = new Color(0,0,0,0);
        blackBackground.color = color;
        blackBackground.enabled = false;
        fadeSpeed = 1f/fadeTime;
    }
    void FixedUpdate()
    {
        if (fadeStart)
        {
            transparency += fadeSpeed * Time.deltaTime;
            Color color = blackBackground.color;

            color.a = transparency;
            blackBackground.color = color;

            if(color.a >= 1)
            {
                SwitchScene();
                fadeStart = false;
            }
        }
    }
    public void Play()
    {
        blackBackground.enabled = true;
        fadeStart = true;
        //Fade out

    }

    private void SwitchScene()
    {
        Debug.Log("SwitchScene");
        //Switch scene
    }

    public void Credits()
    {
        //Show credits
    }

    public void Exit()
    {
        
    }
}
