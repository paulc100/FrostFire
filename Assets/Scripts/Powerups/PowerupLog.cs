using UnityEngine;

public class PowerupLog: MonoBehaviour
{
    private SplitScreenPlayerController splitScreenPlayerController;

    private void OnTriggerEnter (Collider other) 
    {
        splitScreenPlayerController = other.gameObject.GetComponentInChildren<SplitScreenPlayerController>();
        
        if (other.CompareTag("Player")) {
            Debug.Log("Log is picked up!");

            if (splitScreenPlayerController.carryingLog == false)
            {
                splitScreenPlayerController.carryingLog = true;

                Destroy(gameObject);
            }
        }
    }
}
