using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWheel : MonoBehaviour
    {
        public GameObject FRwheel;
        public GameObject FLwheel;
        public GameObject BLwheel;
        public GameObject BRwheel;
        private int layer_mask;
        public CommandServer CommandServer;

        private void Start()
        {
            layer_mask = LayerMask.GetMask("road");
        }

        private void FixedUpdate()
        {
            
        }

    public void wheelRunner() {
        CommandServer.frWheel = checkWheels(FRwheel);
        CommandServer.flWheel = checkWheels(FLwheel);
        CommandServer.brWheel = checkWheels(BRwheel);
        CommandServer.blWheel = checkWheels(BLwheel);
    }
        bool checkWheels(GameObject wheel)
        {
            Vector3 fwd = wheel.transform.up * -1;
            if (!Physics.Raycast(wheel.transform.position, fwd, 1, layer_mask))
            {
                return (true);
            }
            return (false);
        }
    }
