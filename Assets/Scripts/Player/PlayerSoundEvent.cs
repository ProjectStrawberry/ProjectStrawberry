using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEvent : MonoBehaviour
{
    [SerializeField] private AudioClip footStep_1;
    [SerializeField] private AudioClip footStep_2;
    [SerializeField] private AudioClip attack_1;
    [SerializeField] private AudioClip attack_2;
    [SerializeField] private AudioClip rangeAttack;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip doubleJump;
    [SerializeField] private AudioClip damaged;

    public void FootStep_1()
    {
        SoundManager.PlayClip(footStep_1);
    }

    public void FootStep_2()
    {
        SoundManager.PlayClip(footStep_2);
    }

    public void Attack_1()
    {
        SoundManager.PlayClip(attack_1);
    }

    public void Attack_2()
    {
        SoundManager.PlayClip(attack_2);
    }

    public void RangeAttack()
    {
        SoundManager.PlayClip(rangeAttack);
    }

    public void Jump()
    {
        SoundManager.PlayClip(jump);
    }

    public void DoubleJump()
    {
        SoundManager.PlayClip(doubleJump);
    }

    public void Damaged()
    {
        SoundManager.PlayClip(damaged);
    }

}
