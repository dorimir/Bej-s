using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameVolume : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Slider Volumeslider;
    void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume")){
            PlayerPrefs.SetFloat("musicVolume",1);
            Load();
        }else{
            Load();
        }
        
    }

    // Update is called once per frame
    public void changeVolume()
    {
        AudioListener.volume=Volumeslider.value;
        Save();
        Debug.Log(Volumeslider.value);
    }

    private void Load(){
        Volumeslider.value=PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save(){
        PlayerPrefs.SetFloat("musicVolume",Volumeslider.value);
    }
}
