using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public GameObject collisionControl;
	public GameObject snake;
	public GameObject camera;
	public float cameraChangeTime = 0.5f;
	float curTime;
    // Start is called before the first frame update
    void Start()
    {
		curTime = cameraChangeTime;
		int k = collisionControl.GetComponent<CollisionControl>().fieldSide/2;
        camera.transform.position = new Vector3(0, 0, k+1);
		
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime;
		if(curTime <= 0){
			if(Input.GetKey(KeyCode.C)) {
				camera.transform.position = new Vector3(0, 0, -camera.transform.position.z);
				camera.transform.Rotate(Vector3.up, 180);
				snake.GetComponent<SnakeControl>().cameraPos = (snake.GetComponent<SnakeControl>().cameraPos+2)%4;
				curTime = cameraChangeTime;
			}
		}
    }
}
