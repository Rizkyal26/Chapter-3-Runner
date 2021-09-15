using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveController : MonoBehaviour
{
    [Header("Position")]
    public Transform player;
    public float horizontalOffset;

    [Header("Jump")]
    public float JumpAccel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = player.position.x + horizontalOffset;
        transform.position = newPosition;
    }
}
