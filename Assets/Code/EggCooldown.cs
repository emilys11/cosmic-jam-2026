using UnityEngine;
using UnityEngine.UI;

public class SwitchCooldownSlider : MonoBehaviour
{
    [SerializeField] private EggOnPath egg;
    [SerializeField] private Slider cooldownSlider;

    private void Start()
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.minValue = 0f;
            cooldownSlider.maxValue = 1f;
            cooldownSlider.value = 1f;
        }
    }

    private void Update()
    {
        if (egg == null || cooldownSlider == null) return;

        cooldownSlider.value = egg.SwitchCooldownProgress;
    }
}
