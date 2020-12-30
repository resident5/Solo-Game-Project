using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject playerSide;
    public Image playerImage;
    public TMP_Text playerNameBox;
    public TMP_Text playerDialogueBox;

    public GameObject npcSide;
    public Image npcImage;
    public TMP_Text npcNameBox;
    public TMP_Text npcDialogueBox;


    // Start is called before the first frame update
    void Start()
    {
        //playerImage = playerSide.GetComponentInChildren<Image>();
        //playerNameBox = playerSide.GetComponentInChildren<GameObject>().transform.Find("Name Text").GetComponent<TMP_Text>();
        //playerDialogueBox = playerSide.GetComponentInChildren<GameObject>().transform.Find("Dialogue Text").GetComponent<TMP_Text>();

        //npcImage = npcSide.GetComponentInChildren<Image>();
        //npcNameBox = npcSide.GetComponentInChildren<GameObject>().transform.Find("Name Text").GetComponent<TMP_Text>();
        //npcDialogueBox = npcSide.GetComponentInChildren<GameObject>().transform.Find("Dialogue Text").GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
