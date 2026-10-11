using UnityEngine;

public class EggLives : MonoBehaviour
{
    [SerializeField] GameObject eggIcon;
    [SerializeField] EggHealth eggHealthScript;
    [SerializeField] int numOfEggsIcons;
    [SerializeField] EggLife[] eggs; //Handles up to how many slots are serialized
    
    void Awake()
    {
        numOfEggsIcons = eggHealthScript.currentHealth;
        
        for(int i = numOfEggsIcons; i < eggs.Length; i++)
        {
            eggs[i].gameObject.SetActive(false);
        }
    }

    public void ManageLives()
    {
        int health = eggHealthScript.currentHealth;
        if(health > numOfEggsIcons)
        {
            eggs[numOfEggsIcons].gameObject.SetActive(true);
            numOfEggsIcons++;
            //Add a new egg
        }
        else if(health < numOfEggsIcons)
        {
            numOfEggsIcons--;
            eggs[numOfEggsIcons].BreakEgg();
            //Debug.Log(numOfEggs);

            //Play shatter effect and destroy one of the eggs
        }
    }
}
