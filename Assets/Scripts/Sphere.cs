using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : PersistableObject
{
	private Rect allowedArea = new Rect(-17f, -10f, 34f, 20f);
	// Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
		SetUpConstraints();
	}

	//Ensures ball stays in the boundsof the arena
	private void SetUpConstraints()
    {
		Vector3 newPosition = transform.localPosition;

		if (newPosition.x < allowedArea.xMin)
		{
			newPosition.x = allowedArea.xMin;
		}
		else if (newPosition.x > allowedArea.xMax)
		{
			newPosition.x = allowedArea.xMax;
		}
		if (newPosition.z < allowedArea.yMin)
		{
			newPosition.z = allowedArea.yMin;
		}
		else if (newPosition.z > allowedArea.yMax)
		{
			newPosition.z = allowedArea.yMax;
		}
		transform.localPosition = newPosition;
	}

	//Saves the balls current position, rotation, and rigidbody values
	public override void Save(GameDataWriter writer)
	{
		writer.Write(transform.localPosition);
		writer.Write(transform.localRotation);
		writer.Write(gameObject.GetComponent<Rigidbody>().useGravity);
		writer.Write(gameObject.GetComponent<Rigidbody>().isKinematic);
	}

	//loads in the balls current position, rotation, and rigidbody values
	public override void Load(GameDataReader reader)
	{
		transform.localPosition = reader.ReadVector3();
		transform.localRotation = reader.ReadQuaternion();
		gameObject.GetComponent<Rigidbody>().useGravity = reader.ReadBool();
		gameObject.GetComponent<Rigidbody>().isKinematic = reader.ReadBool();
	}
}
