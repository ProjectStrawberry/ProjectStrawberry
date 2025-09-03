using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip attack_1;
    [SerializeField] private AudioClip attack_2;
    [SerializeField] private AudioClip rangeAttack;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip doubleJump;
    [SerializeField] private AudioClip damaged;

    public void FootStep()
    {
        SoundManager.PlayClip(footStep);
    }

    public void Attack_1()
    {
        //SoundMnager.PlayClip(attack_1);
    }

    public void Attack_2()
    {
        //SoundMnager.PlayClip(attack_2);
    }

    public void RangeAttack()
    {
        //SoundMnager.PlayClip(rangeAttack);
    }

    public void Jump()
    {
        //SoundMnager.PlayClip(jump);
    }

    public void DoubleJump()
    {
        //SoundMnager.PlayClip(doubleJump);
    }

    public void Damaged()
    {
        //SoundMnager.PlayClip(damaged);
    }

}
