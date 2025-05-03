using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out var collectible))
        {
        collectible.Collect();
         }
}

        
    }


