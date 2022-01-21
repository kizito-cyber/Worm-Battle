using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UltimateJoystickExample;
using Photon.Pun;

public class Wormy : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D bulletPrefab;
    public Transform currentGun;

    public float wormySpeed = 1;
    public float maxRelativeVelocity;
    public float misileForce = 5;

    public bool IsTurn { get { return WormyManager.singleton.IsMyTurn(wormId); } }

    public int wormId;
    WormyHealth wormyHealth;
    SpriteRenderer ren;
    PhotonView view;
    public static bool isFire = false;
   
    private void Start()
    {
        wormyHealth = GetComponent<WormyHealth>();
        ren = GetComponent<SpriteRenderer>();

        // Don't use GetComponent on Runtime, the better way is make a public variable and drag and drop the Component in the inspector
        view = GetComponent<PhotonView>();
        AddObservable();
    }
    private void AddObservable()
    {
        if (!view.ObservedComponents.Contains(this))
        {
            view.ObservedComponents.Add(this);
        }
    }


    private void Update()
    {
        if (view.IsMine)
        {
            if (!IsTurn)
                return;

            //view.RPC("RPC_RotateGun", RpcTarget.AllBuffered);
            // RotateGun(); 
            PlayerShoot();
           // Debug.Log("Thi is ren FlipX" + ren.flipX);
          
        }

    }

    void PlayerShoot()
    {
        var hor = UltimateJoystick.GetHorizontalAxis("Ultimate");
        if (hor == 0)
        {
            currentGun.gameObject.SetActive(true);

            if (isFire == true)
            {

                view.RPC("RPC_Shoot", RpcTarget.AllBuffered); //For Network

                if (IsTurn)
                    WormyManager.singleton.NextWorm();
            }
        }
        else
        {
            currentGun.gameObject.SetActive(false);
            transform.position += Vector3.right * hor * Time.deltaTime * wormySpeed;

            ren.flipX = UltimateJoystick.GetHorizontalAxis("Ultimate") > 0;
            
        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ren.flipX);
        }
        else
        {
            // object value = stream.ReceiveNext();
            //Debug.Log(ren);
            //Debug.Log(stream.PeekNext()?.GetType()?.FullName ?? "null");
            //ren.flipX = (bool)stream.ReceiveNext(); 
            object next = stream.ReceiveNext();
            if (ren != null || TryGetComponent<SpriteRenderer>(out ren))
            {
               
                ren.flipX = (bool)next;
               
            }



        }
    }


    void RotateGun()
    {
        var diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        currentGun.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > maxRelativeVelocity)
        {
            wormyHealth.ChangeHealth(-3);
            if (IsTurn)
                WormyManager.singleton.NextWorm();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            wormyHealth.ChangeHealth(-10);
            if (IsTurn)
                WormyManager.singleton.NextWorm();
        }

    }

    //RPCs go on the bottom of the code


    [PunRPC]
    public void RPC_Shoot()
    {
        var p = Instantiate(bulletPrefab, currentGun.position - currentGun.right, currentGun.rotation);
        p.AddForce(-currentGun.right * misileForce, ForceMode2D.Impulse);
    }

   
}
