using UnityEngine;
using static PowerupInventory;

public class PowerupOrb : MonoBehaviour
{    public enum OrbType 
    {
        Chicken,
        Egg
    }
    
    [SerializeField] OrbType orbType;
    public OrbType GetOrbType()
    {
        return orbType;
    }

}
