using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using Firebase.Extensions;

public class SetFirebase : MonoBehaviour
{
    string authCode;
    FirstSceneManager SceneMG;

    void Start()
    {

        SceneMG = this.GetComponent<FirstSceneManager>();

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false /* Don't force refresh */).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();


        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                Debug.Log("PlayGames successfully authenticated!");
                Debug.Log("AuthCode: " + authCode);

                RunFirebase();

            }
            else
            {
                SceneMG.FailedSignIn = true;

                Debug.Log("PlayGames SignIn Failed");
            }
        });

        // Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        // {
        //     var dependencyStatus = task.Result;
        //     if (dependencyStatus == Firebase.DependencyStatus.Available)
        //     {
        //         Debug.Log("Firebase Ready!!!");
        //     }
        //     else
        //     {
        //         Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //     }
        // });
    }







    private void RunFirebase()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Debug.Log("init firebase auth ");

        Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        Debug.Log(" passed auth code ");

        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInOnClick was canceled.");
                SceneMG.FailedSignIn = true;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInOnClick encountered an error: " + task.Exception);
                SceneMG.FailedSignIn = true;
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            SceneMG.LoadMenu();
        });

    }
}