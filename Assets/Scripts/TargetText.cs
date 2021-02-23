using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TargetText : MonoBehaviour
{
    [SerializeField] private float TextSpeed = 0.1f;
    Text ThisText;
    float timer;
    void Start()
    {
        ThisText = GetComponent<Text>();
        var textcolor = ThisText.color;
        ThisText.color = new Color(textcolor.r, textcolor.g, textcolor.b, 1);
    }


    void Update()
    {
        var textcolor = ThisText.color;
        transform.Translate(Vector3.up * TextSpeed);
        if (textcolor.a <= 0)
        {
            Destroy(gameObject);
            return;
        }
        timer += Time.deltaTime;
        if(timer >= 0.3)
            ThisText.color = new Color(textcolor.r, textcolor.g, textcolor.b, textcolor.a - 0.1f);
    }
}
