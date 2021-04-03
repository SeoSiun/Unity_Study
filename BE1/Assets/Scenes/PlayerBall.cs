using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBall : MonoBehaviour{
    Rigidbody rigid;
    public float jumpPower=30;
    public int itemCount;
    bool isJump;
    AudioSource audio;
    public GameManagerLogic manager;
    
    void Awake(){
        isJump=false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }
    void Update(){
        if(Input.GetButtonDown("Jump") && !isJump){
            isJump=true;
            rigid.AddForce(new Vector3(0,jumpPower,0),ForceMode.Impulse);
        }
    }
    void FixedUpdate(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h,0,v),ForceMode.Impulse);
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag == "Floor") isJump = false;
    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "Item"){
            itemCount++;
            audio.Play();
            other.gameObject.SetActive(false);    //SetActive(bool) : 오브젝트 활성화 함수
            manager.GetItem(itemCount);
        }
        if(other.tag == "Finish"){
            if(manager.totalItemCount==itemCount){
                //Game clear;
                SceneManager.LoadScene(manager.stage+1);
            }
            else{
                //Restart
                SceneManager.LoadScene(manager.stage);
            }
        }
    }
}
