using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform StartingPoint;
    [SerializeField] private GameObject Crate;
    [SerializeField] private Transform BlocksContainer;

    [SerializeField] private float xMovement = 2f;
    [SerializeField] private float yMovement = -2f;
    void Start()
    {
        string data = LoadLevel("Assets/Levels/Level1.txt");
        string [] line = data.Split('\n');

        //X X X X
        //XXX
        Vector2 position = StartingPoint.position;
        int count = 1;
        for(int i = 0 ; i < line.Length; i++){
            for(int j = 0 ; j < line[i].Length; j++){
                if(line[i][j] == 'X'){
                    GameObject element = GameObject.Instantiate(Crate);
                    //element.transform.position = position;
                    StartCoroutine(AnimateToPosition(element,position));
                    element.name = "Crate "+ count;
                    count++;
                    element.transform.SetParent(BlocksContainer);
                }
                position.x += xMovement;
            }
            position.y += yMovement;
            position.x = StartingPoint.position.x;
        }
    }

    IEnumerator AnimateToPosition(GameObject obj, Vector2 position){
        Transform objTransform = obj.transform;
        while(Vector2.Distance(objTransform.position, position) > 0.1f){
            objTransform.position = Vector2.Lerp(objTransform.position, position, Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public string LoadLevel (string path){
        string data = "";
        try{
            using(StreamReader sr = new StreamReader(path)){
                string line;
                while((line = sr.ReadLine()) != null){
                    data += line + "\n";
                }
            }
        }
        catch(IOException e){
            Debug.Log("File not found: " + e);
        }
        return data;
    }

}
