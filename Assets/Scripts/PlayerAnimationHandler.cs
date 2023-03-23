using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        GameManager.PlayerAttitude = PlayerAttitude.Idle;
    }

    private void Update()
    {
        switch (GameManager.PlayerAttitude)
        {
            case PlayerAttitude.Downward:
                _animator.SetFloat("iY",-1);
                break;
            case PlayerAttitude.Upward:
                _animator.SetFloat("iY",1);
                break;
            case PlayerAttitude.Leftward:
                _animator.SetFloat("iX",-1);
                break;
            case PlayerAttitude.Rightward:
                _animator.SetFloat("iX",1);
                break;
            case PlayerAttitude.Idle:
                _animator.SetFloat("iX",0);
                _animator.SetFloat("iY",0);
                break;
        }
    }
}
