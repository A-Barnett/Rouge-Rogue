using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Camera cam;
    private float origZPos;
    [SerializeField] private float sensitivity;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;
    private Vector3 targetPos;
    void Start()
    {
        cam = GetComponent<Camera>();
        origZPos = transform.position.z;
    }
    void FixedUpdate()
    {
        Vector3 playerpos = player.transform.position;
        
        if (player.transform.position.x < transform.position.x-offsetX)
        {
            playerpos.x += offsetX;
            targetPos.x = Mathf.Lerp(transform.position.x,playerpos.x, Time.deltaTime*sensitivity);
            
        }else if (player.transform.position.x > transform.position.x+offsetX)
        {
            playerpos.x -= offsetX;
            targetPos.x = Mathf.Lerp(transform.position.x,playerpos.x, Time.deltaTime*sensitivity);
        }
        if (player.transform.position.y < transform.position.y-offsetY)
        {
            playerpos.y += offsetY;
            targetPos.y = Mathf.Lerp(transform.position.y,playerpos.y, Time.deltaTime * sensitivity);
        }else if (player.transform.position.y > transform.position.y+offsetY )
        {
            playerpos.y -= offsetY;
            targetPos.y = Mathf.Lerp(transform.position.y,playerpos.y, Time.deltaTime * sensitivity);
        }
        targetPos.z = origZPos;
        transform.position = targetPos;
    }
}