using UnityEngine;

public class numbergenerator : MonoBehaviour, IInteractable{
    public void Interact(){
        Debug.Log(Random.Range(0,100));
    }
}