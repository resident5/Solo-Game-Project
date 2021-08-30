using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonParser;


public class NPC : Interactable
{
    NewPlayer player { get => NewPlayer.Instance; }

    public DialogueUI dialogueUI;

    new string name;

    int index;

    float interactRadius;

    private void Start()
    {
        dialogueUI = GameController.Instance.GetComponent<DialogueController>().dialogueUI;
    }

    public override void Interact()
    {
        StartCoroutine(StartDialogue());
    }

    public IEnumerator StartDialogue()
    {
        var textFile = Resources.Load<TextAsset>("Conversations/Test/NPCTest");
        var data = MiniJSON.Decode(textFile.text) as IDictionary;
        dialogueUI.gameObject.SetActive(true);
        player.inputActive = false;


        index = 1;

        //Use a for loop?
        foreach (var events in data.Values)
        {
            while (!Input.GetKeyDown(KeyboardInputs.Instance.keybinder["ACTIVATE"]))
            {
                var selectedDialogue = events as IDictionary;
                //Debug.Log(selectedDialogue["char"] as string);

                if (selectedDialogue["char"] as string == "Nonplayer")
                {
                    dialogueUI.playerSide.SetActive(false);

                    dialogueUI.npcNameBox.text = "Terabyte";
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

            if (index >= data.Values.Count)
            {
                EndDialogue();
            }

            index++;


        }

        //var firstEvent = data["event_1"] as IDictionary;

        //var name = firstEvent["char"] as string;

        //Debug.Log(name);

        //player.inputActive = false;

        //events["event_1"];
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        player.inputActive = true;
        dialogueUI.gameObject.SetActive(false);
        dialogueUI.playerSide.SetActive(false);
        dialogueUI.npcSide.SetActive(false);

    }
}
