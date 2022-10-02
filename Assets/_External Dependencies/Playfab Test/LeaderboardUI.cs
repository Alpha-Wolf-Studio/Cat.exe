using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{

    [Header("Leaderboard")] 
    [SerializeField] private float leaderboardUpdateTime = 5f;
    [SerializeField] private LeaderboardPanelUI leaderboardDaily = default;
    [SerializeField] private LeaderboardPanelUI leaderboardWeekly = default;
    [SerializeField] private LeaderboardPanelUI leaderboardAllTime = default;

    private void OnEnable()
    {
        PlayfabManager.Get().Login();
        PlayfabManager.Get().OnLeaderboardUpdated += UpdateLeaderBoard;
        StartCoroutine(UpdateLeaderboardCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateLeaderboardCoroutine());
        PlayfabManager.Get().OnLeaderboardUpdated -= UpdateLeaderBoard;
    }

    private IEnumerator UpdateLeaderboardCoroutine()
    {
        while (!PlayfabManager.Get().Connected)
        {
            yield return null;
        }
        while (enabled)
        {
            yield return new WaitForSeconds(leaderboardUpdateTime);
            PlayfabManager.Get().UpdateLeaderboard();
        }
    }

    private void UpdateLeaderBoard(PlayfabManager.LeaderBoardUpdate leaderBoardUpdate)
    {
        switch (leaderBoardUpdate.LeaderboardEnumType)
        {
            case PlayfabManager.LeaderBoardType.AllTime:
                leaderboardAllTime.SetLeaderboard(leaderBoardUpdate.LeaderBoard);
                break;
            case PlayfabManager.LeaderBoardType.Weekly:
                leaderboardWeekly.SetLeaderboard(leaderBoardUpdate.LeaderBoard);
                break;
            case PlayfabManager.LeaderBoardType.Daily:
                leaderboardDaily.SetLeaderboard(leaderBoardUpdate.LeaderBoard);
                break;
        }
    }
}
