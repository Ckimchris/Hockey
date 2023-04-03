using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PersistableObject
{

	public GameObject sphere;

	private bool isHolding;
	private bool isNear;
	private float maxSpeed = 10f;
	private GameObject heldSpot;
	private Rect allowedArea = new Rect(-17f, -10f, 34f, 20f);
	private Vector3 velocity;
	private Rigidbody m_Rb;
	private Vector2 playerInput;

	private void Start()
    {
		SetUp();
	}

	void Update()
	{

		if (Input.GetKeyDown(KeyCode.Z))
		{
			if(isHolding)
            {
				ThrowObject();

			}
			else
            {
				if (isNear)
				{
					PickUpObject();
				}
			}
		}

		Movement();

	}

	private void SetUp()
    {
		heldSpot = gameObject.transform.GetChild(0).gameObject;
		m_Rb = GetComponent<Rigidbody>();

		//Check for save file, if player was already holding an object during last save than pick up the ball
		if (isHolding)
		{
			PickUpObject();
		}
	}

	//Dictates movement in the arena. Rotates the player capsule to aim the ball
	private void Movement()
    {
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		Vector3 movement = new Vector3(playerInput.x, 0, playerInput.y);

		if (movement == Vector3.zero)
		{
			return;
		}

		Quaternion targetRotation = Quaternion.LookRotation(movement);

		targetRotation = Quaternion.RotateTowards(
			transform.rotation,
			targetRotation,
			360 * Time.deltaTime);

		transform.rotation = targetRotation;

		Vector3 desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

		velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, 1f);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, 1f);

		Vector3 displacement = velocity * Time.deltaTime;
		Vector3 newPosition = transform.localPosition + displacement;

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

	//Picks up the ball. Sets the ball's gravity to false and kinematic to true to ensure it stays on spot properly
	private void PickUpObject()
    {
		if(sphere != null)
        {
			sphere.GetComponent<Rigidbody>().useGravity = false;
			sphere.GetComponent<Rigidbody>().isKinematic = true;
			sphere.transform.SetParent(heldSpot.transform);
			sphere.transform.localPosition = heldSpot.transform.localPosition;
			isHolding = true;
		}			

    }	
	
	//Throws ball. Resets the ball's original rigidbody
	private void ThrowObject()
    {
		if(sphere != null)
        {
			sphere.GetComponent<Rigidbody>().useGravity = true;
			sphere.GetComponent<Rigidbody>().isKinematic = false;
			sphere.transform.parent = null;
			sphere.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
			isHolding = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Sphere")
        {
			isNear = true;
		}
	}

    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "Sphere")
		{
			isNear = false;
		}
	}

	//Saves players current position, rotation, and whether it was holding a ball last
	public override void Save(GameDataWriter writer)
	{
		writer.Write(transform.localPosition);
		writer.Write(transform.localRotation);
		writer.Write(gameObject.GetComponent<PlayerController>().isHolding);
		writer.Write(gameObject.GetComponent<PlayerController>().isNear);
	}

	//Loads in players current position, rotation, and whether it was holding a ball last
	public override void Load(GameDataReader reader)
	{
		transform.localPosition = reader.ReadVector3();
		transform.localRotation = reader.ReadQuaternion();
		gameObject.GetComponent<PlayerController>().isHolding = reader.ReadBool();
		gameObject.GetComponent<PlayerController>().isNear = reader.ReadBool();
	}
}
