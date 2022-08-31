using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour, IKillEnemyHandler, IDiePlayerHandler
{
    [SerializeField] private string annotation;
    [SerializeField] private int _pointsForKill;
    private TextMeshProUGUI _text;
    private int _currScore = 0;

    void Start()
    {
        GlobalEventsManager.KillEnemyEvent.AddListener(OnKillEnemy);
        GlobalEventsManager.DiePlayerEvent.AddListener(OnDiePlayer);
        _text = GetComponent<TextMeshProUGUI>();
    }
    public void OnKillEnemy()
    {
        _currScore += _pointsForKill;
        UpdateTextScore();
    }

    public void UpdateTextScore()
    {
        _text.text = $"{annotation} {_currScore}";
    }

    public void OnDiePlayer()
    {
        int bestScore = PlayerPrefs.GetInt("BEST_SCORE");
        if (_currScore > bestScore)
            PlayerPrefs.SetInt("BEST_SCORE", _currScore);
    }

    public void ResetScore()
    {
        _currScore = 0;
        UpdateTextScore();
    }
}
