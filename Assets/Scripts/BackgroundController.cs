using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 basePos;
    private Vector3 currentPos;
    private bool move;
    void Start()
    {
        basePos = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPos = player.transform.position;
        Vector3 newBackgroundPos = transform.position;
        
        if (currentPos.x >= basePos.x + 18)
        {
            newBackgroundPos.x += 18;
            basePos.x += 18;
            move = true;
        }else if (currentPos.x <= basePos.x - 18)
        {
            newBackgroundPos.x -= 18;
            basePos.x -= 18;
            move = true;
        }

        if (currentPos.y >= basePos.y + 18)
        {
            newBackgroundPos.y += 18;
            basePos.y += 18;
            move = true;
        }else if (currentPos.y <= basePos.y - 18)
        {
            newBackgroundPos.y -= 18;
            basePos.y -= 18;
            move = true;
        }
        if (move)
        {
            transform.position = newBackgroundPos;
            move = false;
        }
    }
}
