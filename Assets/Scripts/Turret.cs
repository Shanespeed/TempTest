using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int turretCost = 50;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackSpeed = 1f;
    private float attackTimer;

    private GameObject currentTarget;
    private float tempTargetDistance;
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameManager gameManager;
    private SpriteRenderer sr;
    private CircleCollider2D attackRadius;
    private CircleCollider2D placeRadius;
    private bool attackOnCooldown = true;
    private bool hasBeenPlaced = false;
    private bool canBePlaced = true;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sr = GetComponent<SpriteRenderer>();
        placeRadius = GetComponent<CircleCollider2D>();
        attackRadius = GetComponentInChildren<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start() =>
        attackTimer = attackSpeed;

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenPlaced)
        {
            PlacingModeUpdate();
            return;
        }

        TargetFirst();

        if (attackOnCooldown)
            attackTimer -= Time.deltaTime;
        
        if (attackTimer <= 0)
        {
            attackOnCooldown = false;
            Shoot();
        }

    }

    private void TargetFirst()
    {
        float currentTargetDistance;

        if (currentTarget == null)
            currentTargetDistance = 0;
        else
            currentTargetDistance = currentTarget.GetComponent<Enemy>().ReturnDistance();

        // Give all enemies a progress on the path state. Target enemy that has highest progress value
        foreach (GameObject enemy in enemiesInRange)
        {
            float distance = enemy.GetComponent<Enemy>().ReturnDistance();

            if (distance > currentTargetDistance)
            {
                tempTargetDistance = distance;
                currentTarget = enemy;
            }
        }
    }

    private void Shoot()
    {
        if (currentTarget == null)
            return;

        Debug.Log("BANG");
        attackTimer = attackSpeed;
        attackOnCooldown = true;
        currentTarget.GetComponent<Enemy>().TakeDamage(damage);
    }

    private void PlacingModeUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position += new Vector3(0, 0, 10);

        if (canBePlaced && Input.GetMouseButtonDown(0))
        {
            hasBeenPlaced = true;
            sr.color = Color.white;
            canBePlaced = false;
            gameManager.currency -= turretCost;
        }
        else if (Input.GetMouseButtonDown(1))
            Destroy(gameObject);
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If turret tries to be placed on invalid ground
        if (!hasBeenPlaced)
        {
            canBePlaced = false;
            sr.color = Color.red;
        }

        // If an enemy enters the attack radius, add it to the enemies in range list
        if (col.gameObject.CompareTag("Enemy"))
            enemiesInRange.Add(col.gameObject);
    }

    
    private void OnTriggerExit2D(Collider2D col)
    {
        // If turret is not on invalid ground anymore
        if (!hasBeenPlaced)
        {
            canBePlaced = true;
            sr.color = Color.white;
        }

        // If enemies have passed turret range or have been killed, delete them from the enemies in range list
        if (col.gameObject.CompareTag("Enemy"))
            enemiesInRange.Remove(col.gameObject);
    }
}
