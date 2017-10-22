using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class ScrollViewController : MonoBehaviour {

    private Scrollbar scrollbar;

    public void Start()
    {
        scrollbar = GetComponent<Scrollbar>();    
    }

    private void Update()
    {
        
    }

}
