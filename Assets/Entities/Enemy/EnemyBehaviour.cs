using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    public GameObject enemyProjectile;

    public float health = 150f;
    public float shootingFrequency = 1f; //per second
    public float projectileSpeed = 5f;
    public int scoreValue = 150;
    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    private void Start() {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();

        if (missile) {
            missile.Hit();
            health -= missile.GetDamage();
            if (health <= 0) {
                Death();
            }

        }
    }

    private void Death() {
        Destroy(gameObject);
        Debug.Log("enemy destroyed");
        scoreKeeper.Score(scoreValue);
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
    }

    private void Update() {
        float probability = Time.deltaTime * shootingFrequency;
        if (Random.value < probability) {
            Fire();
        }


    }

    private void Fire() {
        GameObject beam = Instantiate(enemyProjectile, transform.position, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
}
