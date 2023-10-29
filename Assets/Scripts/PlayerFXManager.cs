using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
//Script Made By Daniel Alvarado
public class PlayerFXManager : MonoBehaviour
{
    [SerializeField] private Animator _faceAnimator;
    [SerializeField] private List<ParticleSystem> effects;
    [SerializeField] private VisualEffect dustEffect;
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
    
    public void BatteryEffect()
    {
        _faceAnimator.SetTrigger("HappyTrigger");
        effects[0].Play();
        AudioManager.Instance.Play("Battery");
    }

    public void CanEffect()
    {
        _faceAnimator.SetTrigger("ScoreTrigger");
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
        _faceAnimator.SetTrigger("SadTrigger");
        effects[2].Play();
        AudioManager.Instance.Play("Crash");
    }
    
    public void FootStepSound()
    {
        if(Movement.Instance.isGrounded)
            AudioManager.Instance.Play("StepSound");
    }

}
