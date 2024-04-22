using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private readonly int OpenTrigger = Animator.StringToHash("Open");

    [SerializeField] private Animator _animator;

    // Start is called before the first frame update
    public void Open()
    {
        _animator.SetTrigger(OpenTrigger);
    }
}
