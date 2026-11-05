using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockUnit : MonoBehaviour
{
    [SerializeField] private float FOVangle;
    [SerializeField] private float smoothDamp;

    private List<FlockUnit> cohesionNeighbours = new List<FlockUnit>();
    private List<FlockUnit> avoidanceNeighbours = new List<FlockUnit>();
    private List<FlockUnit> aligementNeighbours = new List<FlockUnit>();
    private Flock assignedFlock;
    private Vector3 currentVelocity;
    private float speed;

    public Transform myTransform { get; set; }

    public void AssignFlock(Flock flock)
    {
        assignedFlock = flock;
    }

    public void InitializeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void MoveUnit()
    {


        FindNeighbours();
        CalculateSpeed();

        var cohesionVector = CalculateCohesionVector();
        var avoidanceVector = CalculateAvoidanceVector();
        var aligementVector = CalculateAligementVector();
        var boundsVector = CalculateBoundsVector();
        var moveVector = cohesionVector + avoidanceVector + aligementVector + boundsVector;

        moveVector = Vector3.SmoothDamp(myTransform.forward, moveVector, ref currentVelocity, smoothDamp);
        moveVector = moveVector.normalized * speed;

        if (moveVector == Vector3.zero)
        {
            moveVector = transform.forward;
        }

        myTransform.forward = moveVector;
        myTransform.position += moveVector * Time.deltaTime;
    }

    

    private void FindNeighbours()
    {
        cohesionNeighbours.Clear();
        avoidanceNeighbours.Clear();
        aligementNeighbours.Clear();
        var allUnits = assignedFlock.allUnits;

        for (int i = 0; i < allUnits.Length; i++)
        {
            var currentUnit = allUnits[i];
            if (currentUnit != this)
            {
                float currentNeighbourDistanceSqr = Vector3.SqrMagnitude(currentUnit.transform.position - myTransform.position);
                if(currentNeighbourDistanceSqr <= assignedFlock.cohesionDistance * assignedFlock.cohesionDistance)
                {
                    cohesionNeighbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.avoidanceDistance * assignedFlock.avoidanceDistance)
                {
                    avoidanceNeighbours.Add(currentUnit);
                }
                if (currentNeighbourDistanceSqr <= assignedFlock.aligementDistance * assignedFlock.aligementDistance)
                {
                    aligementNeighbours.Add(currentUnit);
                }
            }
        }
    }

    private void CalculateSpeed()
    {
        if (cohesionNeighbours.Count == 0)
        {
            return;
        }
        
        speed = 0;

        for (int i = 0; i < cohesionNeighbours.Count; ++i)
        {
            speed += cohesionNeighbours[i].speed;
        }
        speed /= cohesionNeighbours.Count;
        speed = Mathf.Clamp(speed, assignedFlock.minSpeed, assignedFlock.maxSpeed);
    }

    private Vector3 CalculateBoundsVector()
    {
        Vector3 center = assignedFlock.transform.position;
        Vector3 offset = center - myTransform.position;

        if (offset.magnitude > assignedFlock.boundsRadius)
        {
            return offset.normalized;
        }

        return Vector3.zero;
    }

    private Vector3 CalculateCohesionVector()
    {
        var cohesionVector =  Vector3.zero;
        int neighboursInFOV = 0;
        if (cohesionNeighbours.Count == 0)
        {
            return cohesionVector;
        }
        for (int i = 0; i < cohesionNeighbours.Count; ++i)
        {
            if (IsInFov(cohesionNeighbours[i].myTransform.position))
            {
                neighboursInFOV++;
                cohesionVector += cohesionNeighbours[i].myTransform.position;
            }
        }
     
        cohesionVector /= neighboursInFOV;
        cohesionVector -= myTransform.position;
        cohesionVector = cohesionVector.normalized;
        return cohesionVector;
    }



    private Vector3 CalculateAligementVector()
    {
        var aligementVector = myTransform.forward;
        if (aligementNeighbours.Count == 0)
        {
            return aligementVector;
        }

        int neighboursInFov = 0;

        for (int i = 0; i < aligementNeighbours.Count; ++i)
        {
            if (IsInFov(aligementNeighbours[i].myTransform.forward))
            {
            neighboursInFov++;
            aligementVector += aligementNeighbours[i].myTransform.forward;
            }
        }


        aligementVector /= neighboursInFov;
        aligementVector = aligementVector.normalized;


        return aligementVector;
    }


    private Vector3 CalculateAvoidanceVector()
    {
        var avoidanceVector = Vector3.zero;
        if (aligementNeighbours.Count == 0)
        {
            return Vector3.zero;
        }

        int neighboursInFov = 0;

        for (int i = 0; i < avoidanceNeighbours.Count; ++i)
        {
            if (IsInFov(avoidanceNeighbours[i].myTransform.forward))
            {
                neighboursInFov++;
                avoidanceVector += (myTransform.position - avoidanceNeighbours[i].myTransform.position);
            }
        }


        avoidanceVector /= neighboursInFov;
        avoidanceVector = avoidanceVector.normalized;


        return avoidanceVector;
    }



    private bool IsInFov(Vector3 position)
    {
        return Vector3.Angle(myTransform.forward, position - myTransform.position) < FOVangle;
    }

    private void Awake()
    {
        myTransform = transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
