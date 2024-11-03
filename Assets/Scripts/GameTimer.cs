using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float elapsedTime = 0f;
    private bool isCounting = true;

    void Update()
    {
        if (isCounting)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.FloorToInt(elapsedTime);
        timerText.text = $"Tiempo: {seconds}";
    }

    public void StopTimer()
    {
        isCounting = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isCounting = true;
        UpdateTimerDisplay();
    }
}