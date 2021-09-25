using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeControl : MonoBehaviour
{
	public float timeOfStep = 0.7f;
	float curTime, pressColdown;
	
	public GameObject collisionController;
	public GameObject gameOverControl;
	
	public GameObject snakeBodyPart;
	CycledList<GameObject> snake;
	Vector3 direction;
	public int cameraPos=2;
	
    // Start is called before the first frame update
    void Start()
    {
		int k = collisionController.GetComponent<CollisionControl>().fieldSide/2;
        curTime = timeOfStep;
		pressColdown = 0;
		GameObject gO1 = Instantiate (snakeBodyPart, new Vector3(-k+1,-k,-k), Quaternion.identity); //head
		GameObject gO2 = Instantiate (snakeBodyPart, new Vector3(-k,-k,-k), Quaternion.identity);  //tail
		snake = new CycledList<GameObject>(gO2, gO1);
		direction = new Vector3(1,0,0);
    }

    // Update is called once per frame
    void Update()
    {
		pressColdown -= Time.deltaTime;
		if(pressColdown <= 0){
			if(Input.GetKey(KeyCode.W)) {
				changeDirection(new Vector3(0,1,0));
			}
			if(Input.GetKey(KeyCode.S)) {
				changeDirection(new Vector3(0,-1,0));
			}
			if(Input.GetKey(KeyCode.A)) {
				changeDirection(new Vector3(-1,0,0));
			}
			if(Input.GetKey(KeyCode.D)) {
				changeDirection(new Vector3(1,0,0));
			}
		}
		
        if(curTime>0){
			curTime -= Time.deltaTime;
		} else {
			moveSnake();
			curTime = timeOfStep;
		}
    }
	
	void changeDirection(Vector3 directionButton){
		Debug.Log(""+direction.x+" "+direction.y+" "+direction.z+" ");
		
		if(directionButton.y!=0){ //W or S
			if(direction.y==0){
				direction = directionButton;
			} else {
				direction = directionButton==new Vector3(0,1,0) ? new Vector3(0,0,1) : new Vector3(0,0,-1);
				//Quaternion.Euler(90,0,0) * directionButton;
			}
		} else { //A or D
			if(direction.x==0){
				direction = directionButton;
			} else {
				direction = direction==directionButton ? new Vector3(0,0,-1) : new Vector3(0,0,1);
			}
		}
		calcWithCamera();
		pressColdown = timeOfStep;
	}
	
	void calcWithCamera(){
		int x = (int)direction.x;
		int y = (int)direction.y;
		int z = (int)direction.z;
		x = (cameraPos&1)==0 ? (1-cameraPos)*x : (2-cameraPos)*z;
		z = (cameraPos&1)==0 ? (1-cameraPos)*z : (2-cameraPos)*x;
		direction = new Vector3(x,y,z);
	}
	
	public void incSize(){
		GameObject gO = Instantiate (snakeBodyPart, new Vector3(-100,-100,-100), Quaternion.identity);  //+tail
		snake.addEl(gO);
	}
	
	void moveSnake(){
		Vector3 tailFrom = snake.getCurrent().transform.position;
		Vector3 headTo = snake.getNext().transform.position+direction;
		if(collisionController.GetComponent<CollisionControl>().collisionCheck(headTo, tailFrom)){
			Debug.Log("GameOver");
			//collision reaction - game end
			gameOverControl.GetComponent<ScenesSwitcher>().gameOver();
		} else {
			snake.getCurrent().transform.position = headTo;
			snake.prev();
		}
	}
	
	class Node<T>{
		public Node<T> next, prev;
		public T val;
		
		public Node(Node<T> next, Node<T> prev, T val){
			this.next = next;
			this.prev = prev;
			this.val = val;
		}
	}
	
	class CycledList<T>{
		Node<T> current;
		
		public CycledList(T val1, T val2){
			current = new Node<T>(null, null, val1);
			current.prev = current.next = new Node<T>(current, current, val2);
		}
		
		public void addEl(T val){
			current.prev = current.prev.next = new Node<T>(current, current.prev, val);
		}
		
		public void prev(){
			current = current.prev;
		}
		
		public T getCurrent(){
			return current.val;
		}
		
		public T getPrev(){
			return current.prev.val;
		}
		
		public T getNext(){
			return current.next.val;
		}
	}
}
