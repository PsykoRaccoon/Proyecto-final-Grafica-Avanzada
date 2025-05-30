using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    public int totalObjectives;
    public int destroyedCount;

    public EscapeManager escapeManager;

    public GameObject debugTextObject; // Referencia al GameObject que contiene el texto
    private TextMeshProUGUI debugText;

    public Transform playerTransform; // Asignar el transform del jugador
    public Transform firstObjective;

    private bool hasActivatedProximityText = false;

    private Vector3 firstObjectivePosition;

    public float distanceToActivate;

    void Start()
    {
        // Obtener el componente de texto desde el GameObject
        debugText = debugTextObject.GetComponent<TextMeshProUGUI>();
        debugTextObject.SetActive(false); // Asegúrate de que esté oculto al inicio si quieres eso
        if (firstObjective != null)
        {
            firstObjectivePosition = firstObjective.position; // Guardar la posición una vez
        }
    }
    void Update()
    {
        if (!hasActivatedProximityText && playerTransform != null)
        {
            float distance = Vector3.Distance(playerTransform.position, firstObjectivePosition);
            if (distance < distanceToActivate)
            {
                hasActivatedProximityText = true;
                debugTextObject.SetActive(true);
                debugText.text = $"Objetivos destruidos: {destroyedCount}/{totalObjectives}";
            }
        }
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
