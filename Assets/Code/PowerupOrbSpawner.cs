using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class PowerupOrbSpawner : MonoBehaviour
{
    [SerializeField] SplineContainer[] rings;

    // orbs
    [SerializeField] GameObject chickenOrbsPrefab;
    [SerializeField] GameObject eggOrbPrefab;

    [SerializeField] float orbLifetime = 5f;
    [SerializeField] float timeBetweenSpawns = 2f;
    [SerializeField] float heightOffset = 0.2f;

    void Start()
    {
        StartCoroutine(SpawnOrbs());
    }
    IEnumerator SpawnOrbs()
    {
        while (true)
        {
            // randomly choose Chicken or Egg orb
            GameObject prefab;

            if (Random.value < 0.5f)
            {
                prefab = chickenOrbsPrefab;
            }
            else
            {
                prefab = eggOrbPrefab;
            }

            SpawnOrb(prefab);

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    void SpawnOrb(GameObject orbPrefab)
    {
        if (rings.Length == 0) return;
        
        int ringIndex = Random.Range(0, rings.Length); // pick random ring
        SplineContainer ring = rings[ringIndex];

        float t = Random.Range(0f, 1f); // random position on selected ring
        Vector3 localPosition = ring.Spline.EvaluatePosition(t);

        Vector3 worldPosition = ring.transform.TransformPoint(localPosition);
        worldPosition.y += heightOffset;

        GameObject orb = Instantiate(orbPrefab, worldPosition, Quaternion.identity);

        Destroy(orb, orbLifetime);
    }

    // spawn chicken and egg orbs separately
    public void SpawnChickenOrb()
    {
        SpawnOrb(chickenOrbsPrefab);
    }
    public void SpawnEggOrb()
    {
        SpawnOrb(eggOrbPrefab);
    }
}
