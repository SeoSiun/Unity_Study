using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    string targetMsg;
    public GameObject EndCusor;
    public int CharPerSeconds;
    Text msgText;
    AudioSource audioSource;
    int index;
    float interval;
    public bool isAnim;

    private void Awake(){
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg){
        if(isAnim){
            msgText.text=targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else{
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart(){
        msgText.text="";
        index=0;
        EndCusor.SetActive(false);

        interval = 1.0f/CharPerSeconds;
        isAnim=true;

        Invoke("Effecting",interval);
    }

    void Effecting(){
        if(msgText.text==targetMsg){
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        if(targetMsg[index]!=' ' || targetMsg[index]!='.')
        audioSource.Play();
        index++;

        Invoke("Effecting",interval);

    }
    
    void EffectEnd(){
        EndCusor.SetActive(true);
        isAnim=false;
    }
}
