using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    public bool isOverlapping = false;

    private int collisionCount = 0;

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Building"))
        {
            collisionCount++;
            isOverlapping = collisionCount > 0;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Building"))
        {
            collisionCount--;
            isOverlapping = collisionCount > 0;
        }
    }
}
