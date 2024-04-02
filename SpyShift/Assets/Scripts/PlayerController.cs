using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveDir;
    private Rigidbody rigid;
    private int speed=5;
    public float minSpinDistance;
    public float minPickUpDistance;
    private bool hasRock;
    private bool isShootingRock;
    private GameObject rock = null;
    private bool isCrawling;

    private float shootingForce;
    private float addedForce = 0;

    public LineRenderer lineRenderer;
    private int linePointCount = 20;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        lineRenderer.positionCount = linePointCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShootingRock)
        {
          
            if (shootingForce>=15)
            {
                addedForce = -Time.deltaTime * 3;
            }
            else if(shootingForce<=1)
            {
                addedForce = Time.deltaTime * 3;
            }
            shootingForce += addedForce;
       //     Projectileline();
        }
    }

    private void Projectileline()
    {
        float initiateVelocity = shootingForce / rock.GetComponent<Rigidbody>().mass;
        float yAccesserate = Physics.gravity.y / rock.GetComponent<Rigidbody>().mass;
        for (int i = 1; i < linePointCount; i++)
        {
            float zPos = initiateVelocity * i * Time.fixedDeltaTime * 50;
            float yPos = (initiateVelocity + (initiateVelocity - yAccesserate * i*Time.fixedDeltaTime * 50)) / 2 * Time.fixedDeltaTime *i* 50;
            Vector3 pos = transform.TransformDirection(new Vector3(rock.transform.position.x, yPos, zPos));
            lineRenderer.SetPosition(i, pos);
        }
    }
    private void FixedUpdate()
    {
        rigid.velocity = transform.TransformDirection(new Vector3(moveDir.x, rigid.velocity.y, moveDir.y));
    }

    public void Move(InputAction.CallbackContext input )
    {
        //if (input.performed)
        //{
        //    Vector2 v= input.ReadValue<Vector2>();
        //    rigid.velocity = new Vector3(v.x, 0, v.y);
        //}
        moveDir = input.ReadValue<Vector2>()*speed;
    }

    public void Spin(InputAction.CallbackContext input)
    {
    
        if (input.performed)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, minSpinDistance))
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    GetComponent<MeshRenderer>().material.color = hit.transform.GetComponent<MeshRenderer>().material.color;
                }
            }
        }    
    }

    public void PickUp(InputAction.CallbackContext input)
    {
        
        if (input.performed)
        {
            if (!hasRock)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, minPickUpDistance))
                {
                    if (hit.transform.gameObject.tag == "Rock")
                    {
                        //     hit.transform.SetParent(transform);
                        hit.transform.GetComponent<Rock>().SetOnPlayer();
                        hasRock = true;
                        rock = hit.transform.gameObject;
                    }
                }
            }
            else
            {
                isShootingRock = true;
                shootingForce = 0;
           //     lineRenderer.SetPosition(0, rock.transform.position);

            }        
        }

        if (input.canceled)
        {
            if (isShootingRock)
            {
                isShootingRock = false;
                hasRock = false;
                rock.transform.GetComponent<Rock>().SetOnPlayer();
                rock.transform.GetComponent<Rigidbody>().AddRelativeForce(0, shootingForce, shootingForce,ForceMode.Impulse);
                rock = null;
                
            }
        }

    }

    public void Crawl(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            isCrawling = !isCrawling;
            float Yscale = isCrawling ? 0.5f : 1;
            transform.localScale = new Vector3(1, Yscale, 1);
        }
    }
}
