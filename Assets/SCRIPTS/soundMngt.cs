using UnityEngine;
using UnityEngine.UI;

public class soundMngt : MonoBehaviour
{
    public static soundMngt sInstance;
    [SerializeField] private AudioSource _BGM;
    [SerializeField] private AudioSource _SFX;
    [SerializeField] private AudioClip[] bgm;
    [SerializeField] private AudioClip[] sfx;

    public Slider bgmVol;
    public Slider sfxVol;
    private void Awake()
    {
        if (sInstance == null)
        {
            sInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (sInstance != this)
        {
            Destroy(gameObject); 
        }
    }
    public void PlayBGM(int inde)
    {
        if (inde < bgm.Length)
        {
            _BGM.clip = bgm[inde];
            _BGM.Play();
        }
    }
    public void PlaySfx(int inde)
    {
        if (inde < sfx.Length)
        {
            _SFX.PlayOneShot(sfx[inde]);
        }
    }

    public void stopSound()
    {
        _BGM.Stop();
        
    }

    public void BGMToggle(float vol)
    {
        bgmVol.value = vol;
        _BGM.volume = bgmVol.value;
    }
    public void soundToggle(float vol)
    {
        sfxVol.value = vol;
        _SFX.volume = sfxVol.value;
    }

}
