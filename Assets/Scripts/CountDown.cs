using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Image timeImage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private int durationInSeconds;
    [SerializeField][Range(0, 100)] private int stressTimePercentage;

    private int currentTime;

    public static UnityAction TimerEnding;

    // Start is called before the first frame update
    private void OnEnable()
    {
        currentTime = durationInSeconds;
        timeText.text = currentTime.ToString();
        StartCoroutine(TimeIEn());
    }

    IEnumerator TimeIEn()
    {
        while(currentTime >= 0)
        {
            timeImage.fillAmount = Mathf.InverseLerp(0, durationInSeconds, currentTime);
            timeText.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            currentTime--;
            if(currentTime == (int) 100 / (durationInSeconds * stressTimePercentage))
            {
                timeText.color = Color.red;
            }
        }
        TimerEnding?.Invoke();
        gameObject.SetActive(false);
    }
}
