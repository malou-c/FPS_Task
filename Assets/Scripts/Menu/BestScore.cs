using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    [SerializeField] private string _annotation;
    private int _bestScore;
    private TextMeshProUGUI _textBestScore;
    private
    void Start()
    {
        // Для первого запуска
        if (!PlayerPrefs.HasKey("BEST_SCORE"))
            PlayerPrefs.SetInt("BEST_SCORE", 0);

        _textBestScore = GetComponent<TextMeshProUGUI>();
        _bestScore = PlayerPrefs.GetInt("BEST_SCORE");
        UpdateBestScoreText();
    }

    public void UpdateBestScoreText()
    {
        _textBestScore.text = $"{_annotation} {_bestScore}";
    }

    
}
