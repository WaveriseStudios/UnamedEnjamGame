using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerCharacter;
    public float cameraFollowSpeed = 3f;
    public float distanceToPlayer = 10f;

    private void FixedUpdate()
    {
        Vector3 newPos = new Vector3(playerCharacter.position.x, playerCharacter.position.y, -distanceToPlayer);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraFollowSpeed * Time.deltaTime);
    }
}
