using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    private TextMeshProUGUI _textMeshProUGUI;
    private static float timeStart;
    private static bool timerIsOn;

    void Start()
    {
        timerIsOn = true;
        _textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public static void SetTimer(bool timer)
    {
        timerIsOn = timer;
    }

    private void Update()
    {
        if (timerIsOn)
        {
            timeStart += Time.deltaTime;
            float minutes = Mathf.FloorToInt(timeStart / 60);
            float seconds = Mathf.FloorToInt(timeStart % 60);
            _textMeshProUGUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}