using Unity.VisualScripting;
using UnityEngine;

public class MiniMapRender : MonoBehaviour
{
    public Camera MiniMap;
    public Transform Player;
    public float updateInterval = 0.005f;
    public float MovementTrashHold = 0.1f;
    public float LastUpdateTime;
    public Vector3 LastPlayerPosition;
    public Quaternion LastPlayerRotation;
    public float RotationTrashHold = 1.0f;
    void Start()
    {
        MiniMap.enabled = false;
    }
    void LateUpdate ()
    {
        if (Time.time - LastUpdateTime >= updateInterval)
        {
            float Distance = Vector3.Distance(Player.position, LastPlayerPosition);
            float RotationAngle = Quaternion.Angle(Player.rotation, LastPlayerRotation);
            if (Distance > MovementTrashHold || RotationAngle > RotationTrashHold)
            {
                MiniMap.Render();
                LastPlayerPosition = Player.position;
                LastPlayerRotation = Player.rotation;
            }
            LastUpdateTime = Time.time;
        }
    }
}
