﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Photon.MonoBehaviour
{
    //assets parameters
    public PhotonView photonView;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public Text PlayerNameText;

    //movement parameters
    public float MoveSpeed;
    public float turnSpeed;
    public bool isDragon;

    // public Transform playerPos;

    //attack parameters
    public GameObject BulletObject;
    public Transform FirePos;

    public bool DisableInput = false;


    private void Awake()
    {
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
        }
        else
        {
            PlayerNameText.text = photonView.owner.name;
            PlayerNameText.color = Color.cyan;
        }
    }

    private void Update()
    {
        if (photonView.isMine && !DisableInput)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (isDragon)
        {
            if (Input.GetKey("a"))
            {
                // rb.rotation += 100.0f * Time.deltaTime;
                rb.transform.Rotate(0.0f, 0.0f, turnSpeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey("d"))
            {
                // rb.rotation -= 100.0f * Time.deltaTime;
                rb.transform.Rotate(0.0f, 0.0f, -turnSpeed * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey("w"))
            {
                rb.AddForce(transform.up * MoveSpeed);
            }
            if (Input.GetKey("s"))
            {
                rb.AddForce(transform.up * MoveSpeed * -1);
            }
            //Debug.Log("Rotation" + rb.rotation);
        }
        else
        {
            var moveH = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            var moveV = new Vector3(0, Input.GetAxis("Vertical"), 0);
            transform.position += moveH * MoveSpeed * Time.deltaTime;
            transform.position += moveV * MoveSpeed * Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        //flip face direction
        if (Input.GetAxis("Horizontal") < 0)
        {
            photonView.RPC("FlipTrue", PhotonTargets.AllBuffered);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            photonView.RPC("FlipFalse", PhotonTargets.AllBuffered);
        }

        //play running animation
        if (Input.GetAxis("Horizontal") != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }



    private void Shoot()
    {
        GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
        if (isDragon)
        {
            obj.transform.Rotate(0.0f, 0.0f, rb.rotation+90, Space.Self);
        }
        else{
            if (sr.flipX == false){}
            if (sr.flipX == true){
                obj.GetComponent<PhotonView>().RPC("ChangeDir_left", PhotonTargets.AllBuffered);
            }
        }

        anim.SetTrigger("shootTrigger");
    }



    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }

    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }

}
