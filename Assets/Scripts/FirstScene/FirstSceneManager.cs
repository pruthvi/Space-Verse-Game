using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneManager : MonoBehaviour
{
    public GameObject GuestButton; // Activating Guest button, upon sign in fail
    public bool FailedSignIn = false; // Check condition if signin has failed

    void Update()
    {

        // If Google or any automated sign fails, allows user to get inside game with Guest Login
        // This will be changed to proper login button, if automated sign in fails
        // This can be used as testing purpose as of now for Unity and Web
        if (FailedSignIn == true)
        {
            GuestButton.SetActive(true);
        }
    }

    // Loads Menu Scene Async
    public void LoadMenu()
    {
        Debug.Log("Loading Menu Scene");
        StartCoroutine(AsyncSceneLoading());
    }
    IEnumerator AsyncSceneLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Menu");
        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            yield return null;
        }
    }


}
