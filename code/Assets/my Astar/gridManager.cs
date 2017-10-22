using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Node : IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;


    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = true;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = true;
        this.parent = null;
        this.position = pos;
    }

    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        if (this.estimatedCost > node.estimatedCost)
            return 1;
        return 0;
    }
}

public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get { return this.nodes.Count; }
    }

    public bool Contains(object node)
    {
        return this.nodes.Contains(node);
    }

    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    public void Push(Node node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }

    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        this.nodes.Sort();
    }
}

public class gridManager : MonoBehaviour {

    private static gridManager s_Instance = null;
    public static gridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(gridManager))
                        as gridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager " +
                            "object. \n You have to have exactly " +
                            "one GridManager in the scene.");
            }
            return s_Instance;
        }
    }

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    public Node[,] nodes { get; set; }
    public Vector3 Origin
    {
        get { return origin; }
    }
    public GameObject reference;
    public GameObject[,] references { get; set; }



    void Awake()
    {
        //obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        origin = this.transform.position;
        Debug.Log("origin:" + Origin);
        Calculate();
    }
    void Calculate()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                GameObject item = (GameObject)Instantiate(reference,
                                   new Vector3(GetGridCellCenter(index).x, 0, GetGridCellCenter(index).z),
                                   Quaternion.identity);
                GameObject.Destroy(item.transform.gameObject);
                index++;
            }
           
        }
       
    }

    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);
        return cellPosition;
    }
    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);
        //return (row * numOfColumns + col);
        return (col * numOfRows + row);
    }
    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
                pos.x <= Origin.z + height && pos.z >= Origin.z);
    }

    public int GetRow(int index)
    {
        int row = index % numOfRows;
        return row;
    }
    public int GetColumn(int index)
    {
        int col = index / numOfRows;
        return col;
    }

    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        //Debug.Log("row" + row);
        //Debug.Log("col" + column);
        //Bottom  
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Top  
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Right  
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Left  
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }

    void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        /*Vector3 cellSize = new Vector3(gridCellSize, 1.0f,
                gridCellSize);*/
        if (row != -1 && column != -1 &&
            row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[column, row];
            //Debug.Log(nodeToAdd.bObstacle);
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
                /*Gizmos.DrawCube(GetGridCellCenter(
                            column*numOfRows+row), cellSize);*/
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns,
                    gridCellSize, Color.blue);
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f,
                gridCellSize);
            /*if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach (GameObject data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(
                            GetGridIndex(data.transform.position)), cellSize);
                }
            }*/
            /*int index = 0;
            for (int i = 0; i < numOfColumns; i++)
            {
                for (int j = 0; j < numOfRows; j++)
                {
                    if (i == 3 && j == 11) Debug.Log(nodes[i, j].bObstacle);
                    if(nodes[i,j].bObstacle == false)
                    {
                        Gizmos.DrawCube(GetGridCellCenter(
                            index), cellSize);
                        //references[i, j].SetActive(false);
                    }
                    index++;                   
                }
            }*/
        }
    }
    public void DebugDrawGrid(Vector3 origin, int numRows, int
        numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);
        // Draw the horizontal grid lines  
        for (int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f,
                0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f,
                0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
        // Draw the vertial grid lines  
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f,
                0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f,
                1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
 
}
