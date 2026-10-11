using UnityEngine;
using UnityEngine.UI;

public class OrbUI : MonoBehaviour
{
    [SerializeField] Image[] orbs;
    private int index;

    void Awake()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].enabled = true;
        }
    }
    public void RemoveOrb()
    {
        if(index < 2)
        {
            orbs[index].enabled = false;
            index++;
        }

    }
    public void ResetOrbs()
    {
        for(int i = 0; i < orbs.Length; i++)
        {
            orbs[i].enabled = true;
        }
        index = 0;
    }
}
