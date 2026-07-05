using System.Collections;
using UnityEngine;

public class cocSpawner : MonoBehaviour
{
   [SerializeField] private GameObject cocPrefab; // The prefab to spawn
    public Transform[] cocPos; // The prefab to spawn
    [SerializeField] private int minCount = 8;
    [SerializeField] private int maxCount = 45;
    int cocHordes = 0;
    private void Start()
    {
        StartCoroutine(spawner()); 
    }
    IEnumerator spawner()
    {
        
        while (cocHordes <= 5)
        {
            var _c = Random.Range(minCount, maxCount);
            var _pos = Random.Range(0, cocPos.Length);
            for (int i = 0; i < _c; i++)
            {
                var coc = Instantiate(cocPrefab, cocPos[_pos].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(2f);
            cocHordes++;
        }
    }
  
}
