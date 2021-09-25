using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControl : MonoBehaviour
{
	public GameObject snake;
	public GameObject infoControl;
	
	public GameObject food;
	public GameObject border;
	public int fieldSide=10;
	int k;
	
	bool[,,] cellsStatus;
	GameObject currentFood;
    // Start is called before the first frame update
    void Start()
    {
		k = fieldSide/2;
		cellsStatus = new bool[fieldSide, fieldSide, fieldSide];
		for(int i=0; i<fieldSide; i++)
			for(int j=0; j<fieldSide; j++)
				for(int l=0; l<fieldSide; l++)
					cellsStatus[i,j,l]=true;
		cellsStatus[0,0,0]=cellsStatus[1,0,0]=false; //занято змейкой
		
        for(int i=0; i<6; i++){
			GameObject bord = Instantiate (border, defineVector(i), Quaternion.Euler(0, 90*((i/2)&1), 45*((i/2)&2)));
			bord.transform.localScale = new Vector3(1, 1f*fieldSide+2, 1f*fieldSide+2);
		}
		
		infoControl.GetComponent<InfoControl>().clearPoints();  //сброс старого результата
		setFood(); //размещаем первую еду
    }
	
	Vector3 defineVector(int i){
		float x = i/2==0 ? (k+1)*((2*i)%4-1) : 0;
		float y = i/2==2 ? (k+1)*((2*i)%4-1) : 0;
		float z = i/2==1 ? (k+1)*((2*i)%4-1) : 0;
		return new Vector3(x,y,z);
	}
	
    // Update is called once per frame
    public bool collisionCheck(Vector3 headTo, Vector3 tailFrom) {
		if(headTo==tailFrom) return false; //змея зациклена: голова в хвост
		if(headTo.x > k || headTo.x < -k || headTo.y > k || headTo.y < -k || headTo.z > k || headTo.z < -k) return true; //выход за границы поля
		if(tailFrom.x != -100) //координаты -100 - у прирастающего блока хвоста, иначе необходимо осводить клетку с хвостом
			cellsStatus[(int)tailFrom.x+k, (int)tailFrom.y+k, (int)tailFrom.z+k] = true;
		if(headTo==currentFood.transform.position){ //змейка ест
			snake.GetComponent<SnakeControl>().incSize();
			Destroy(currentFood);
			infoControl.GetComponent<InfoControl>().addPoint();
			infoControl.GetComponent<InfoControl>().showPoints();
			setFood();
			return false;
		} else { //змейка просто перемещается
			if(cellsStatus[(int)headTo.x+k, (int)headTo.y+k, (int)headTo.z+k]){ //свободна ли клетка для перемещения?
				cellsStatus[(int)headTo.x+k, (int)headTo.y+k, (int)headTo.z+k] = false;
				return false;
			} else return true;
		}
		Debug.Log("You mustn't be here!");
		//? - здесь вас быть не должно!
        return true;
    }
	
	void setFood(){
		int x,y,z;
		do{
			x = Random.Range (0, fieldSide);
			y = Random.Range (0, fieldSide);
			z = Random.Range (0, fieldSide);
		} while(!cellsStatus[x,y,z]); //пока случайная клетка не окажется свободной
		
		currentFood = Instantiate(food, new Vector3(x-k, y-k, z-k), Quaternion.identity);
		cellsStatus[x,y,z] = false;
	}
}
