using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : PersistableObject
{
    public delegate void CubeHit();
    public event CubeHit cubeHit;
    public bool isHit;

    //Method to change cube color to green
    void ChangeColor()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0, 1);
    }

    void OnCollisionEnter(Collision collision)
    {
        //Checks if the cube has already been hit
        if(isHit == false)
        {
            //ensures the collider belongs to the ball object
            if (collision.collider.tag == "Sphere")
            {
                //Marks cube isHit to true, sends an event, and changes color
                isHit = true;
                cubeHit?.Invoke();
                ChangeColor();
            }
        }
    }

    //Saves the current cube color and bool isHit value
    public override void Save(GameDataWriter writer)
    {
        writer.Write(transform.GetComponent<MeshRenderer>().material.color);
        writer.Write(isHit);
    }

    //Loads the saved cube color and bool isHit value
    public override void Load(GameDataReader reader)
    {
        transform.GetComponent<MeshRenderer>().material.color = reader.ReadColor();
        gameObject.GetComponent<Cube>().isHit = reader.ReadBool();
    }
}
