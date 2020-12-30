using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Singleton
    private static GameController instance;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }
            return instance;
        }
    }
    #endregion

    NewPlayer mainPlayer;

    [Range(0.01f, 0.2f)]
    public float hitStopAmount;

    #region Menu
    [SerializeField] GameObject PauseMenu;
    #endregion

    bool paused = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        mainPlayer = FindObjectOfType<NewPlayer>();
    }

    void Start()
    {

    }

    void Update()
    {
        Inputs();
        InputData();

    }

    void Inputs()
    {
        if (Input.GetKeyDown(KeyboardInputs.Instance.keybinder["PAUSE"]))
        {
            paused = !paused;

        }
    }

    void InputData()
    {
        if (paused)
        {
            Time.timeScale = 0;
            mainPlayer.inputActive = false;
            PauseMenu.SetActive(true);
        }
    }
}
