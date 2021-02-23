using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    [SerializeField] private int TargetScroe = 10;
    [SerializeField] private GameObject TargetUIElement;
    [SerializeField] private Transform TargetCanvas;
    
    private Text TargetText;

    IScoreManager score;

    private void Start()
    {
        score = ScoreManager.Instance;
    }

    public void TakeDamage()
    {
        score.Score += TargetScroe;
        var Text = Instantiate(TargetUIElement,TargetCanvas.position,Quaternion.identity,TargetCanvas);
        TargetText = Text.GetComponent<Text>();
        TargetText.text = "+ " + TargetScroe.ToString();
    }

}
