
//#define DEBUGLOGS

using System;
using System.Collections;
using System.Collections.Generic;
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
    private TimeParserForPlayfab timeParserForPlayfab = new TimeParserForPlayfab();
    
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

    public void Login()
    {
        Connected = false;
        var request = new LoginWithCustomIDRequest
        {
            CustomId = GetCustomGUI(),
            CreateAccount = true
        };
#if DEBUGLOGS
        Debug.Log("Logging into playfab...");
#endif
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessful, OnLoginFailed);
    }

    private void OnLoginSuccessful(LoginResult loginResult)
    {
#if DEBUGLOGS
        Debug.Log("Log into playfab successful with ID: " + loginResult.PlayFabId);
#endif
        Connected = true;
        UpdateLeaderboard();
    }

    private void OnLoginFailed(PlayFabError error)
    {
#if DEBUGLOGS
        Debug.Log("Error while logging into playfab.");
        Debug.Log(error.GenerateErrorReport());
#endif
    }

    private string GetCustomGUI()
    {
        return Guid.NewGuid().ToString();
    }

    public void SubmitTimeScore(float time, string nickname)
    {
        int newTime = timeParserForPlayfab.ParseTime(time);
        StartCoroutine((SubmitScoreCoroutine(newTime, nickname)));
    }

    private IEnumerator SubmitScoreCoroutine(int time, string nickname)
    {
        requestLocked = true;
        
        var userDataRequest = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = nickname
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(userDataRequest, OnUserDisplayNameUpdateSuccessful, OnUserDisplayNameUpdateFailed);
        
        while (requestLocked)
        {
            yield return null;
        }
        
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
#if DEBUGLOGS
        Debug.Log("Statistics send successful");
#endif
        requestLocked = false;
    }

    private void OnStatisticsSaveFailed(PlayFabError error)
    {
#if DEBUGLOGS
        Debug.Log("Error while logging into playfab.");
        Debug.Log(error.GenerateErrorReport());
#endif
        requestLocked = false;
    }

    private void OnUserDisplayNameUpdateSuccessful(UpdateUserTitleDisplayNameResult userUpdateResult)
    {
#if DEBUGLOGS
        Debug.Log("User display name update send successful to: " + userUpdateResult.DisplayName);
#endif
        requestLocked = false;
    }

    private void OnUserDisplayNameUpdateFailed(PlayFabError error)
    {
#if DEBUGLOGS
        Debug.Log("User display name update failed.");
        Debug.Log(error.GenerateErrorReport());
#endif
        requestLocked = false;
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
