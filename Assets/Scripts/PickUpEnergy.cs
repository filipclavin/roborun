
using System.Linq;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	private Movement movement;
	private PlayerScore playerScore;
	private MeshRenderer meshRenderer;
	private BatteryController batteryController;
	
	[SerializeField] private bool needFullEnergy = true;
	[SerializeField] private int scoreValue;
    [SerializeField] private int batteryValue;
    
    private void Start()
    {
	    movement = FindAnyObjectByType<Movement>();
		playerScore = FindAnyObjectByType<PlayerScore>();
		meshRenderer = GetComponent<MeshRenderer>();
        batteryController = playerScore.GetComponent<BatteryController>();
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (batteryController.ChargeBattery(batteryValue) == true || needFullEnergy == false)
			{
				playerScore.AddScore(scoreValue);
			}
			gameObject.SetActive(false);

            if (gameObject.CompareTag("Battery"))
			{
				UIManager.Instance.faceAnimator.SetTrigger("HappyTrigger");
				movement.effects.ElementAt(0).Play();
				AudioManager.instance.Play("Battery");
			}
			else if(gameObject.CompareTag("TinCan"))
			{
                UIManager.Instance.faceAnimator.SetTrigger("ScoreTrigger");
                movement.effects.ElementAt(1).Play();
				AudioManager.instance.Play("Can_Pickup");
			}
				
		}
	}
}
