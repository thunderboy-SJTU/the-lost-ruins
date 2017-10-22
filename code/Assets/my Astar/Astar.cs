using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    public static PriorityQueue closedList, openList;

    private static float HeuristicEstimateCost(Node curNode,
        Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }

    public static ArrayList FindPath(Node start, Node goal)
    {
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);
        closedList = new PriorityQueue();
        Node node = null;

        while (openList.Length != 0)
        {
            node = openList.First();
            //Debug.Log("position"+node.position);
            //Check if the current node is the goal node  
            if (node.position == goal.position)
            {
                return CalculatePath(node);
            }
            //Create an ArrayList to store the neighboring nodes  
            ArrayList neighbours = new ArrayList();
            gridManager.instance.GetNeighbours(node, neighbours);
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                //Debug.Log("neighbor" + neighbourNode.position);
                if (!closedList.Contains(neighbourNode))
                {
                    float cost = HeuristicEstimateCost(node,
                            neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeEstCost = HeuristicEstimateCost(
                            neighbourNode, goal);
                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimatedCost = totalCost +
                            neighbourNodeEstCost;
                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Push(neighbourNode);
                    }
                }
            }
            //Push the current node to the closed list  
            closedList.Push(node);
            //and remove it from openList  
            openList.Remove(node);
        }
        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }
        return CalculatePath(node);

    }

    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    public static ArrayList findPath(Vector3 begin,Vector3 end)
    {
        //Debug.Log("begin" + begin);
        Node startNode = new Node(gridManager.instance.GetGridCellCenter(
           gridManager.instance.GetGridIndex(begin)));
        Node goalNode = new Node(gridManager.instance.GetGridCellCenter(
                gridManager.instance.GetGridIndex(end)));
        ArrayList path = FindPath(startNode, goalNode);
        OnDrawGizmos(path);
        return path ;
        
    }

    /*public static void OnDrawGizmos(Node node)
    {
        Vector3 cellSize = new Vector3(gridManager.instance.gridCellSize, 1.0f,
               gridManager.instance.gridCellSize);

        Vector3 Pos = node.position;
        int Index = gridManager.instance.GetGridIndex(Pos);
        Gizmos.DrawCube(gridManager.instance.GetGridCellCenter(
                            Index), cellSize);
    }*/
    public static void OnDrawGizmos(ArrayList pathArray)
    {
        if (pathArray == null)
            return;
        if (pathArray.Count > 0)
        {
            //Debug.Log("242424  "+pathArray.Count);
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    //Debug.Log("gg  "+nextNode.position);
                    Debug.DrawLine(node.position, nextNode.position,
                        Color.green);
                    index++;
                }
            }
        }
    }
    }
