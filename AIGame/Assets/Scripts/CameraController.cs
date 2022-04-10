using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }

    public static UnityEvent onTransitionBegin = new UnityEvent();
    public static UnityEvent onTransitionComplete = new UnityEvent();

    private void Awake()
    {
        Instance = this;
    }

    public static void MoveTo(Vector2 position, float time)
    {
        if (isMovingTowards == null)
            isMovingTowards = Instance.StartCoroutine(Instance.MoveTowards(position, time));
	}

	private static Coroutine isMovingTowards = null;
	private IEnumerator MoveTowards(Vector2 position, float time)
    {
        onTransitionBegin?.Invoke();

        float currentTime = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(position.x, position.y, transform.position.z);
		
        while(currentTime < time)
        {
			transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(currentTime / time));
			yield return null;
            currentTime += Time.deltaTime;
        }

        transform.position = endPos;
        isMovingTowards = null;

        onTransitionComplete?.Invoke();
    }
}
