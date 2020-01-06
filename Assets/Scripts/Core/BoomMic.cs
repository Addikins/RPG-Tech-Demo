using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class BoomMic : MonoBehaviour
    {
        GameObject player = null;
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            gameObject.transform.position = player.transform.position + Vector3.up;
        }
    }

}