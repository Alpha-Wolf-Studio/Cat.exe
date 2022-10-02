using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{

    [Header("Leaderboard")] 
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private float leaderboardUpdateTime = 5f;
    [SerializeField] private LeaderboardPanelUI leaderboardDaily = default;
    [SerializeField] private LeaderboardPanelUI leaderboardWeekly = default;
    [SerializeField] private LeaderboardPanelUI leaderboardAllTime = default;
    [SerializeField] private int leaderboardsAmount = 3;

    private int currentLeaderboardsUpdated = 0;
    
    [Header("WaitingPanel")]
    [SerializeField] private GameObject leaderboardWaitPanel;
    private void OnEnable()
    {
        leaderboardPanel.SetActive(false);
        leaderboardWaitPanel.SetActive(true);
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

        currentLeaderboardsUpdated++;

        if (currentLeaderboardsUpdated >= leaderboardsAmount)
        {
            leaderboardPanel.SetActive(true);
            leaderboardWaitPanel.SetActive(false);
        }
    }
}
