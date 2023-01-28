using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRigidbody;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float jumpForce = 60f;

    private float _moveHorizontal;

    private float _moveVertical;

    private bool _isJumping;

    // Start is called before the first frame update
    void Start()
    {
        _isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (_moveHorizontal != 0)
            Debug.Log(_moveVertical);

        _moveVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (_moveHorizontal is > 0f or < 0f)
        {
            playerRigidbody.AddForce(new Vector2(_moveHorizontal * moveSpeed, 0), ForceMode2D.Impulse);
        }

        if (_moveVertical > 0f)
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce);
        }
    }
}