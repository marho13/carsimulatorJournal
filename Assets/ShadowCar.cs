using UnityEngine;

public class ShadowCar : MonoBehaviour {
    public GameObject segCar;
    public GameObject vehicle;
    public Vector3 offset;
    public Vector3 that;


    public void Start() {
        Debug.Log(typeof(GameObject).GetProperties());
        offset = new Vector3(0.0f, 0.0f, -1000.0f);
    }

    public void Update()
    {
        moveVehicle();

    }
    // Use this for initialization

    public void moveVehicle() {
        segCar.transform.position = vehicle.transform.position + offset;
        segCar.transform.rotation = vehicle.transform.rotation;
    }

    //public void moveVehiclePub() {
    //    moveVehicle();
    //}
}
