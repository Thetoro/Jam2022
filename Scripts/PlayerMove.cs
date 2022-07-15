using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float _speed;
    private Rigidbody2D _rb;
    private PlayerMoveControl playerControl;

    private void Awake() 
    {
        playerControl = new PlayerMoveControl();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
