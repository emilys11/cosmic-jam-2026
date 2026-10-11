using UnityEngine;
using static PowerupInventory;

public class PowerupOrb : MonoBehaviour
{    public enum OrbType 
    {
        Chicken,
        Egg
    }
    
    [SerializeField] OrbType orbType;
    [Header("FX")]
    [SerializeField] GameObject chickenOrbPickupVFX;
    [SerializeField] GameObject eggOrbPickupVFX;

    public void SpawnOrbPickupVFX()
    {
        GameObject fx = null;

        switch (orbType)
        {
            case OrbType.Chicken: fx = chickenOrbPickupVFX; break;
            case OrbType.Egg: fx = eggOrbPickupVFX; break;
        }

        if (fx == null) return;
        GameObject spawned = Instantiate(fx, transform.position, Quaternion.identity);
        Destroy(spawned, 2f); // cleanup in case the effect's Stop Action isn't Destroy
    }
    public OrbType GetOrbType()
    {
        return orbType;
    }

}
