using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 运动基础 : MonoBehaviour
{
    public Animator animator_cat;
    public AudioSource audioSource_cat;

    public GameObject prefab;

    public List<GameObject> prefabs;
    bool _rightIsPressed;
    bool _leftIsPressed;
    bool _forwardIsPressed;
    bool _backwardIsPressed;
    public float speed;

    public Rigidbody rb;

    private void Update()
    {
        ReadInput();
        MoveByInput();
    }

    void ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _leftIsPressed = true;
        if (Input.GetKeyUp(KeyCode.A))
            _leftIsPressed = false;

        if (Input.GetKeyDown(KeyCode.D))
            _rightIsPressed = true;
        if (Input.GetKeyUp(KeyCode.D))
            _rightIsPressed = false;

        if (Input.GetKeyDown("w"))
            _forwardIsPressed = true;
        if (Input.GetKeyUp("w"))
            _forwardIsPressed = false;

        if (Input.GetKeyDown("s"))
            _backwardIsPressed = true;
        if (Input.GetKeyUp("s"))
            _backwardIsPressed = false;
    }

    void MoveByInput()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);
        if (_rightIsPressed)
        {
            moveVector += new Vector3(1, 0, 0);
        }
        if (_leftIsPressed)
        {
            moveVector += new Vector3(-1, 0, 0);
        }
        if (_forwardIsPressed)
        {
            moveVector += new Vector3(0, 0, 1);
        }
        if (_backwardIsPressed)
        {
            moveVector += new Vector3(0, 0, -1);
        }

        moveVector.Normalize();
        rb.velocity = moveVector * speed;
        //transform.position += moveVector * speed * Time.deltaTime;
    }


























    private void OnTriggerEnter(Collider other)
    {
        animator_cat.SetTrigger("sound");
        audioSource_cat.Play();

        foreach (var p in prefabs)
        {
            Instantiate(p, transform.position, Quaternion.identity);
        }

        bool b = 1 > 9;//false true

        if (!b)
            Debug.Log("1大于9");
        else
            Debug.Log("1不大于9");
    }

    private void OnTriggerExit(Collider other)
    {
        // Destroy(gameObject);
        Instantiate(prefab, transform.position, Quaternion.identity);

        for (int i = -10; i < 0; i++)
        {
            var pos = transform.position + Vector3.up * i * 0.5f;
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
