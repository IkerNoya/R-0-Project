using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] Transform player;
    CameraShake shake;
    private void Start()
    {
        shake = GetComponent<CameraShake>();
    }
    void LateUpdate() {
        if (player != null)
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z) + shake.localPos;
    }
}
