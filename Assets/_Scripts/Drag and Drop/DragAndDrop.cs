﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] GameObject colliderOfFoodOnPlate;
    Vector3 dist;
    Vector3 startPos;
    Rigidbody rb;
    GameManager gm;
    float posX;
    float posZ;
    float posY;
    bool isDragging = false; 
    bool isMouseEnable = true;
    bool isCollided = false;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        ActivedCollider(false);
    }

    private void Update()
    {
        if (isDragging)
        {
            float disX = Input.mousePosition.x - posX;
            float disY = Input.mousePosition.y - posY;
            float disZ = Input.mousePosition.z - posZ;
            Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));
            transform.position = new Vector3(lastPos.x, startPos.y, lastPos.z);
        }
    }

    private void ActivedCollider(bool v)
    {
        if (colliderOfFoodOnPlate != null)
        {
            colliderOfFoodOnPlate.SetActive(v);
        }
    }

    private void OnMouseUp()
    {
        //gm.DisableArrow();
        Debug.Log("Mouse Up");
        ActivedCollider(true);
        rb.useGravity = true;
        rb.velocity = new Vector3(0, -1, 0);
        isDragging = false; // Quando o jogador solta o mouse, ele não consegue mais arrastar
        isMouseEnable = false;
    }
    void OnMouseDown()
    {
        if (isMouseEnable)
        {
            Debug.Log("Mouse Down");
            isDragging = true;
            rb.useGravity = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + 4.5f, transform.position.z);
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            dist = Camera.main.WorldToScreenPoint(transform.position);
            posX = Input.mousePosition.x - dist.x;
            posY = Input.mousePosition.y - dist.y;
            posZ = Input.mousePosition.z - dist.z;
            gm.PlaySoundEffect(1);
            ActivedCollider(false);
        }
        
    }

    void OnMouseDrag()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Mesa") && !isCollided)
        {
            WrongTarget();
        }
        else if (collision.gameObject.tag.Equals("Tabua"))
        {
            isMouseEnable = true; // Deixa o jogador pegar o objeto novamente
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollided)
        {
            //gm.foodDropped++;
            //Debug.Log(gm.foodDropped);
            isCollided = true;
            GameObject go = other.gameObject;
            switch (gameObject.tag)
            {
                case "Hamburger":
                    CheckTarget(go.tag, "PosiHamburger", go);
                    break;
                case "Egg":
                    CheckTarget(go.tag, "PosiEgg", go);
                    break;
                case "Lettude":
                    CheckTarget(go.tag, "PosiLettude", go);
                    break;
                case "Steak":
                    CheckTarget(go.tag, "PosiSteak", go);
                    break;
                case "Chicken":
                    CheckTarget(go.tag, "PosiChicken", go);
                    break;
                case "Maki":
                    CheckTarget(go.tag, "PosiMaki", go);
                    break;
                case "Tempura":
                    CheckTarget(go.tag, "PosiTempura", go);
                    break;
                case "Sushi":
                    CheckTarget(go.tag, "PosiSushi", go);
                    break;
            }
        }
        
    }

    private void CheckTarget(string objectTag, string posiFood, GameObject go)
    {
        Destroy(GetComponent<DragAndDrop>()); // vai destruir o Script para não correr o risco do jogador pega-lo novamente
        if (objectTag.Equals(posiFood))
        {
            gm.AddTotalHitPerStage();
            gm.PlaySoundEffect(2);
            ActivedCollider(false);
            Destroy(go);
            Debug.Log("Acertou!");
        }
        else
        {
            WrongTarget();
        }
    }

    void WrongTarget()
    {
        DropDragAndDrop(); // não permite que o jogador pegue o aliemento para não correr o risco dele pegar um alimento diferente depois que errar o anterior.
        Debug.Log("Errou!");
        gm.RestartStages("DragAndDrop");
    }

    private static void DropDragAndDrop()
    {
        DragAndDrop[] dragAndDrop = FindObjectsOfType<DragAndDrop>();
        foreach (DragAndDrop dd in dragAndDrop)
        {
            Destroy(dd);
        }
    }
}
