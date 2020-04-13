using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;



public class SetFirebase : MonoBehaviour
{
    string authCode;
    void Start()
    {
        StartFirebase();
    }

    void StartFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Debug.Log("Firebase Ready!!!");
                SignINPlayGames();
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void SignINPlayGames()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
          .RequestServerAuthCode(false /* Don't force refresh */)
          .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

            }
            // else{
            //             statusText.text = "Sign In Failed";
            // }
        });

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                // statusText.text = "SignInWithCredentialAsync was canceled.";
                // Debug.LogError("SignInOnClick was canceled.");

                return;
            }
            if (task.IsFaulted)
            {
                // statusText.text = "SignInWithCredentialAsync encountered an error: " + task.Exception;
                // Debug.LogError("SignInOnClick encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.Log("User Signed In Successfully");
            //  statusText.text = "User signed in successfully: "+  newUser.DisplayName + newUser.UserId;
            // Debug.LogFormat("SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

        });

    }
}
