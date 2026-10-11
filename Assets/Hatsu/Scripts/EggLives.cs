using UnityEngine;

public class EggLives : MonoBehaviour
{
    [SerializeField] GameObject eggIcon;
    [SerializeField] EggHealth eggHealthScript;
    [SerializeField] EggLife[] eggs; //Handles up to how many slots are serialized
    [SerializeField] int numOfEggIcons;

    void Awake()
    {
        numOfEggIcons = eggHealthScript.currentHealth;

        for(int i = numOfEggIcons; i < eggs.Length; i++)
        {
            eggs[i].gameObject.SetActive(false);
        }
    }

    public void ManageLives()
    {
        int health = eggHealthScript.currentHealth;

        if(health > numOfEggIcons)
        {
            eggs[numOfEggIcons].gameObject.SetActive(true);
            numOfEggIcons++;
            //Add a new egg
        }
        else if(health < numOfEggIcons)
        {
            numOfEggIcons--;
            eggs[numOfEggIcons].BreakEgg();

            //Debug.Log(numOfEggIcons);

            //Play shatter effect and destroy one of the eggs
        }
    }
}
