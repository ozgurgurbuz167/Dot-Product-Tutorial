using UnityEngine;

namespace OzgurGurbuz
{
	/** Dot Product Tutorial
	
	This tutorial demonstrates how the dot product works in Unity.
	
	Instructions:
	1. Create an empty GameObject in the Hierarchy.
	2. Attach this script to the newly created GameObject.
	3. Press Play to see the objects move and the dot product results being displayed.
	
	Features:
	- Two spheres that move back and forth.
	- Two spheres that rotate 180 degrees.
	- Two text displays showing the results based on the dot product.
	
	Enjoy exploring the concept of dot products in 3D space!
	
	*/
	
	public class DotProductTutorial : MonoBehaviour
	{
		[SerializeField][Range(1, 2)]private float speed = 1f; // Speed of the objects
		[SerializeField][Range(-1, 1)]private float firstDotProduct; // Result of the first dot product calculation
		[SerializeField][Range(-1, 1)]private float secondDotProduct; // Result of the second dot product calculation
		
		private GameObject cameraObject;
		
		private GameObject firstExample, objectA, objectB;
		private TextMesh objectAText, objectBText, firstResultText;
		private float moveDistance = 0.5f;
		private float currentDistanceA = 0f, currentDistanceB = 0f;
		private bool movingA = true, movingB = true;
		
		private GameObject secondExample, objectX, objectY;
		private TextMesh objectXText, objectYText, secondResultText;
		private float objectXTargetAngle = 90f, objectYTargetAngle = 90f;
		private bool isObjectYWaiting = false;
		
		private void Awake()
		{
			// The main parent object
			gameObject.name = "Dot Product Tutorial";
			transform.position = Vector3.zero;
			
			#region First Example
			// Create parent object for the first example 
			firstExample = new GameObject("First Example");
			firstExample.transform.SetParent(gameObject.transform);
			firstExample.transform.localPosition = new Vector3(0, 0, 1.75f);
			
			// Create Object A
			objectA = CreateGameObject(firstExample.transform, PrimitiveType.Sphere, new Vector3(-2, 0, 0), Color.red);
			CreateText(out objectAText, "A", objectA.transform, new Vector3(0, 1.5f, 0));		
			
			// Create Object B
			objectB = CreateGameObject(firstExample.transform, PrimitiveType.Sphere, new Vector3(2, 0, 0), Color.blue);	
			CreateText(out objectBText, "B", objectB.transform, new Vector3(0, 1.5f, 0));
			
			// Create result text for the first example
			CreateResultText(out firstResultText, "First Result", firstExample.transform, new Vector3(-4.75f, 3.5f, 2.5f));
			#endregion
			
			#region Second Example
			// Create parent object for the second example 
			secondExample = new GameObject("Second Example");
			secondExample.transform.SetParent(gameObject.transform);
			secondExample.transform.localPosition = new Vector3(0, 0, -1.75f);
			
			// Create Object X
			objectX =  CreateGameObject(secondExample.transform, PrimitiveType.Sphere, new Vector3(-2, 0, 0), Color.green);
			CreateText(out objectXText, "X", objectX.transform, new Vector3(0, 1.5f, 0));
			
			// Create Object Y
			objectY = CreateGameObject(secondExample.transform, PrimitiveType.Sphere, new Vector3(2, 0, 0), Color.yellow);
			CreateText(out objectYText, "Y", objectY.transform, new Vector3(0, 1.5f, 0));
			
			// Create result text for the second example
			CreateResultText(out secondResultText, "Second Result", secondExample.transform, new Vector3(-4.75f, 3.5f, -2.5f));
			#endregion
			
			#region Create Camera
			cameraObject = new GameObject("Tutorial Camera");
			cameraObject.transform.SetParent(gameObject.transform);
			cameraObject.transform.localPosition = new Vector3(0, 9, 0);
			cameraObject.transform.localEulerAngles = new Vector3(90, 0, 0);
			
			Camera cameraComponent = cameraObject.AddComponent<Camera>();
			cameraComponent.orthographic = true;			
			cameraComponent.orthographicSize = 5f;
			#endregion
		}
		
		private void Update()
		{
			#region First Example
			float _speed = speed * 0.15f;
			
			// Update Object A's movement
			if(movingA)
			{
				currentDistanceA += _speed * Time.deltaTime;
				if(currentDistanceA >= moveDistance)
				{
					currentDistanceA = moveDistance;
					movingA = false;
				}
			}
			else
			{
				currentDistanceA -= _speed * Time.deltaTime;
				if(currentDistanceA <= -moveDistance)
				{
					currentDistanceA = -moveDistance;
					movingA = true;
				}
			}
			
			// Update Object B's movement
			if(movingB)
			{
				currentDistanceB += _speed * Time.deltaTime;
				if(currentDistanceB >= moveDistance)
				{
					currentDistanceB = moveDistance;
					movingB = false;
				}
			}
			else
			{
				currentDistanceB -= _speed * Time.deltaTime;
				if(currentDistanceB <= -moveDistance)
				{
					currentDistanceB = -moveDistance;
					movingB = true;
				}
			}
						
			// Move the objects using their forward direction
			objectA.transform.position += objectA.transform.forward * _speed * Time.deltaTime * (movingA ? 1 : -1);
			objectB.transform.position += -objectB.transform.forward *  _speed * Time.deltaTime * (movingB ? 1 : -1);
			#endregion
			
			#region Second Example
			float objectXCurrentY = objectX.transform.localEulerAngles.y;
			float objectXTargetY = Mathf.MoveTowardsAngle(objectXCurrentY, objectXTargetAngle, (speed * 16) * Time.deltaTime);
			objectX.transform.localEulerAngles = new Vector3(objectX.transform.localEulerAngles.x, objectXTargetY, objectX.transform.localEulerAngles.z);

			if (Mathf.Abs(objectXTargetY - objectXTargetAngle) < 0.1f)
			{
				objectXTargetAngle = objectXTargetAngle == 90f ? -90f : 90f;
			}
			
			if (!isObjectYWaiting)
			{
				float objectYCurrentY = objectY.transform.localEulerAngles.y;
				float objectYTargetY = Mathf.MoveTowardsAngle(objectYCurrentY, objectYTargetAngle, (speed * 16) * Time.deltaTime);
				objectY.transform.localEulerAngles = new Vector3(objectY.transform.localEulerAngles.x, objectYTargetY, objectY.transform.localEulerAngles.z);

				if (Mathf.Abs(objectYTargetY - objectYTargetAngle) < 0.1f)
				{
					objectYTargetAngle = objectYTargetAngle == 90f ? -90f : 90f;

					if (Mathf.Abs(objectX.transform.localEulerAngles.y - 270f) > 0.1f)
					{
						isObjectYWaiting = true;
					}
				}
			}
			else
			{
				if (Mathf.Abs(objectX.transform.localEulerAngles.y - 270f) < 0.1f)
				{
					isObjectYWaiting = false;
				}
			}
			#endregion
			
			#region Hold The Texts In Place
			objectXText.transform.eulerAngles = new Vector3(objectXText.transform.eulerAngles.x, secondResultText.transform.eulerAngles.y, objectYText.transform.eulerAngles.z);
			objectYText.transform.eulerAngles = new Vector3(objectYText.transform.eulerAngles.x, secondResultText.transform.eulerAngles.y, objectYText.transform.eulerAngles.z);
			#endregion
			
			// Perform dot product calculations
			CalculateDotProduct();
		}
		
		// Helper function to create GameObjects
		private GameObject CreateGameObject(Transform _parent, PrimitiveType _type, Vector3 _position, Color _color)
		{
			GameObject _go = GameObject.CreatePrimitive(_type);
			_go.transform.SetParent(_parent);
			_go.transform.localPosition = _position;
			_go.GetComponent<Renderer>().material.color = _color;
			CreateForwardAxis(_go.transform);
			
			return _go;
		}
		
		// Helper function to create text objects
		private void CreateText(out TextMesh textMesh, string _name, Transform _parent, Vector3 _position)
		{
			string gameObjectName = _name + " (Text)";
			string textName = _name;
			
			GameObject textObject = new GameObject(gameObjectName);
			textObject.transform.SetParent(_parent);
			textMesh = textObject.AddComponent<TextMesh>();
			textMesh.text = textName;
			textMesh.fontSize = 500;
			textMesh.color = Color.white;
			textMesh.alignment = TextAlignment.Center;
			textMesh.anchor = TextAnchor.MiddleCenter;
			
			textObject.transform.localPosition = _position;
			textObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			textObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		}
		
		// Helper function to create result text objects
		private void CreateResultText(out TextMesh textMesh, string _name, Transform _parent, Vector3 _position)
		{
			string gameObjectName = _name + " (Text)";
			string textName = "Object: " + _name;
			
			GameObject textObject = new GameObject(gameObjectName);
			textObject.transform.SetParent(_parent);
			textMesh = textObject.AddComponent<TextMesh>();
			textMesh.text = textName;
			textMesh.fontSize = 500;
			textMesh.color = Color.white;
			textMesh.alignment = TextAlignment.Center;
			textMesh.anchor = TextAnchor.MiddleLeft;
			
			textObject.transform.localPosition = _position;
			textObject.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
			textObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		}
		
		// Helper function to create forward axis indicator
		private void CreateForwardAxis(Transform parent)
		{
			GameObject forwardCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			forwardCube.transform.SetParent(parent);
			forwardCube.transform.localPosition = new Vector3(0, 0, 0.6f);
			forwardCube.transform.localScale = new Vector3(0.04f, 0.04f, 0.6f);
			forwardCube.GetComponent<Renderer>().material.color = Color.blue;
		}
		
		// Function to calculate the dot product and update text results
		private void CalculateDotProduct()
		{
			#region First Example
			Vector3 directionA = objectA.transform.forward; // Direction of Object A
			Vector3 directionB = (objectB.transform.position - objectA.transform.position).normalized; // Direction to Object B

			firstDotProduct = Vector3.Dot(directionA, directionB); // Calculate the first dot product
			
			if(firstDotProduct < 0) { firstResultText.text = "1st Result: A's front is pointing away from B."; }
			else if(firstDotProduct > 0) { firstResultText.text = "1st Result: A's front is pointing towards B."; }
			else { firstResultText.text = "1st Result: The directions of A and B are perpendicular to each other, indicating they are positioned at a 90-degree angle relative to each other."; }
			#endregion
			
			#region Second Example
			Vector3 directionX = objectX.transform.forward; // Direction of Object X
			Vector3 directionY = (objectY.transform.position - objectX.transform.position).normalized; // Direction to Object Y

			secondDotProduct = Vector3.Dot(directionX, directionY); // Calculate the second dot product
			
			if(secondDotProduct < 0) { secondResultText.text = "2nd Result: X's front is pointing away from Y"; }
			else if(secondDotProduct > 0){ secondResultText.text = "2nd Result: X's front is pointing towards Y"; }
			else { secondResultText.text = "2nd Result: The directions of X and Y are perpendicular to each other, indicating they are positioned at a 90-degree angle relative to each other"; }
			#endregion
		}
	}
}
