using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CanChop_V2 : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public LayerMask sliceableLayer;
    public VelocityEstimator velocityEstimator;

    // Start is called before the first frame update
    public float cutForce = 50; // Reduced cut force for less bounce

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit)
        {
            GameObject target = hit.collider.gameObject;
            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal, target.GetComponent<Renderer>().material);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    void SetupSlicedComponent(GameObject slicedObject)
    {
        // Assuming you are using Unity XR Interaction Toolkit, adjust accordingly for your specific VR SDK
        // First, ensure the object can be interacted with by adding an interactable component
        // var interactable = slicedObject.AddComponent<XRGrabInteractable>(); // 

        // Set up Rigidbody for physics interactions
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;
        collider.sharedMesh = slicedObject.GetComponent<MeshFilter>().mesh;

        slicedObject.AddComponent<XRGrabInteractable>();

        // Apply a gentle force to make the slicing effect more realistic
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);

        // Set the layer to the same as the original sliceable layer
        slicedObject.layer = LayerMask.NameToLayer("Food");
    }
}
