using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	private PlayerScore playerScore;
	private BatteryController batteryController;
	
	[SerializeField] private bool needFullEnergy = true;
	[SerializeField] private int scoreValue;
    [SerializeField] private int batteryValue;
    
    private void Start()
    {
		playerScore = FindAnyObjectByType<PlayerScore>();
        batteryController = playerScore.GetComponent<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (batteryController.ChargeBattery(batteryValue) == true || needFullEnergy == false)
			{
				playerScore.AddScore(scoreValue);
				HandleEffect();
			}
			gameObject.SetActive(false);
		}
	}
    
    private void HandleEffect()
    {
	    if (gameObject.CompareTag("Battery"))
	    {
		    PlayerFXManager.Instance.BatteryEffect();
	    }
	    else if (gameObject.CompareTag("TinCan"))
	    {
		    PlayerFXManager.Instance.CanEffect();
	    }
    }
}
