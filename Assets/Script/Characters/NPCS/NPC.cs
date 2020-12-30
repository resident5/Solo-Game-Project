using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonParser;

public class NPC : MonoBehaviour
{

    bool buttonPressed = false;
    NewPlayer player { get => NewPlayer.Instance; }

    public DialogueUI dialogueUI;

    new string name;

    float interactRadius;

    private void Start()
    {
        dialogueUI = GameController.Instance.GetComponent<DialogueController>().dialogueUI;
    }


    public IEnumerator StartDialogue()
    {
        var textFile = Resources.Load<TextAsset>("Conversations/Test/NPCTest");

        var data = MiniJSON.Decode(textFile.text) as IDictionary;

        dialogueUI.gameObject.SetActive(true);

        foreach (var events in data.Values)
        {
            while (!Input.GetKeyDown(KeyboardInputs.Instance.keybinder["ACTIVATE"]))
            {
                var selectedDialogue = events as IDictionary;
                //Debug.Log(selectedDialogue["char"] as string);

                if (selectedDialogue["char"] as string == "Nonplayer")
                {
                    dialogueUI.playerSide.SetActive(false);

                    dialogueUI.npcNameBox.text = name;
                    dialogueUI.npcDialogueBox.text = selectedDialogue["text"] as string;
                    dialogueUI.npcSide.SetActive(true);

                }

                if (selectedDialogue["char"] as string == "Player")
                {
                    dialogueUI.npcSide.SetActive(false);

                    dialogueUI.playerNameBox.text = "Atlas";
                    dialogueUI.playerDialogueBox.text = selectedDialogue["text"] as string;
                    dialogueUI.playerSide.SetActive(true);

                }
                yield return null;
            }

            yield return null;

            Debug.Log("Finished dialouge");

        }

        //var firstEvent = data["event_1"] as IDictionary;

        //var name = firstEvent["char"] as string;

        //Debug.Log(name);

        //player.inputActive = false;


        //events["event_1"];
    }

    void EndDialogue()
    {
        dialogueUI.gameObject.SetActive(true);
        dialogueUI.playerSide.SetActive(false);
        dialogueUI.npcSide.SetActive(false);

    }

    public void ButtonPressed()
    {
        buttonPressed = true;
    }
}
