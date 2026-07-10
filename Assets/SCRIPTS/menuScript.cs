using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class menuScript : MonoBehaviour
{

    public GameObject soundS;
    private void Start()
    {
        soundMngt.sInstance.PlayBGM(3);
    }
    public void onPlay()
    {
        soundMngt.sInstance.stopSound();
        soundMngt.sInstance.PlayBGM(Random.Range(0,2));
        SceneManager.LoadScene(1);

    }
   
    public void toggle()
    {
       soundS.SetActive(!soundS.activeSelf);
    }
}
