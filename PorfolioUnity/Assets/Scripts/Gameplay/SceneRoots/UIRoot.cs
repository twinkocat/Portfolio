using System.Collections.Generic;
using UnityEngine;


public class UIRoot : MonoBehaviour
{
    private readonly List<View> views = new();

    public void Add(View view)
    {
        views.Add(view);
    }
    
    public void UpdateRootOrder()
    {
        views.Sort((viewA, viewB) => viewA.ScreenOrder.CompareTo(viewB.ScreenOrder));

        for (var i = 0; i < views.Count; i++)
        {
            views[i].transform.SetSiblingIndex(i);
        }
    }
}
