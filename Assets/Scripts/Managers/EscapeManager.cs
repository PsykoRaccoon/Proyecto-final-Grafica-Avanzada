using UnityEngine;
using TMPro;

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
    private TextMeshProUGUI resultText; 

    void Start()
    {
        resultText = resultTextObject.GetComponent<TextMeshProUGUI>();
        resultTextObject.SetActive(false); 
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
                ShowResult("No pudiste escapar ¡Perdiste!", Color.red);
            }
            escapeActive = false;
        }
        else if (!isInside)
        {
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
}
