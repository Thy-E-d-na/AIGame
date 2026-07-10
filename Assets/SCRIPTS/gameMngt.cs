using UnityEngine;

public class gameMngt : MonoBehaviour
{
    public static gameMngt Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
  

    [SerializeField] private GameObject keyPref;
    [SerializeField] private GameObject winPnl;
    [SerializeField] private GameObject defeatedPnl;

    public bool gotKey = false;
    public Transform[] keyPos;

    public bool isDefeated = false;

    private void Start()
    {
        int x = Random.Range(0, keyPos.Length - 1);
        Instantiate(keyPref, keyPos[x].position,Quaternion.identity);
    }
    private void Update()
    {
    }
    public void OnWin()
    {
        winPnl.SetActive(gotKey);
    }

    public void OnDefeated()
    {
        if(isDefeated)
        {
            defeatedPnl.SetActive(isDefeated);
        }
    }
}
