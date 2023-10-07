using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayfabManager : MonoBehaviour
{
    [Header("Login UI")]
    public TMP_Text messageText;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;

    [Header("Load Scene")]
    public string sceneName;

    // void Start()
    // {
    //     Login();
    // }

    public void RegisterButton()
    {
        if(passwordInput.text.Length<6)
        {
            messageText.text="Password too short!";
            return; //to stop the script here
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text="Registered and Logged in!";
        StartCoroutine(Wait());
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email=emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "99A42"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset,OnError);
    }

    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
    }

    // void Login()
    // {
    //     var request = new LoginWithCustomIDRequest
    //     {
    //         CustomId = SystemInfo.deviceUniqueIdentifier,
    //         CreateAccount = true
    //     };
    //     PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    // }

    void OnSuccess(LoginResult result)
    {
        messageText.text = "Logging in!";
        Debug.Log("Successful login/account created");
        StartCoroutine(Wait());
    }

    void OnError(PlayFabError error)
    {
        messageText.text="Encountered an error!";
        Debug.Log("Error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate{
                    StatisticName = "PlatformScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent!");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position+1).ToString();
            texts[1].text = item.PlayFabId;
            texts[2].text = item.StatValue.ToString();

            Debug.Log(string.Format("Place: {0} | ID: {1} | Value: {2}",item.Position, item.PlayFabId, item.StatValue));
        }
    }
    
}
