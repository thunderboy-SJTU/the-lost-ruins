using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sample : MonoBehaviour {


    //当前格子坐标
    public int x;
    public int y;

    private void Awake()
    {
        float gridCellsize = gridManager.instance.gridCellSize;
        Vector3 position = this.transform.position - gridManager.instance.Origin;
        x = (int)((position.x - gridCellsize / 2) / gridCellsize);
        y = (int)((position.z - gridCellsize / 2) / gridCellsize);
    }
    //判断当前格子的类型
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.layer == 12)
        {
            //Debug.Log("samples");
            gridManager.instance.nodes[x, y].bObstacle = false;
        }
  
    }
}
