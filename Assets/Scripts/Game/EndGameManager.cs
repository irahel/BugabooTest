using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class EndGameManager : NetworkBehaviour
{
    private int _myScore = 0;
    private int _enemyScore = 0;

    private string _myName = "";
    private string _enemyName = "";

    [SerializeField] private TextMeshProUGUI _myScoreText;
    [SerializeField] private TextMeshProUGUI _enemyScoreText;
    [SerializeField] private TextMeshProUGUI _resultText;

    [SerializeField] private TextMeshProUGUI _rankingOne;
    [SerializeField] private TextMeshProUGUI _rankingTwo;
    [SerializeField] private TextMeshProUGUI _rankingThree;

    void Start()
    {
        string prefix = NetworkManager.Singleton.LocalClientId.ToString();
        _myScore = PlayerPrefs.GetInt(prefix + "-MyScore", 0);
        _enemyScore = PlayerPrefs.GetInt(prefix + "-EnemyScore", 0);

        _myName = PlayerPrefs.GetString(prefix + "-MyName");
        _enemyName = PlayerPrefs.GetString(prefix + "-EnemyName");

        PostScore(_myName, _myScore);
        PostScore(_enemyName, _enemyScore);

        ShowResult();
        GetRanking();
    }

    private void ShowResult()
    {
        _myScoreText.text = _myName + ": " + _myScore.ToString();
        _enemyScoreText.text = _enemyName + ": " + _enemyScore.ToString();

        if (_myScore > _enemyScore) _resultText.text = "Ganhou!";
        else if (_myScore < _enemyScore) _resultText.text = "Perdeu!";
        else _resultText.text = "Empatou!";
    }

    private void PostScore(string player, int score)
    {
        StartCoroutine(PostScoreCoroutine(player, score));
    }

    private IEnumerator PostScoreCoroutine(string player, int score)
    {
        string url = "http://localhost:6161/score";
        string jsonBody = $"{{ \"player\": \"{player}\", \"score\": {score} }}";

        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Score postado com sucesso!");
            }
            else
            {
                Debug.Log("Erro ao postar o score: " + request.error);
            }
        }
    }

    private void GetRanking()
    {
        StartCoroutine(GetRankingCoroutine());
    }

    private IEnumerator GetRankingCoroutine()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:6161/ranking"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseBody = request.downloadHandler.text;
                List<PlayerRank> rankingData = JsonConvert.DeserializeObject<List<PlayerRank>>(responseBody);

                if (rankingData.Count >= 3)
                {
                    _rankingOne.text = $"{rankingData[0].player}: {rankingData[0].score}";
                    _rankingTwo.text = $"{rankingData[1].player}: {rankingData[1].score}";
                    _rankingThree.text = $"{rankingData[2].player}: {rankingData[2].score}";
                }
            }
            else
            {
                Debug.Log("Erro ao obter o ranking: " + request.error);
            }
        }
    }
}

public class PlayerRank
{
    public string player { get; set; }
    public int score { get; set; }
}
