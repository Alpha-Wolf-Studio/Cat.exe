using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviourSingleton<PlayfabManager>
{

    public Action<LeaderBoardUpdate> OnLeaderboardUpdated;
    public Action OnLeaderboardUpdatedFailed;

    private readonly string PLAYER_PREF_ID = "playerPrefID";
    private readonly string TIME_SCORE_LEADERBOARD_ALLTIME = "TimeScoreComplete";
    private readonly string TIME_SCORE_LEADERBOARD_WEEKLY = "TimeScoreWeekly";
    private readonly string TIME_SCORE_LEADERBOARD_DAILY = "TimeScoreDaily";
    
    bool requestLocked = false;

    public class LeaderBoardUpdate
    {
        public LeaderBoardType LeaderboardEnumType;
        public List<LeaderBoardResult> LeaderBoard = new List<LeaderBoardResult>();
    }
    public class LeaderBoardResult
    {
        public string nickname;
        public int scoreValue;
    }
    
    public enum LeaderBoardType
    {
        Daily,
        Weekly,
        AllTime
    }
    
    public bool Connected { get; private set; }
    
    private void Start()
    {
        Login();
    }

    private void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = GetCustomGUI(),
            CreateAccount = true
        };
        Debug.Log("Logging into playfab...");
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessful, OnLoginFailed);
    }

    private void OnLoginSuccessful(LoginResult loginResult)
    {
        Debug.Log("Log into playfab successful with ID: " + loginResult.PlayFabId);
        Connected = true;
        UpdateLeaderboard();
    }

    private void OnLoginFailed(PlayFabError error)
    {
        Debug.Log("Error while logging into playfab.");
        Debug.Log(error.GenerateErrorReport());
    }

    private string GetCustomGUI()
    {
        return Guid.NewGuid().ToString();
    }

    public void SubmitTimeScore(int time, string nickname)
    {
        var userDataRequest = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = nickname
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(userDataRequest, OnUserDisplayNameUpdateSuccessful, OnUserDisplayNameUpdateFailed);
        
        var sendScoreRequest = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = TIME_SCORE_LEADERBOARD_ALLTIME,
                    Value = -time
                },
                new StatisticUpdate()
                {
                    StatisticName = TIME_SCORE_LEADERBOARD_WEEKLY,
                    Value = -time
                },
                new StatisticUpdate()
                {
                    StatisticName = TIME_SCORE_LEADERBOARD_DAILY,
                    Value = -time
                }   
                
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(sendScoreRequest, OnStatisticsSaveSuccessful, OnStatisticsSaveFailed);
    }
    
    private void OnStatisticsSaveSuccessful(UpdatePlayerStatisticsResult statisticsUpdateResult)
    {
        Debug.Log("Statistics send successful");
    }

    private void OnStatisticsSaveFailed(PlayFabError error)
    {
        Debug.Log("Error while logging into playfab.");
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnUserDisplayNameUpdateSuccessful(UpdateUserTitleDisplayNameResult userUpdateResult)
    {
        Debug.Log("User display name update send successful");
    }

    private void OnUserDisplayNameUpdateFailed(PlayFabError error)
    {
        Debug.Log("User display name update failed.");
        Debug.Log(error.GenerateErrorReport());
    }
    
    public void UpdateLeaderboard()
    {
        RequestLeaderBoard(TIME_SCORE_LEADERBOARD_DAILY, OnLeaderboardDailyRequestSuccessful, OnLeaderboardDailyRequestFailed,0, 0, 10);
        RequestLeaderBoard(TIME_SCORE_LEADERBOARD_WEEKLY, OnLeaderboardWeeklyRequestSuccessful, OnLeaderboardWeeklyRequestFailed,1, 0, 10);
        RequestLeaderBoard(TIME_SCORE_LEADERBOARD_ALLTIME, OnLeaderboardAllTimeRequestSuccessful, OnLeaderboardAllTimeRequestFailed,2, 0, 10);
    }

    private void RequestLeaderBoard(string leaderboard, Action<GetLeaderboardResult> OnRequestSuccessful, Action<PlayFabError> OnRequestFailed, int leaderboardIndex,  int startPosition, int maxResultCount)
    {
        requestLocked = true;
        var request = new GetLeaderboardRequest()
        {
            StatisticName = leaderboard,
            StartPosition = startPosition,
            MaxResultsCount = maxResultCount,
        };
        PlayFabClientAPI.GetLeaderboard(request, OnRequestSuccessful, OnRequestFailed);
    }

    private void OnLeaderboardAllTimeRequestSuccessful(GetLeaderboardResult leaderboardResult)
    {
        LeaderBoardUpdate leaderBoardUpdate = new LeaderBoardUpdate();
        leaderBoardUpdate.LeaderboardEnumType = LeaderBoardType.AllTime;
        foreach (var result in leaderboardResult.Leaderboard)
        {
            LeaderBoardResult newLeaderboardResult = new LeaderBoardResult();
            newLeaderboardResult.nickname = result.DisplayName;
            newLeaderboardResult.scoreValue = -result.StatValue;
            leaderBoardUpdate.LeaderBoard.Add(newLeaderboardResult);
        }
        requestLocked = false;
        OnLeaderboardUpdated?.Invoke(leaderBoardUpdate);
    }
    
    private void OnLeaderboardAllTimeRequestFailed(PlayFabError playFabError)
    {
        OnLeaderboardUpdatedFailed?.Invoke();
    }
    
    private void OnLeaderboardDailyRequestSuccessful(GetLeaderboardResult leaderboardResult)
    {
        LeaderBoardUpdate leaderBoardUpdate = new LeaderBoardUpdate();
        leaderBoardUpdate.LeaderboardEnumType = LeaderBoardType.Daily;
        foreach (var result in leaderboardResult.Leaderboard)
        {
            LeaderBoardResult newLeaderboardResult = new LeaderBoardResult();
            newLeaderboardResult.nickname = result.DisplayName;
            newLeaderboardResult.scoreValue = -result.StatValue;
            leaderBoardUpdate.LeaderBoard.Add(newLeaderboardResult);
        }
        requestLocked = false;
        OnLeaderboardUpdated?.Invoke(leaderBoardUpdate);
    }
    
    private void OnLeaderboardDailyRequestFailed(PlayFabError playFabError)
    {
        OnLeaderboardUpdatedFailed?.Invoke();
    }
    
    private void OnLeaderboardWeeklyRequestSuccessful(GetLeaderboardResult leaderboardResult)
    {
        LeaderBoardUpdate leaderBoardUpdate = new LeaderBoardUpdate();
        leaderBoardUpdate.LeaderboardEnumType = LeaderBoardType.Weekly;
        foreach (var result in leaderboardResult.Leaderboard)
        {
            LeaderBoardResult newLeaderboardResult = new LeaderBoardResult();
            newLeaderboardResult.nickname = result.DisplayName;
            newLeaderboardResult.scoreValue = -result.StatValue;
            leaderBoardUpdate.LeaderBoard.Add(newLeaderboardResult);
        }
        requestLocked = false;
        OnLeaderboardUpdated?.Invoke(leaderBoardUpdate);
    }
    
    private void OnLeaderboardWeeklyRequestFailed(PlayFabError playFabError)
    {
        OnLeaderboardUpdatedFailed?.Invoke();
    }
    
}
