using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPGS : MonoBehaviour
{
    string log;


    void OnGUI()
    {
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 1.5f);


        if (GUILayout.Button("ClearLog"))
            log = "";

        if (GUILayout.Button("Login"))
            GPGSBinder.Inst.Login((success, localUser) =>
            log = $"{success}, {localUser.userName}, {localUser.id}, {localUser.state}, {localUser.underage}");

        if (GUILayout.Button("Logout"))
            GPGSBinder.Inst.Logout();

        if (GUILayout.Button("SaveCloud"))
            GPGSBinder.Inst.SaveCloud("mysave", "want data", success => log = $"{success}");

        if (GUILayout.Button("LoadCloud"))
            GPGSBinder.Inst.LoadCloud("mysave", (success, data) => log = $"{success}, {data}");

        if (GUILayout.Button("DeleteCloud"))
            GPGSBinder.Inst.DeleteCloud("mysave", success => log = $"{success}");

        if (GUILayout.Button("ShowAchievementUI"))
            GPGSBinder.Inst.ShowAchievementUI();

        //if (GUILayout.Button("UnlockAchievement_one"))
        //    GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_one, success => log = $"{success}");

        //if (GUILayout.Button("UnlockAchievement_two"))
        //    GPGSBinder.Inst.UnlockAchievement(GPGSIds.achievement_two, success => log = $"{success}");

        //if (GUILayout.Button("IncrementAchievement_three"))
        //    GPGSBinder.Inst.IncrementAchievement(GPGSIds.achievement_three, 1, success => log = $"{success}");

        if (GUILayout.Button("ShowAllLeaderboardUI"))
            GPGSBinder.Inst.ShowAllLeaderboardUI();

        if (GUILayout.Button("ShowTargetLeaderboardUI_num"))
            GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_bestheight);

        if (GUILayout.Button("ReportLeaderboard_num"))
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_bestheight, 1000, success => log = $"{success}");

        if (GUILayout.Button("LoadAllLeaderboardArray_num"))
            GPGSBinder.Inst.LoadAllLeaderboardArray(GPGSIds.leaderboard_bestheight, scores =>
            {
                log = "";
                for (int i = 0; i < scores.Length; i++)
                    log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
            });

        if (GUILayout.Button("LoadCustomLeaderboardArray_num"))
            GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_bestheight, 10,
                GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily, (success, scoreData) =>
                {
                    log = $"{success}\n";
                    var scores = scoreData.Scores;
                    for (int i = 0; i < scores.Length; i++)
                        log += $"{i}, {scores[i].rank}, {scores[i].value}, {scores[i].userID}, {scores[i].date}\n";
                });

        //if (GUILayout.Button("IncrementEvent_event"))
        //    GPGSBinder.Inst.IncrementEvent(GPGSIds.event_events, 1);

        //if (GUILayout.Button("LoadEvent_event"))
        //    GPGSBinder.Inst.LoadEvent(GPGSIds.event_events, (success, iEvent) =>
        //    {
        //        log = $"{success}, {iEvent.Name}, {iEvent.CurrentCount}";
        //    });

        //if (GUILayout.Button("LoadAllEvent"))
        //    GPGSBinder.Inst.LoadAllEvent((success, iEvents) =>
        //    {
        //        log = $"{success}\n";
        //        foreach (var iEvent in iEvents)
        //            log += $"{iEvent.Name}, {iEvent.CurrentCount}\n";
        //    });

        GUILayout.Label(log);
    }
}
