using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEditor;
using UnityEngine;

/// <summary>
/// This script handle all the control code, so detecting when the users click on a unit or building and selecting those
/// If a unit is selected it will give the order to go to the clicked point or building when right clicking.
/// </summary>
public class UserControl : MonoBehaviour
{
    public Camera GameCamera;
    public float PanSpeed = 10.0f;
    public GameObject Marker;
    
    private Unit m_Selected = null;

    int count = 0;
    bool notAvailable =  true;
    bool underBulding = false;
    

    private void Start()
    {
        Marker.SetActive(false);
    }

    private void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GameCamera.transform.position = GameCamera.transform.position + new Vector3(move.y, 0, -move.x) * PanSpeed * Time.deltaTime;
        
        
        
        if (Input.GetMouseButtonDown(0) && notAvailable)
        {
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //the collider could be children of the unit, so we make sure to check in the parent
                var unit = hit.collider.GetComponentInParent<Unit>();
                m_Selected = unit;


                if (m_Selected != null)
                {
                    notAvailable = false;
                    Debug.Log("notAvailable "+ notAvailable + " mSelect" + m_Selected);
                }
                else
                {
                    
                Debug.Log("m_Selected null "+ m_Selected);
                    notAvailable = true;
                }
                //check if the hit object have a IUIInfoContent to display in the UI
                //if there is none, this will be null, so this will hid the panel if it was displayed
                var uiInfo = hit.collider.GetComponentInParent<UIMainScene.IUIInfoContent>();
                UIMainScene.Instance.SetNewInfoContent(uiInfo);
            }
        }   
        else if (m_Selected != null && Input.GetMouseButtonDown(0) && TouchCounting(4) == 4)
        {//right click give order to the unit
        
            
            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var building = hit.collider.GetComponentInParent<Building>();
               
                
                if (building != null)
                {   
                    Debug.Log($"truck Position "+ m_Selected.transform.position);
                    Debug.Log("building Position "+ building.transform.position);
                    m_Selected.GoTo(building);
                    underBulding  = true;
                    
                    
                
                }
                else
                {        Debug.Log("truck Position "+m_Selected.transform.position);
                         Debug.Log("hit Position "+ hit.point);
                        m_Selected.GoTo(hit.point);
                        Marker.SetActive(false);
                        Marker.transform.SetParent(null);
                        notAvailable = true;
                        
                        Debug.Log("notAvailable "+ notAvailable + " mSelect" + m_Selected);
                

                }
                    
        
                
            }
        }
        else
        {
            move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        GameCamera.transform.position = GameCamera.transform.position + new Vector3(move.y, 0, -move.x) * PanSpeed * Time.deltaTime;

            var ray = GameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //the collider could be children of the unit, so we make sure to check in the parent
                var unit = hit.collider.GetComponentInParent<Unit>();
                m_Selected = unit;
            }
        }
        

        MarkerHandling();
    }
    
    // Handle displaying the marker above the unit that is currently selected (or hiding it if no unit is selected)
    void MarkerHandling()
    {
        
        if (m_Selected != null && Marker.transform.parent != m_Selected.transform)
        {
            Marker.SetActive(true);
            Marker.transform.SetParent(m_Selected.transform, true);
            Marker.transform.localPosition = Vector3.zero;
        }
    
    }

    private  int TouchCounting(int x)
    {
        if (m_Selected != null &&Input.GetMouseButtonDown(0))
            {Debug.Log("touch count "+ count);
            if (count > x)
            {
                count  = 0;
            }
                return count++;
            }
        else
        {
         return count;   
        }
    }

   
    
}
