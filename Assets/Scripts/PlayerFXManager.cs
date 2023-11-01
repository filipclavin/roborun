using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
//Script Made By Daniel Alvarado
public class PlayerFXManager : MonoBehaviour
{
    [FormerlySerializedAs("_faceAnimator")] [SerializeField] private Animator faceAnimator;
    [SerializeField] private List<ParticleSystem> effects;
    [SerializeField] private VisualEffect dustEffect;
    [SerializeField] private GameTimer _gameTimer;
    private BatteryController _batteryController;
    private static readonly int SadTrigger = Animator.StringToHash("SadTrigger");
    private static readonly int ScoreTrigger = Animator.StringToHash("ScoreTrigger");
    private static readonly int HappyTrigger = Animator.StringToHash("HappyTrigger");
    public static PlayerFXManager Instance { get; private set; }

   
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _batteryController = GetComponent<BatteryController>();
    }

    private void Update()
    {
    }

    public void BatteryEffect()
    {
        faceAnimator.SetTrigger(HappyTrigger);
        effects[0].Play();
        AudioManager.Instance.Play("Battery");
    }

    public void CanEffect()
    {
        faceAnimator.SetTrigger(ScoreTrigger);
        effects[1].Play();
        AudioManager.Instance.Play("Can_Pickup");
    }
    
    public void DustEffect()
    {
        dustEffect.Play();
    }
    
    public void StopDustEffect()
    {
        dustEffect.Stop();
    }

    public void DamageEffect()
    {
        faceAnimator.SetTrigger(SadTrigger);
        effects[2].Play();
        AudioManager.Instance.Play("Crash");
    }
    
    public void FootStepSound()
    {
        if(Movement.Instance.isGrounded)
            AudioManager.Instance.Play("StepSound");
    }

    public void SlideSpark()
    {
        effects[3].Play();
    }
    public void StopSlideSpark()
    {
        effects[3].Stop();
    }

    public void PlayGodSparkles()
    {
        effects[4].Play();
    }
    
    public void StopGodSparkles()
    {
        effects[4].Stop();
    }
    
    public void PlayPPP()
    {
        effects[5].Play();
    }

    public void Play2X()
    {
        effects[6].Play();
    }

}
