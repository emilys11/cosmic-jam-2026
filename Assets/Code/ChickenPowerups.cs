using UnityEngine;

public class ChickenPowerups : MonoBehaviour
{
    [SerializeField] ChickenShooting chickenShooting;
    [SerializeField] float fireRateMultiplier = 0.8f; // TO BE TRIALED

    public void FireRateUpgrade()
    {
        chickenShooting.IncreaseFireRate(fireRateMultiplier);
    }

    public void MultishotUpgrade()
    {
        chickenShooting.Multishot();
    }

    public void ReverseControlsUpgrade()
    {
        // egg controller reverse controls
    }
}
