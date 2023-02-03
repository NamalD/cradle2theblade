using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D playerRigidbody;

    // TODO: Bundle combat together
    [SerializeField]
    private Transform attackPoint;

    [FormerlySerializedAs("attackRange")]
    [SerializeField]
    private float attackRadius = 0.5f;

    private const int AttackDamage = 10;
    public int HeavyAttackCharges { get; private set; }

    [SerializeField]
    private LayerMask enemyLayers;

    // TODO: Bundle animation stuff together
    [SerializeField]
    private Animator animator;

    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    private static readonly int HeavyAttacking = Animator.StringToHash("HeavyAttacking");

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float jumpForce = 60f;
    
    private float _moveHorizontal;
    private float _moveVertical;
    private bool _isJumping;
    private bool _facingRight = true;

    // Start is called before the first frame update

    void Start()
    {
        _isJumping = false;
        HeavyAttackCharges = 0;
    }

    // Update is called once per frame

    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat(MoveSpeed, Math.Abs(_moveHorizontal));

        _moveVertical = Input.GetAxisRaw("Vertical");

        // TODO: Limit attacks
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            HeavyAttack();
        }
    }

    private void FixedUpdate()
    {
        if (_moveHorizontal is > 0f or < 0f)
        {
            playerRigidbody.AddForce(new Vector2(_moveHorizontal * moveSpeed, 0), ForceMode2D.Impulse);
        }

        if (_facingRight && _moveHorizontal < 0f || !_facingRight && _moveHorizontal > 0f)
        {
            Flip();
        }

        if (!_isJumping && _moveVertical > 0f)
        {
            playerRigidbody.AddForce(Vector2.up * jumpForce);
        }
    }

    private void Attack()
    {
        animator.SetTrigger(Attacking);
        
        var attackCollisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
        foreach (var collision in attackCollisions)
        {
            HeavyAttackCharges++;
            collision.GetComponent<Enemy>().Attack(AttackDamage);
        }
    }

    private void HeavyAttack()
    {
        animator.SetTrigger(HeavyAttacking);
        
        // TODO: Refactor repetition
        var attackCollisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayers);
        foreach (var collision in attackCollisions)
        {
            // TODO: Handle 0 charges (disable heavy attack?)
            collision.GetComponent<Enemy>().Attack(AttackDamage * HeavyAttackCharges);
        }

        HeavyAttackCharges = 0;
    }

    private void Flip()
    {
        var o = gameObject;
        var currentScale = o.transform.localScale;
        currentScale.x *= -1;
        o.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Colliding with {other.name}");
        _isJumping = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Stopped Colliding with {other.name}");
        _isJumping = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}