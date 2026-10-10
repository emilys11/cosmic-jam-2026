using UnityEngine;

public class EggSpawn : MonoBehaviour
{
    [SerializeField] Transform minSpawnPoint;
    [SerializeField] Transform maxSpawnPoint;
    [SerializeField] GameObject egg;

    [SerializeField] float spawnInterval;
    [SerializeField] float maxForce;
    private float counter;
    void Update()
    {
        counter += Time.deltaTime;
        if(counter >= spawnInterval)
        {
            Vector3 randomPos = new Vector3(Random.Range(minSpawnPoint.localPosition.x, maxSpawnPoint.localPosition.x), transform.position.y + 0.2f, Random.Range(minSpawnPoint.localPosition.z, maxSpawnPoint.localPosition.z));
            GameObject e = GameObject.Instantiate(egg, randomPos, Random.rotation);
            e.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(0.1f, maxForce), ForceMode.Impulse);
            // e.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0,1), Random.Range(0,1), Random.Range(0,1)) * Random.Range(0, 0.5f), ForceMode.Impulse);

            counter = 0;
        }
    }
}
