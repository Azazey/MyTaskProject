using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed;
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Animator _animator;

    private const string RunningParameter = "IsRunning";
    private const float DeltaInput = 0.1f;
    
    private void Update()
    {
        Moving();
        ModelTurnOnMove();
    }

    private void ModelTurnOnMove()
    {
        Vector3 rotate = new Vector3(_joystick.Direction.x, 0f, _joystick.Direction.y);
        
        if (rotate.magnitude >= DeltaInput)
        {
            transform.rotation = Quaternion.LookRotation(rotate);
        }
    }

    private void Moving()
    {
        float horizontal = _joystick.Horizontal;
        float vertical = _joystick.Vertical;
        Vector3 move = new Vector3(horizontal, 0, vertical);
        if (move.magnitude >= DeltaInput)
        {
            _controller.Move(move * _speed * Time.deltaTime);
            _animator.SetBool(RunningParameter, true );
        }
        else
        {
            _animator.SetBool(RunningParameter, false);
        }
    }
}
