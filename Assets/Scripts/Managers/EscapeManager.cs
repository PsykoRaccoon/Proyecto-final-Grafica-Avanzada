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

    void Update()
    {
        if (!escapeActive) return;

        timer -= Time.deltaTime;
        timerText.text = "Tiempo restante: " + Mathf.CeilToInt(timer) + "s";


        if (timer <= 0f)
        {
            if (isInside)
            {
                print("Womp womp, perdiste");
                timerText.gameObject.SetActive(false); 
                //ayudame con la UI ploxxx
            }
            escapeActive = false;
        }
        else if (!isInside)
        {
            print("Ganaste, que pro");
            timerText.gameObject.SetActive(false);
            escapeActive = false;
        }
    }

    public void StartEscapeTimer()
    {
        marker.gameObject.SetActive(false);
        timer = escapeTime;
        escapeActive = true;
        timerText.gameObject.SetActive(true); 
    }


    public void SetInside(bool inside)
    {
        isInside = inside;
    }
}
