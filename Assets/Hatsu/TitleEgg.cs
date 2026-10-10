using UnityEngine;

public class TitleEgg : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("DestroyBox")) Destroy(gameObject);
    }
}
