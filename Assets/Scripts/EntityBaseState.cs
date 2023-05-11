using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBaseState : StateMachineBehaviour
{
    private Entity _entity;
    public Entity GetEntity(Animator animator)
    {
        if (_entity == null)
        {
            _entity = animator.GetComponentInParent<Entity>();
        }
        return _entity;
    }
}
