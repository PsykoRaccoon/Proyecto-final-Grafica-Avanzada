using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public int totalObjectives;
    public int destroyedCount;

    public EscapeManager escapeManager;

    public GameObject debugTextObject; // Referencia al GameObject que contiene el texto
    private TextMeshProUGUI debugText;

    void Start()
    {
        // Obtener el componente de texto desde el GameObject
        debugText = debugTextObject.GetComponent<TextMeshProUGUI>();
        debugTextObject.SetActive(false); // Asegúrate de que esté oculto al inicio si quieres eso
    }

    public void ObjectiveDestroyed()
    {
        destroyedCount++;

        debugTextObject.SetActive(true); // Mostrar el texto al jugador

        if (destroyedCount >= totalObjectives)
        {
            debugText.text = "Todos los objetivos destruidos. ¡Escapa!";
            escapeManager.StartEscapeTimer();
        }
        else
        {
            debugText.text = $"Objetivos destruidos: {destroyedCount}/{totalObjectives}";
        }
    }
}
