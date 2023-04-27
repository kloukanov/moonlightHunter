using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateBase : StateMachineBehaviour
{
    private Player _player;
    public Player GetPlayer(Animator animator)
    {
        if (_player == null)
        {
            _player = animator.GetComponentInParent<Player>();
        }
        return _player;
    }
}
