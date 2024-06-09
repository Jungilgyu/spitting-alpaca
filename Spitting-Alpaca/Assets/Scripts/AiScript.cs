using System.Collections;
using UnityEngine;

public class AiScript : MonoBehaviour
{
    public float finalSpeed;
    public float speed = 3.5f;
    public float runSpeed = 5f;
    public float rotateSpeed = 3f;
    public float directionChangeInterval = 2f;
    public float jumpHeight = 2.3f;
    public float gravity = -9.81f;

    public bool isLive = true;
    public float reactionDelay = 1f;  // ���� ���� �ð� �߰�

    private Rigidbody rb;
    private Vector3 movement;
    private float nextDirectionChangeTime;
    private bool isMoving = true;
  
   
 
    

    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        nextDirectionChangeTime = Time.time + directionChangeInterval;
        ChangeDirection(); // reset direction
    }

    void Update()
    {
        if (!isLive)
        {
            isMoving = false;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            return;
        }
        if (Time.time >= nextDirectionChangeTime)
        {
            float actionChance = Random.value;
            if (actionChance <= 0.1) // 10%Ȯ���� ����
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                Jump(); // ���� ����
                
            }

            else if (actionChance <= 0.2) 
            {
                if (isMoving)
                {
                    isMoving = false;
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isRunning", false);
                    nextDirectionChangeTime += Random.Range(0, 3f); // stop 0~3 seconds
                }
                else
                {
                    ChangeDirection();
                    isMoving = true;
                    animator.SetBool("isRunning", true);
                }
            }

            else if (actionChance <= 0.3)
            {
                Eat();
            }

            else
            {
                if (isMoving)
                {
                    isMoving = false;
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isWalking", false);
                    nextDirectionChangeTime += Random.Range(0, 3f); // stop 0~3 seconds
                }
                else
                {
                    ChangeDirection();
                    isMoving = true;
                    animator.SetBool("isWalking", true);
                }
            }
            nextDirectionChangeTime += directionChangeInterval;
        }
    }

    void FixedUpdate()
    {
        if(!isMoving || !isLive)
        {
            return;
        }

        if(animator.GetBool("isWalking") == true)
        {
            Move();
        }
        else if (animator.GetBool("isRunning") == true)
        {
            Run();
            
        }
        
    }

    void ChangeDirection(Vector3? specificDirection = null)
    {
        // check specificDirection
        if (specificDirection.HasValue)
        {
            movement = specificDirection.Value.normalized;
        }
        else
        {
            float horizontalMove = Random.Range(-1f, 1f);
            float verticalMove = Random.Range(-1f, 1f);
            movement = new Vector3(horizontalMove, 0, verticalMove).normalized;
        }
    }

    void Move()
    {
        Vector3 movePosition = transform.position + movement * speed * Time.fixedDeltaTime;
        rb.MovePosition(movePosition);

        if (movement != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }

    void Jump()
    {
        if (!animator.GetBool("isJumping")) // �̹� ���� ���� �ƴ� ���� ���� ����
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.Impulse);
            animator.SetBool("isJumping", true);
        }

    }

    void Run()
    {
        Vector3 movePosition = transform.position + movement * runSpeed * Time.fixedDeltaTime;
        rb.MovePosition(movePosition);

        if (movement != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }

    void Eat()
    {
        animator.Play("Eat", -1, 0f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Grass"))
            {
                Destroy(hitCollider.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5) // �ٴڰ� �浹�ߴٸ�
        {
            animator.SetBool("isJumping", false); // ���� ���� ����
            animator.SetBool("isRunning", false);
        }
        else
        {
            StartCoroutine(DelayedDirectionChange()); // �ڷ�ƾ�� ����Ͽ� ������ ���� ���� ����
        }
    }

    IEnumerator DelayedDirectionChange()
    {
        isMoving = false; // �̵� ����
        animator.SetBool("isWalking", false); // �ȴ� �ִϸ��̼� ��Ȱ��ȭ
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(reactionDelay); // reactionDelay ��ŭ ���

        Vector3 newDirection = -movement; // �浹 �ݴ� �������� �� ���� ����
        ChangeDirection(newDirection);

        isMoving = true; // �̵� �簳
        animator.SetBool("isWalking", true); // �ȴ� �ִϸ��̼� Ȱ��ȭ

        // ���� ���� ��������� �ð� ����
        nextDirectionChangeTime = Time.time + Random.Range(1f, 3f);
    }
}
