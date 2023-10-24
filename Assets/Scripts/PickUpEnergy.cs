using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEnergy : MonoBehaviour
{
	private PlayerScore playerScore;
	private MeshRenderer meshRenderer;
	private BatteryController batteryController;
	private bool flyingToUI;
	private float flyingSpeed = 1f;
	private float flyTime;

    private Vector3 whenTaken;
	private Transform scoreObject;
	private Collider pickUpcollider;

	[SerializeField] private bool needFullEnergy = true;
	
	public int scoreValue;
    public int batteryValue;

    private void Start()
    {
		playerScore = FindAnyObjectByType<PlayerScore>();
		meshRenderer = GetComponent<MeshRenderer>();
        batteryController = playerScore.GetComponent<BatteryController>();
        pickUpcollider = GetComponent<Collider>();
        scoreObject = batteryController.transform;
    }

    private void OnEnable()
    {
		flyingToUI = false;
    }

    private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (batteryController.ChargeBattery(batteryValue) == true || needFullEnergy == false)
			{
				FlyToUI();
            }
		}
	}

	private void FlyToUI()
	{
		UIManager.Instance.SpawnPickupText(this);
		/*
			flyingToUI = true;
			whenTaken = transform.position;
			pickUpcollider.enabled = false;
		*/
    }
	
    private void Update()
    {
		/*
        if (flyingToUI)
		{
			flyTime += flyingSpeed * Time.deltaTime;
			transform.position = Vector3.Lerp(whenTaken, scoreObject.position, flyTime);
			
			if (transform.position == scoreObject.position)
			{
				playerScore.AddScore(scoreValue);
				gameObject.SetActive(false);
			}
		}
		*/
    }
}
