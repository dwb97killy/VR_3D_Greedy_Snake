using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [HideInInspector]
    public PlayerDirection direction;// Start is called before the first frame update

    [HideInInspector]
    public float step_length=0.2f;

    [HideInInspector]
    public float movement_Frequency= 0.1f;

    private float counter;
    private bool move;

    [SerializeField]
    private GameObject tailprefab;

    private List<Vector3> delta_position;

    private List <Rigidbody> nodes;

    private Rigidbody main_Body;
    private Rigidbody head_Body;
    private Transform tr;

    private bool create_Node_At_Tail;
    void Awake()
    {
        tr=transform;
        main_Body =GetComponent<Rigidbody>();
        InitSnakeNodes();
        InitPlayer();

        delta_position = new List<Vector3>(){
            new Vector3(-step_length,0f), // -dx ..left
            new Vector3(0f,step_length), // dy up
            new Vector3(step_length,0f), // dx right
            new Vector3(0f,-step_length),//-dy down
        };
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovementFrequency();
    }

    void FixedUpdate(){
        if(move){
            move =false;

            Move();
        }
    }
    void InitSnakeNodes(){
        nodes = new List<Rigidbody>();
        nodes.Add(tr.GetChild(0).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(1).GetComponent<Rigidbody>());
        nodes.Add(tr.GetChild(2).GetComponent<Rigidbody>());

        head_Body=nodes[0];
    }
    void SetDirectionRandom(){
        int dirRandom =Random.Range(0, (int)PlayerDirection.COUNT);
        direction=(PlayerDirection)dirRandom;
    }
    void InitPlayer(){
        SetDirectionRandom();
        switch (direction)
        {
            
          case PlayerDirection.RIGHT:
            nodes[1].position= nodes[0].position  - new Vector3(Metrics.NODE,0f,0f);
            nodes[2].position= nodes[0].position  - new Vector3(Metrics.NODE * 2f,0f,0f);
          break;
          case PlayerDirection.LEFT:
           nodes[1].position= nodes[0].position  + new Vector3(Metrics.NODE,0f,0f);
            nodes[2].position= nodes[0].position  + new Vector3(Metrics.NODE * 2f,0f,0f);
          break;
          case PlayerDirection.UP:
           nodes[1].position= nodes[0].position  - new Vector3(0f,Metrics.NODE,0f);
           nodes[2].position= nodes[0].position  - new Vector3(0f,Metrics.NODE * 2f,0f);
          break;
          case PlayerDirection.DOWN:
            nodes[1].position= nodes[0].position  + new Vector3(0f,Metrics.NODE,0f);
           nodes[2].position= nodes[0].position  + new Vector3(0f,Metrics.NODE * 2f,0f);
          break;
        }
    }
    void Move(){
        Vector3 dPosition =delta_position[(int)direction];
        Vector3 parentPos = head_Body.position;
        Vector3 prevPosition;
        main_Body.position = main_Body.position + dPosition;
        head_Body.position = head_Body.position + dPosition;
        for(int i=1; i<nodes.Count; i++){
            prevPosition = nodes[i].position;

            nodes[i].position= parentPos;
            parentPos = prevPosition;
        }
    }
    void CheckMovementFrequency(){
        counter += Time.deltaTime;
        if(counter>= movement_Frequency){
            counter = 0f;
            move = true;
        }
    }
}
