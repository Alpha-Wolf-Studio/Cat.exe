using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardPanelUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> positionsText = new List<TextMeshProUGUI>();

    public void SetLeaderboard(List<PlayfabManager.LeaderBoardResult> leaderBoard)
    {
        Debug.Log("Update leaderBoard");
        int resultsAmount = leaderBoard.Count;
        for (int i = 0; i < resultsAmount; i++)
        {
            string timeValueString = leaderBoard[i].timeValue.ToString().Replace(',', '.');
            positionsText[i].text = leaderBoard[i].nickname + " - " + " Time: " + timeValueString;
        }

        for (int i = resultsAmount; i < positionsText.Count; i++)
        {
            positionsText[i].text = "";
        }
    }
    
}
