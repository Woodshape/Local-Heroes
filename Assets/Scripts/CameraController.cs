using System.Collections;
using System.Collections.Generic;
using LH.UI;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public Transform cameraTransform;
    public Transform followTransform;

    public GameObject informationDisplay;
    
    public float speed = 5f;
    public Vector3 zoom;

    private Vector3 newPosition;
    private Vector3 newZoom;
    private Quaternion newRotation;

    private Vector3 dragStartPos;
    private Vector3 dragCurrentPos;
    private Vector3 rotateStartPos;
    private Vector3 rotateCurrentPos;

    private Entity activeEntity;

    private void Start()
    {
        camera = Camera.main;
        cameraTransform = camera.transform;
        
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }
    
    private void Update()
    {
        if (followTransform && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Escape)))
        {
            followTransform = null;
        }
        else {
            if (Input.GetKeyDown(KeyCode.F)) {
                if (activeEntity) {
                    followTransform = activeEntity.transform;
                }
            }
        }

        if (followTransform)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, speed * Time.deltaTime);
        }
        
        HandleKeyInput();
        HandleMouseInput();

        //transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, speed * Time.deltaTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, speed * Time.deltaTime);
    }
    private void HandleKeyInput()
    {
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.Translate(horizontal, 0, vertical);
    }

    private void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoom;
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                Entity entity = hit.transform.gameObject.GetComponentInParent<Entity>();
                
                //  we have hit an entity
                if (entity != null) {
                    activeEntity = entity;

                    EntityInformationDisplay information = informationDisplay.GetComponent<EntityInformationDisplay>();
                    information.DisplayInformation(activeEntity);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            rotateCurrentPos = Input.mousePosition;

            Vector3 diff = rotateStartPos - rotateCurrentPos;
            rotateStartPos = rotateCurrentPos;
            
            newRotation *= Quaternion.Euler(Vector3.up * (-diff.x / 5f));
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPos = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPos - dragCurrentPos;
            }
        }
    }
}
