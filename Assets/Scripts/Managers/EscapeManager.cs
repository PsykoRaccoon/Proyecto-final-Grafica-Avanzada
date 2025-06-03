using UnityEngine;
using TMPro;
using System.Collections;

public class EscapeManager : MonoBehaviour
{
    public float escapeTime;
    public float timer;
    public TextMeshProUGUI timerText;
    public bool escapeActive = false;
    public bool isInside = false;

    public Transform player;
    public GameObject marker;

    public GameObject resultTextObject;
    public GameObject winOrLose;
    public GameObject nuke;
    private TextMeshProUGUI resultText; 

    void Start()
    {
        resultText = resultTextObject.GetComponent<TextMeshProUGUI>();
        resultTextObject.SetActive(false);
        winOrLose.SetActive(false);
    }

    void Update()
    {
        if (!escapeActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Tiempo restante: " + Mathf.CeilToInt(timer) + "s";

        if (timer <= 0f)
        {
            if (isInside)
            {
                StartCoroutine(ShowLoseWithDelay());
            }
            escapeActive = false;
        }
        else if (!isInside)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            winOrLose.SetActive(true);
            ShowResult("Escapaste ¡Ganaste!", Color.green);
            escapeActive = false;
        }
    }

    void ShowResult(string message, Color color)
    {
        resultText.text = message;
        resultText.color = color;
        resultTextObject.SetActive(true);
        timerText.gameObject.SetActive(false);
    }

    public void StartEscapeTimer()
    {
        marker.gameObject.SetActive(false);
        timer = escapeTime;
        escapeActive = true;
        timerText.gameObject.SetActive(true);
        resultTextObject.SetActive(false); 
    }

    public void SetInside(bool inside)
    {
        isInside = inside;
    }

    IEnumerator ShowLoseWithDelay()
    {
        nuke.SetActive(true);

        Time.timeScale = 0.2f; 
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f; 
        Time.fixedDeltaTime = 0.02f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winOrLose.SetActive(true);
        ShowResult("No pudiste escapar ¡Perdiste!", Color.red);
        Time.timeScale = 0f;
    }
}
