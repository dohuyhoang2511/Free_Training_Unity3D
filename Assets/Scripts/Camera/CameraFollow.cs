using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;
    private float newY;
    [SerializeField] private float speedFollow = 5f;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followPos = target.position + offset;
        RaycastHit hit;
        if (Physics.Raycast(target.position, Vector3.down, out hit, 2.5f))
        {
            newY = Mathf.Lerp(newY, hit.point.y, speedFollow * Time.deltaTime);
        }
        else
        {
            newY = Mathf.Lerp(newY, target.position.y, speedFollow * Time.deltaTime);
        }

        followPos.y = offset.y + newY;
        transform.position = followPos;
    }
}
