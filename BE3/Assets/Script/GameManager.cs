using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TypeEffect talk;
    public GameObject scanObject;
    public Animator talkPanel;
    public Image portraitImg;
    public Animator portraitAnim;
    public Sprite prevPortrait;
    public bool isAction;
    public GameObject menuSet;
    public Text questText;
    public Text npcName;
    public GameObject player;
    public TalkManager talkManager;
    public QuestManager questManager;
    public int talkIndex;

    void Start(){
        GameLoad();
        questText.text = questManager.CheckQuest();
    }

    void Update(){
        //Sub Menu
        if(Input.GetButtonDown("Cancel")){
            SubMenuActive();
        } 
    }

    public void SubMenuActive(){
        if(menuSet.activeSelf) menuSet.SetActive(false);
        else menuSet.SetActive(true);
    }

    public void Action(GameObject scanObj){
        //Get current Object
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id,objData.isNpc);

        //Visible Talk for Action
        talkPanel.SetBool("isShow",isAction);
    }

    void Talk(int id, bool isNpc){
        //Set Talk Data
        int questTalkIndex=0;
        string talkData="";
        if(talk.isAnim){
            talk.SetMsg("");
            return;
        }
        else{
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex,talkIndex);
        }

        //End Talk
        if(talkData==null){
            isAction=false;
            talkIndex=0;
            questText.text= questManager.CheckQuest(id);
            return;
        }
        if(isNpc){
            if(id==1000) npcName.text="루나";
            else if(id==2000) npcName.text="루도";

            talk.SetMsg(talkData.Split(':')[0]);
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1,1,1,1);
            if(prevPortrait!=portraitImg.sprite){
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }
        }
        else{
            npcName.text = "";
            talk.SetMsg(talkData);
            portraitImg.color = new Color(1,1,1,0);
        }
        isAction=true;
        talkIndex++;
    }

    public void GameSave(){
        PlayerPrefs.SetFloat("PlayerX",player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY",player.transform.position.y);
        PlayerPrefs.SetFloat("QuestId",questManager.questId);
        PlayerPrefs.SetFloat("QuestActionIndex",questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);
    }

    public void GameLoad(){
        if(!PlayerPrefs.HasKey("PlayerX")) return;
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        float questId = PlayerPrefs.GetFloat("QuestId");
        float questActionIndex = PlayerPrefs.GetFloat("QuestActionIndex");

        player.transform.position = new Vector3(x,y,0);
        questManager.questId = (int)questId;
        questManager.questActionIndex = (int)questActionIndex;
        questManager.ControlObject();
    }
    
    public void GameExit(){
        Application.Quit();
    }
}
