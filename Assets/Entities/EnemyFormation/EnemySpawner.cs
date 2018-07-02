using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    public float speed = 5.0f;
    public float spawnDelay = 1.0f;

    private bool movingRight = false;
    private float xmin;
    private float xmax;

	// Use this for initialization
	void Start () {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftBoundary = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distanceToCamera));
        Vector3 rightBoundary = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, distanceToCamera));
        xmin = leftBoundary.x;
        xmax = rightBoundary.x;
        SpawnUntilFull();
	}

    private void SpawnEnemies() {
        foreach (Transform child in transform) {
            //put the new enemy inside parent gameObject
            //instantiate at their actual parent origin
            GameObject enemy = Instantiate(enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = child;
        }
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {

        if (movingRight) {
            transform.position += Vector3.right * speed * Time.deltaTime;
        } else {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + 0.5f * width;
        float leftEdgeOfFormation = transform.position.x - 0.5f * width;
        if (rightEdgeOfFormation > xmax) {
            movingRight = false;
        } else if (leftEdgeOfFormation < xmin) {
            movingRight = true;
        }

        if (AllMembersDead()) {
            SpawnUntilFull();
        }
	}

    private void SpawnUntilFull() {
        Transform freePosition = NextFreePosition();
        if (freePosition) {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
        }

        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    private bool AllMembersDead() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount > 0) {
                return false;
            }
        }
        return true;
    }

    private Transform NextFreePosition() {
        foreach (Transform childPositionGameObject in transform) {
            if (childPositionGameObject.childCount <= 0) {
                return childPositionGameObject;
            }
        }
        return null;
    }
}
