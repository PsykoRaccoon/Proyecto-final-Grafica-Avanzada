using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public int totalObjectives;
    public int destroyedCount;

    public EscapeManager escapeManager;

    public void ObjectiveDestroyed()
    {
        destroyedCount++;

        if (destroyedCount >= totalObjectives)
        {
            Debug.Log("Todos los objetivos destruidos. ¡Escapa!");
            //igual ayudame con la UI manito no seas malo
            escapeManager.StartEscapeTimer();
        }
    }
}
