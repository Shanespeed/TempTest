using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    [SerializeField] private bool protection = false;
    [SerializeField] private int worth;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private AudioClip[] dieSFX;

    private GameManager gameManager;
    private AudioSource audioSource;
    private int currentGoal = 0;
    private float distance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager.activeEnemies.Add(gameObject);
        currentGoal = 0;
    }

    // Update is called once per frame
    void Update() =>
        Move();

    private void Move()
    {
        // Move towards the nearest path checkpoint
        transform.position = Vector2.MoveTowards(gameObject.transform.position, PathMaker.pathPositions[currentGoal], speed * Time.deltaTime);

        // Get next path checkpoint if close enough to the original target checkpoint
        if (Vector2.Distance(gameObject.transform.position, PathMaker.pathPositions[currentGoal]) < 0.1f)
            currentGoal++;

        // Sets distance to current goal;
        distance = currentGoal;
    }

    public float ReturnDistance()
    {
        return distance;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("End"))
        {
            gameManager.lives -= hp;
            Destroy(gameObject);
        }

        if (col.CompareTag("Attack"))
            Die();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        //AudioClip dieSound = dieSFX[Random.Range(0, dieSFX.Length + 1)];
        //audioSource.PlayOneShot(dieSound);
        gameManager.currency += worth;
        Destroy(gameObject);
    }

    private void OnDestroy() =>
        gameManager.activeEnemies.Remove(gameObject);
}
