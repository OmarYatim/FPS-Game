using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


interface IScoreManager
{
    int Score { get;
        set;
    }
}
public class ScoreManager : MonoBehaviour, IScoreManager
{
    [HideInInspector] public static ScoreManager Instance;

    [SerializeField] private Text ScoreText;
    private int score = 0;

    private void Awake()
    {
        Instance = this;    
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            ScoreText.text = score.ToString();
        }
    }
}
