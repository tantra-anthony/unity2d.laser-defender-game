using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 15.0f;
    public float padding = 1.0f;
    public float projectileSpeed;
    public float fireRate = 0.2f;
    public float health = 250f;
    public GameObject projectile;
    public AudioClip fireSound;

    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        //first value is between 0 and 1, relative to the pos of the screen
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
	}

    private void FireProjectile() {
        GameObject beam = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, projectileSpeed, 0f);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("FireProjectile", 0.00001f, fireRate);
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke("FireProjectile");
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            //transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            //transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
            transform.position += Vector3.left * speed * Time.deltaTime;
        } 

        //restrict to game space
        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile) {
            missile.Hit();
            health -= missile.GetDamage();
            if (health <= 0) {
                Destroy(gameObject);
                LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
                man.LoadLevel("Win Screen");
            }
        }
    }
}
