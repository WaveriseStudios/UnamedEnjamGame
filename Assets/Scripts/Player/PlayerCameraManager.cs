using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform player; // assign your player

    [Header("Grid Settings")]
    public float cellSize = 20f; // each "room" is 20x20 units
    public float moveSpeed = 5f; // how fast camera moves to new cell

    private Vector3 targetPosition; // where camera should move next

    private void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to CameraFollowGrid!");
            enabled = false;
            return;
        }

        player = FindFirstObjectByType<PlayerCharacter>().transform;

        if(player)
        {
            // Initialize at player's grid position
            targetPosition = GetGridCenter(player.position);
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        }
    }

    private void Update()
    {
        player = FindFirstObjectByType<PlayerCharacter>().transform;

        if(player)
        {
            Vector3 playerGridCenter = GetGridCenter(player.position);

            // If player moves into a new grid cell, update camera target
            if (playerGridCenter != targetPosition)
                targetPosition = playerGridCenter;

            // Smooth move camera towards target cell center
            Vector3 newPos = Vector3.Lerp(transform.position,
                                          new Vector3(targetPosition.x, targetPosition.y, transform.position.z),
                                          Time.deltaTime * moveSpeed);
            transform.position = newPos;
        }
    }

    // Rounds player position to nearest 20x20 cell center
    private Vector3 GetGridCenter(Vector3 position)
    {
        float x = Mathf.Floor(position.x / cellSize) * cellSize + cellSize / 2f;
        float y = Mathf.Floor(position.y / cellSize) * cellSize + cellSize / 2f;
        return new Vector3(x, y, 0);
    }
}
