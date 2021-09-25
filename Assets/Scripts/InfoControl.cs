using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoControl : MonoBehaviour
{
	public GameObject pointsTextField;
	public static int points;
	
	void Start(){
		showPoints();
	}
	
	public void clearPoints(){
		points=0;
	}
	
    public void addPoint(){
		points++;
	}
	
	public void showPoints(){
		pointsTextField.GetComponent<Text>().text=""+points;
	}
}
