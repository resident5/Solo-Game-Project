using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Interactable
{
    public enum PortalID { A, B, C, D, E, F }

    public PortalID portalSelection;

    public bool teleportToScene;
    public Scene nextScene;

    public override void Interact()
    {
        Debug.Log("Enter portal");
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        ///AsyncOperation op = SceneManager.LoadSceneAsync(nextScene.buildIndex);
        LoadSceneParameters sceneParameter = new LoadSceneParameters(LoadSceneMode.Additive);

        Scene newScene = SceneManager.LoadScene(nextScene.name, sceneParameter);
        var allGameObjects = newScene.GetRootGameObjects();

        //Find a gameObject of type portal
        //THIS DOES NOT CURRENTLY WORK IF THERE ARE MULTIPLE PORTALS
        foreach (var obj in allGameObjects)
        {
            var port = obj.GetComponent<Portal>();
            
            //If the portal the player used is the same ID as one of the portals in the new scene then move the player to there
            if(port.portalSelection == portalSelection)
            {
                NewPlayer.Instance.gameObject.transform.position = new Vector2(port.transform.position.x, port.transform.position.y + 20f);
            }

        }

        yield return null;
    }



}
