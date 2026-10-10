using UnityEngine;

public class EggLives : MonoBehaviour
{
    [SerializeField] GameObject eggIcon;

    [SerializeField] int numOfEggs;
    [SerializeField] EggLife[] eggs; //Handles up to how many slots are serialized
    void Awake()
    {
        for(int i = numOfEggs; i < eggs.Length; i++)
        {
            eggs[i].gameObject.SetActive(false);
        }
    }
    public void ManageLives(float lives)
    {
        if(lives > 0)
        {
            eggs[numOfEggs].gameObject.SetActive(true);
            numOfEggs++;
            //Add a new egg
        }
        else if(lives < 0)
        {
            numOfEggs--;
            eggs[numOfEggs].BreakEgg();
            Debug.Log(numOfEggs);

            //Play shatter effect and destroy one of the eggs
        }
    }
}
