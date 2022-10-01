using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabSceneUI : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField nicknameInputField = default;
    [SerializeField] private TMPro.TMP_InputField inputField = default;
    [SerializeField] private Button sendScoreButton = default;

    [Header("Leaderboard")] 
    [SerializeField] private float leaderboardUpdateTime = 2f;
    [SerializeField] private LeaderboardPanelUI leaderboardDaily = default;
    [SerializeField] private LeaderboardPanelUI leaderboardWeekly = default;
    [SerializeField] private LeaderboardPanelUI leaderboardAllTime = default; 

    private void Start()
    {
        sendScoreButton.onClick.AddListener(delegate
        {
            if (int.TryParse(inputField.text, out var score))
            {
                PlayfabManager.Get().SubmitTimeScore(score, nicknameInputField.text);
                PlayfabManager.Get().UpdateLeaderboard();
            }
        });

        StartCoroutine(UpdateLeaderboardCoroutine());
        
        PlayfabManager.Get().OnLeaderboardUpdated += UpdateLeaderBoard;
    }

    private void OnDestroy()
    {
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
