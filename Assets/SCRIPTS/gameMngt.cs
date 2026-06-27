using UnityEngine;

public class gameMngt : MonoBehaviour
{
    [SerializeField] private GameObject keyPref;
    public Transform[] keyPos;
    private void Start()
    {
        int x = Random.Range(0, keyPos.Length - 1);
        Instantiate(keyPref, keyPos[x].position,Quaternion.identity);
    }
}
