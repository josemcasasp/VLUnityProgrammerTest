using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    public float damage;
    public float speed = 0.5f;
    public bool canStartRolling = true;
    public int health;
    public bool safetyPos = false;
    public int steps = 0;

    bool grounded = false;
    Rigidbody rb;

    public enum Enemies { SimpleCube = 0, SmallJumpingCube, ZigZagCube, TitanCube }
    public Enemies enemyType;

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();

        SafetyPosToSpawn();

        //Initial face to player
        transform.LookAt(new Vector3(0, transform.localScale.y / 2, 0));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        //Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, transform.localScale.x * 2)
        if (Physics.SphereCast(transform.localPosition, transform.localScale.x/2, (player.transform.position - transform.position).normalized, out hit))
        {
            if (hit.transform.tag == "Enemy")
            {
                float otherDistanceToPlayer = Vector3.Distance(player.transform.position, hit.transform.position);
                float myDistanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

                if (otherDistanceToPlayer < myDistanceToPlayer) safetyPos = false;

            }
            else
            {
                safetyPos = true;
            }
        }
        

        isGrounded();

        if (canStartRolling && safetyPos)
        {

            if (enemyType == Enemies.SmallJumpingCube && steps > Random.Range(5, 15) && grounded)
            {
                Jump();
                steps = 0;
                return;
            }

            //ToDO: Fix
            transform.LookAt(new Vector3(player.transform.position.x, transform.localScale.y, player.transform.position.z));
            StartCoroutine("RotateTo", (player.transform.position - transform.position).normalized);
            steps++;
            return;
        }

        /*isGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();         
        }
        */

        if (transform.position.y < transform.localScale.y / 2 && grounded)
        {
            rb.isKinematic = true; rb.useGravity = false;
        }

    }

    void Jump()
    {
        grounded = false;
        StopCoroutine("RotateTo");

        rb.isKinematic = false;
        rb.useGravity = true;
        Vector3 jumpVel = new Vector3(0, 5, 0);
        rb.velocity = rb.velocity + jumpVel;

        StartCoroutine("WaitAndRool", 1);
    }

    void isGrounded()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.y / 2))
        {
            if (hit.transform.tag == "Floor")
            {
                grounded = true;
                rb.useGravity = false;
                rb.isKinematic = true;
            }
            else
            {
                grounded = false;
            }
        }

    }

    IEnumerator WaitAndRool(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!canStartRolling) canStartRolling = true;
    }

    public void SetEnemy(int id)
    {
        switch (id)
        {
            case 0:
                enemyType = Enemies.SimpleCube;
                speed = 1f;
                health = 30;
                damage = 10;
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                transform.name = "SimpleCube";
                break;

            case 1:
                enemyType = Enemies.SmallJumpingCube;
                speed = 0.25f;
                health = 15;
                damage = 5;
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                transform.name = "SmallJumpingCube";
                break;

            case 2:
                enemyType = Enemies.ZigZagCube;
                speed = 1f;
                health = 15;
                damage = 5;
                transform.localScale = new Vector3(1, 1, 1);
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                transform.name = "ZigZagCube";
                break;

            case 3:
                enemyType = Enemies.TitanCube;
                speed = 20f;
                health = 500;
                damage = 100;
                transform.localScale = new Vector3(5, 5, 5);
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                transform.name = "Titan";
                break;

            default:
                break;
        }
    }

    void SafetyPosToSpawn()
    {
        float newPosXZ = Random.Range(-transform.localScale.y * 2, transform.localScale.y * 2);

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, transform.localScale.x * 2, transform.forward, out hit))
        {

            if (hit.transform.tag == "Enemy")
            {
                Debug.Log("He spawneado muy cerca.");
                //Debug.Log("Esta era mi pos: " + transform.position);
                transform.position = new Vector3(transform.position.x + newPosXZ, transform.localScale.y / 2, transform.position.z + newPosXZ);
                //Debug.Log("Esta es mi nueva pos: " + transform.position);
                safetyPos = true;
                return;
            }
        }

        else
        {
            safetyPos = true;
        }
    }

    public void OnDamage(int damage)
    {
        health -= damage;

        float color = GetComponent<MeshRenderer>().material.color.r;
        color -= 0.05f;

        GetComponent<MeshRenderer>().material.color = new Color(color, GetComponent<MeshRenderer>().material.color.g, GetComponent<MeshRenderer>().material.color.b);



        if (health <= 0) { GameManager.Instance.enemiesKilled++; Die(); }
    }

    IEnumerator RotateTo(Vector3 direction)
    {
        canStartRolling = false;

        float rollDecimal = 0;
        float rollAngle = 90;
        float halfEdge = transform.localScale.x / 2;

        Vector3 pointAround = transform.localPosition + (-transform.up * halfEdge) + (direction * halfEdge);
        Vector3 rollAxis = Vector3.Cross(Vector3.up, direction);

        Quaternion rotation = transform.localRotation;
        Quaternion endRotation = rotation * Quaternion.Euler(rollAxis * rollAngle);

        //multiplica por su escala para avanzar su posicion
        Vector3 endPosition = transform.localPosition + (direction * transform.localScale.x);

        float oldAngle = 0;

        //speed = lo que tarda en rodar
        while (rollDecimal < speed)
        {
            yield return new WaitForEndOfFrame();
            rollDecimal += Time.fixedDeltaTime;
            float newAngle = (rollDecimal / speed) * rollAngle;
            float rotateThrough = newAngle - oldAngle;
            oldAngle = newAngle;

            transform.RotateAround(pointAround, rollAxis, rotateThrough);

        }


        canStartRolling = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GameManager.Instance._playerHealth -= damage;
            if (GameManager.Instance._playerHealth <= 0) GameManager.Instance.VictoryLose(false);
            Die();
        }
    }

    void Die()
    {
        if (enemyType == Enemies.TitanCube) GameManager.Instance.VictoryLose(true);
        Destroy(gameObject);
    }
}

