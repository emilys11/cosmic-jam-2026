using UnityEngine;

public class TitleEgg : MonoBehaviour
{
    void FixedUpdate()
    {
        if(transform.position.y > 7.8f) Destroy(gameObject);
    }
}
