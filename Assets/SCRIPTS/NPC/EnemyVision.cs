using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Range(1,10)]public float normalVision = 5f;
    [Range(1,5)]public float stealthVision = 5f;
    public GameObject visionCone;
    float range;
    private void Start()
    {
        PlayerStealth player = FindFirstObjectByType<PlayerStealth>();
        player.OnStealthChanged += HandleStealthState;
    }
    void HandleStealthState(bool isStealth)
    {
        if(isStealth)
        {
            GetComponent<EnemyVision>().range = stealthVision;
            visionCone.SetActive(true);
        }
        else
        {
            GetComponent<EnemyVision>().normalVision = normalVision;
            visionCone.SetActive(false);
        }
    }
}
