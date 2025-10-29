using UnityEngine;

public class ExitRandomly : MonoBehaviour
{
    private Tile tileManager;

    void Start()
    {
        tileManager = GetComponentInParent<Tile>();
    }
}
