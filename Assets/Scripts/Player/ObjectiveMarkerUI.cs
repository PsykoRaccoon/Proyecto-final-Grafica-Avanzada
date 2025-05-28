using UnityEngine;
using UnityEngine.UI;

public class ObjectiveMarkerUI : MonoBehaviour
{
    public Transform target;          
    public Camera cam;                
    public RectTransform iconUI;

    public Transform player;
    public float hideDistance;


    void Update()
    {
        if (target == null || cam == null || iconUI == null || player == null)
            return;

        float distance = Vector3.Distance(player.position, target.position);

        if (distance <= hideDistance)
        {
            iconUI.gameObject.SetActive(false);
            return;
        }
        else
        {
            iconUI.gameObject.SetActive(true);
        }

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        if (screenPos.z < 0)
        {
            screenPos *= -1;
        }

        float padding = 50f;
        screenPos.x = Mathf.Clamp(screenPos.x, padding, Screen.width - padding);
        screenPos.y = Mathf.Clamp(screenPos.y, padding, Screen.height - padding);

        iconUI.position = screenPos;
    }
}
