using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class InputManager : ARBaseGestureInteractable
{
    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private XROrigin sessionOrigin;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Touch touch;
    private Pose pose;


    private void FixedUpdate()
    {
        CrossHairCalculation();
      /*  touch = Input.GetTouch(0);
        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began)
            return;

        if (IsPointerOverUI(touch))
            return;

        Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
*/

    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if(gesture.targetObject == null) 
        {
            return true;
        }
        return false;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.isCanceled)
            return;
        if(gesture.targetObject != null || IsPointerOverUI(gesture))
        {
            return;
        }
        if (GestureTransformationUtility.Raycast(gesture.startPosition , hits, base.xrOrigin ,TrackableType.PlaneWithinPolygon))
        {
            GameObject placedObj = Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);

            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;
            placedObj.transform.parent = anchorObject.transform;
        }
    }

    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);

        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventData, results);
        return results.Count > 0;
    }

    void CrossHairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
       
        if (GestureTransformationUtility.Raycast(origin, hits, base.xrOrigin, TrackableType.PlaneWithinPolygon))
        { 
            pose = hits[0].pose;
            crossHair.transform.position = pose.position;
            crossHair.transform.eulerAngles = new Vector3(90,0,0);
        }
    }
}
