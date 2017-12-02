using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBall : MonoBehaviour
{

    private GameObject parchment;
    private bool isActive;
    private List<GameObject> ListOfBalls;

	void Start ()
    {
        ListOfBalls = new List<GameObject>();
        ListOfBalls.Add(GameObject.Find("StandardBall"));
        ListOfBalls.Add(GameObject.Find("FireBall"));
        ListOfBalls.Add(GameObject.Find("EyeBall"));
        //Debug.Log("list count : " + ListOfBalls.Count);
        ChangeTheBall(0); //activate the standard ball

        parchment = gameObject.transform.Find("Parchment").gameObject;
        isActive = parchment.activeSelf;
        if (isActive == true)
        {
            //this will deactivate the UI at the start of the game
            OpenClosePanel();
        }
    }

    public void OpenClosePanel()
    {
        isActive = !isActive;
        parchment.SetActive(isActive);
    }

    public void ChangeTheBall(int changeTo)
    {
        Debug.Log("change to : " + changeTo);
        for(int i = 0; i < ListOfBalls.Count; i++)
        {
            if(i == changeTo)
            {
                ListOfBalls[i].SetActive(true);
                Debug.Log("true");
            }
            else
            {
                ListOfBalls[i].SetActive(false);
                Debug.Log("false");
            }
        }
    }
}
