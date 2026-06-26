using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menuScript : MonoBehaviour
{
    [Header("Select Character")]

    #region   Select Character
    public Character selected;
    public GameObject[] Animals;
    public int number;
    public float rotateSpeed = 0.5f;
    public GameObject characterPnl;
    public void ChangeCharacter(int num)
    {
        for (int i = 0; i < Animals.Length; i++)
        {
            Animals[i].SetActive(false);
        }
        number += num;
        if (number > Animals.Length - 1)
        {
            number = 1;
        }
        if (number < 1)
        {
            number = Animals.Length - 1;
        }
        Animals[number].SetActive(true);
        selected = (Character)number;
    }

    #endregion
    private void Start()
    {

    }
    private void Update()
    {
        Animals[number].transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
    public void Create()
    {
        if (number == 0) return;
        characterPnl.SetActive(false);
        var chars = new List<GameObject>();
        for (int i = 0; i < Animals.Length; i++)
        {
            if(i != number) chars.Add(Animals[i]);
        }
        foreach (GameObject character in chars)
        {
            Destroy(character);
        }
    }

}
