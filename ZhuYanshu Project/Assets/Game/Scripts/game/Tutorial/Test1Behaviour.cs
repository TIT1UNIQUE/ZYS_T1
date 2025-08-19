using UnityEngine;


public class Test1Behaviour : MonoBehaviour
{
    [Range(1,5)]
    public float speed;
    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Vector3.up * speed * Time.deltaTime;
    }
}