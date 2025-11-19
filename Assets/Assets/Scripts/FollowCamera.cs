using UnityEngine;   // ¡ESTO ES CLAVE!

public class FollowCamera : MonoBehaviour
{
    public Transform player;      // El personaje
    public Vector3 offset;        // Offset de la cámara

    // Tus 4 paredes tal cual se llaman en el inspector
    public BoxCollider2D Walls;    // "Walls"
    public BoxCollider2D Walls_1;  // "Walls (1)"
    public BoxCollider2D Walls_2;  // "Walls (2)"
    public BoxCollider2D Walls_3;  // "Walls (3)"

    private float minX, maxX, minY, maxY;
    private float camHalfHeight, camHalfWidth;

    void Start()
    {
        // Obtenemos la cámara
        Camera cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        camHalfHeight = cam.orthographicSize;
        camHalfWidth  = camHalfHeight * cam.aspect;

        // Unimos los 4 colliders en un solo bounds
        Bounds total = Walls.bounds;
        total.Encapsulate(Walls_1.bounds);
        total.Encapsulate(Walls_2.bounds);
        total.Encapsulate(Walls_3.bounds);

        // Límites ajustados al tamaño de la cámara
        minX = total.min.x + camHalfWidth;
        maxX = total.max.x - camHalfWidth;

        minY = total.min.y + camHalfHeight;
        maxY = total.max.y - camHalfHeight;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + offset;

        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = targetPos;
    }
}