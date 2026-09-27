using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialoguemanager : MonoBehaviour
{
    public bool ReadyToDialogue = false;
    public Text DialogueField;
    public int ReplicNumber = 1;
    public bool QuestAccepted = false;
    public List<string> Replics;
    public int ReplicsCount = 0;
    public Button NextReplicButton;
    public bool WaitingForQuest = false;
    public bool NPCEnabled = true;
    public string CurrentNPCname;
    public Color ActiveQuest;
    public Color UnActiveQuest;
    public GameObject CurrentNPC;
    public GameObject QuestTarget;
    public GameObject Waypoint;
    public ShopGenerator CurrentNPCshop;
    public bool openBox = false;
    public UIManager UI;
    void Awake()
    {
        Waypoint = GameObject.Find("Hud/WayPoint");
        Waypoint.SetActive(false);
        QuestTarget = GameObject.Find("Spawner");
        NextReplicButton = GameObject.Find("Hud/Dialogue/DialogueField/NextReplic").GetComponent<Button>();
        DialogueField = GameObject.Find("Hud/Dialogue/DialogueField").GetComponent<Text>();
        DialogueField.enabled = false;
        UI = GameObject.Find("Hud").GetComponent<UIManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && ReadyToDialogue)
        {
            StopPlayer();
            DialogueField.enabled = true;
            if (CurrentNPCname == "Villager")
            {
                if (!QuestAccepted)
                {
                    DialogueField.text = DialogueData.Replics[0];
                    Replics = DialogueData.Replics;
                    ReplicsCount = DialogueData.Replics.Count;
                    ReplicNumber = 1;
                    NextReplicButton.enabled = true;
                    QuestAccepted = true;
                }
                if (WaitingForQuest)
                {
                    DialogueField.text = DialogueData.WaitingReplic;
                    NextReplicButton.enabled = false;
                    WaitingForQuest = false;
                }
                if (GameObject.Find("Hud").GetComponent<UIManager>().QuestEnded)
                {
                    DialogueField.text = DialogueData.FinishReplics[0];
                    Replics = DialogueData.FinishReplics;
                    ReplicsCount = DialogueData.FinishReplics.Count;
                    ReplicNumber = 1;
                    NextReplicButton.enabled = true;
                    GameObject.Find("Hud").GetComponent<UIManager>().QuestEnded = false;
                }
            }
            else if (CurrentNPCname == "Trader")
            {
                UI.Shop.GetComponent<ShopController>().ShowShop(CurrentNPCshop);
                DialogueField.enabled = false;
                UI.Interact.enabled = false; 
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && ReadyToDialogue)
        {
            StartPlayer();
            UI.Interact.enabled = true;
        }
        if (ReplicNumber == 2)
        {
            AddNewQuest();
        }
    }
    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "NPC" || col.gameObject.tag == "Merchant")
        {
            UI.Interact.enabled = false;
            ReadyToDialogue = false;
            CurrentNPCshop = null;
        }
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "NPC" && NPCEnabled)
        {
            UI.Interact.enabled = true;
            ReadyToDialogue = true;
            CurrentNPCname = "Villager";
            CurrentNPC = col.gameObject.transform.parent.gameObject;
        }
        if (col.gameObject.tag == "Merchant")
        {
            UI.Interact.enabled = true;
            ReadyToDialogue = true;
            CurrentNPCname = "Trader";
            CurrentNPCshop = col.gameObject.transform.parent.GetComponent<ShopGenerator>();
        }
    }
    public void NextReplic()
    {
        if (ReplicNumber < ReplicsCount)
        {
            DialogueField.text = Replics[ReplicNumber];
            ReplicNumber++;
        }
    }
    public void AddNewQuest()
    {
        GameObject.Find("Hud").GetComponent<UIManager>().Buttons.SetActive(true);
        ReplicNumber = 1;
    }
    public void AcceptQuest()
    {
        GetComponent<Player>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DialogueField.enabled = false;
        GameObject.Find("Hud").GetComponent<UIManager>().Buttons.SetActive(false);
        if (GameObject.Find("Hud").GetComponent<UIManager>().QuestEnded)
        {
            GameObject.Find("Player").GetComponent<Player>().Coin += 50;
            NPCEnabled = false;
            CurrentNPC.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
            Waypoint.SetActive(false);
            Camera.main.gameObject.GetComponent<WayPoint>().QuestTaken = true;
        }
        else
        {
            QuestAccepted = true;
            CurrentNPC.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = ActiveQuest;
            Waypoint.SetActive(true);
            Camera.main.gameObject.GetComponent<WayPoint>().target = QuestTarget.transform;
            Camera.main.gameObject.GetComponent<WayPoint>().QuestTaken = true;
        }
    }
    public void CancelQuest()
    {
        GetComponent<Player>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DialogueField.enabled = false;
        GameObject.Find("Hud").GetComponent<UIManager>().Buttons.SetActive(false);
    }
    public void StartPlayer()
    {
        GetComponent<Player>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        DialogueField.enabled = false;
        GameObject.Find("Hud").GetComponent<UIManager>().Buttons.SetActive(false);
    }
    public void StopPlayer()
    {
        GetComponent<Animator>().SetBool("IsRunning", false);
        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetBool("Death", false);
        GetComponent<Animator>().SetBool("GetHit", false);
        GetComponent<Player>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
