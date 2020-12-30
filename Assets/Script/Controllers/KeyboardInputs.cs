using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class KeyboardInputs : MonoBehaviour
{
    #region Singletion
    private static KeyboardInputs instance;

    public static KeyboardInputs Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeyboardInputs>();
            }
            return instance;
        }
    }
    #endregion

    public Dictionary<string, KeyCode> keybinder;
    public GameObject keybindGroup;

    void Start()
    {
        keybinder = new Dictionary<string, KeyCode>();

        keybinder.Add("JUMP", KeyCode.W);
        keybinder.Add("LEFT", KeyCode.LeftArrow);
        keybinder.Add("RIGHT", KeyCode.RightArrow);
        keybinder.Add("CROUCH", KeyCode.DownArrow);
        keybinder.Add("ATTACK", KeyCode.Z);
        keybinder.Add("PAUSE", KeyCode.Escape);
        keybinder.Add("ACTIVATE", KeyCode.Space);

        SetKeybindGroups();
    }

    public void RemoveKeybind(string key)
    {
        keybinder.Remove(key);
        ReplaceKeybind(key);
    }

    private void ReplaceKeybind(string key)
    {
        Event e = new Event();

        if (e.isKey)
        {
            keybinder.Add(key, e.keyCode);
        }
    }

    private void SetKeybindGroups()
    {
        try
        {
            foreach(var key in keybinder.Keys)
            {
                var transform = keybindGroup.transform.GetComponent<Keybinds>();
            }
        }
        catch
        {
        }
    }

}
