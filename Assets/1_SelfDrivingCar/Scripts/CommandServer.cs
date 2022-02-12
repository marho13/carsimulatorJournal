using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using UnityStandardAssets.Vehicles.Car;
using System;
using System.Security.AccessControl;


public class CommandServer : MonoBehaviour
{
	public CarRemoteControl CarRemoteControl;
	public Camera FrontFacingCamera;
	private SocketIOComponent _socket;
	public CarController _carController;
    public tireOffRoad tireText;
    public bool frWheel = false;
    public bool flWheel = false;
    public bool brWheel = false;
    public bool blWheel = false;
    public bool Collide = false;
    public bool onRoad = true;
    public bool resetEnv = false;
    public int straightCheckpoint = 0;
    public int nonStraightCheckpoint = 0;
    public CheckWheel checky;
    public checkpoint checkpoint;
    public float reward = 0.0f;
    public int offRoadInt = 0;

    // Use this for initialization
    void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
		_socket.On("manual", onManual);
        _socket.On("ready", onReady);
		_carController = CarRemoteControl.GetComponent<CarController>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
       
	}

	void OnOpen(SocketIOEvent obj)
	{
		Debug.Log("Connection Open");
		EmitTelemetry(obj);
	}

	// 
	void onManual(SocketIOEvent obj)
	{
        checky.wheelRunner();
        tireText.changeText("front left", flWheel);
        tireText.changeText("front right", frWheel);
        tireText.changeText("back left", blWheel);
        tireText.changeText("back right", brWheel);

        Debug.Log("Manual");
        EmitTelemetry (obj);
	}

	void OnSteer(SocketIOEvent obj)
	{
		JSONObject jsonObject = obj.data;
		//    print(float.Parse(jsonObject.GetField("steering_angle").str));
		CarRemoteControl.SteeringAngle = float.Parse(jsonObject.GetField("steering_angle").str);
		CarRemoteControl.Acceleration = float.Parse(jsonObject.GetField("throttle").str);
        //Make the color represent if the tires are off road
        checky.wheelRunner();
        tireText.changeText("front left", flWheel);
        tireText.changeText("front right", frWheel);
        tireText.changeText("back left", blWheel);
        tireText.changeText("back right", brWheel);

        Collide = checkpoint.stuckCheck();
        wheelCheck();
        Debug.Log(onRoad);
        EmitTelemetry(obj);
	}

	void EmitTelemetry(SocketIOEvent obj)
	{
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			print("Attempting to Send...");
			// send only if it's not being manually driven
			if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
				_socket.Emit("telemetry", new JSONObject());
			}
			else {
				// Collect Data from the Car
				Dictionary<string, string> data = new Dictionary<string, string>();
				data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
				data["throttle"] = _carController.AccelInput.ToString("N4");
				data["speed"] = _carController.CurrentSpeed.ToString("N4");
				data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
                data["reward"] = reward.ToString();
                data["checkpointStraight"] = straightCheckpoint.ToString();
                data["checkpointNonStraight"] = nonStraightCheckpoint.ToString();
                data["onRoad"] = onRoad.ToString();
                data["resetEnv"] = resetEnv.ToString();
                _socket.Emit("telemetry", new JSONObject(data));

                if (resetEnv) {
                    Debug.Log("reset");
                    resetCheckpoints();
                    reward = 0.0f;
                    Time.timeScale = 0.0f;
                }
                resetEnv = false;
            }
		});
	}

    public void rewardCheckpoint()
    {
        nonStraightCheckpoint++;
        reward += 7.0f;
    }

    public void rewardStraight()
    {
        straightCheckpoint++;
        reward += 2.0f;
    }

    void resetCheckpoints()
    {
        straightCheckpoint = 0;
        nonStraightCheckpoint = 0;
    }

    void stillReward()
    {
        reward -= 0.001f;
    }

    void offRoad()
    {
        reward -= 0.01f;
    }

    void episodeReward()
    {
        reward -= 0.5f;
    }

    void onReady(SocketIOEvent obj)
    {
        Time.timeScale = 1.0f;
    }

    void wheelCheck()
    {
        if (frWheel | flWheel | blWheel | brWheel)
        {
            offRoadInt++;
            onRoad = false;

            if (offRoadInt >= 300)
            {
                this.episodeReward();
                checkpoint.reset();
                resetEnv = true;
                offRoadInt = 0;
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
            }

            else
            {
                this.offRoad();
            }
        }

        else
        {

            offRoadInt = 0;
            onRoad = true;

            if (Collide)
            {
                this.episodeReward();
                Collide = false;
                checkpoint.reset();
                resetEnv = true;
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
                _carController.Move(0.0f, 0.0f, 0.0f, 0.0f);
            }

            else
            {
                this.stillReward();
            }
        }
    }
}