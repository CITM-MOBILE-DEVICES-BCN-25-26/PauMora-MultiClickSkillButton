using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiSkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	enum ButtonState
	{
		Idle,
		DetectingLongPress,
		WaitingForDoubleClick,
		WaitingForRelease
	}

	CancellationTokenSource cancellationTokenSource;
	
	[SerializeField] int timeToShortClick = 200;
	[SerializeField] int timeToDoubleClick = 100;

	[SerializeField] ButtonState buttonState = ButtonState.Idle;

	float timeButtonPressed;

	private void OnDisable()
	{
		if (cancellationTokenSource != null) cancellationTokenSource.Cancel();
	}

	private void ResetTasks()
	{
		if (cancellationTokenSource != null)
		{
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
			cancellationTokenSource = null;
        }

        cancellationTokenSource = new CancellationTokenSource();
    }

    public void OnPointerDown(PointerEventData eventData)
	{
		switch (buttonState)
		{
			case ButtonState.Idle:
				ResetTasks();
				
				buttonState = ButtonState.DetectingLongPress;
				
				LongClickDetection(cancellationTokenSource.Token).Forget();

				break;
			case ButtonState.WaitingForDoubleClick:

                ResetTasks();

				DoubleClickDetection(cancellationTokenSource.Token).Forget();

				buttonState = ButtonState.WaitingForRelease;

                break;
		}
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		switch (buttonState)
		{
			case ButtonState.DetectingLongPress:

                ResetTasks();

				buttonState = ButtonState.WaitingForDoubleClick;

                DoubleClickOrNormal(cancellationTokenSource.Token).Forget();

				break;

			case ButtonState.WaitingForRelease:
				ResetTasks();

				buttonState = ButtonState.Idle;
				break;
		}

	}

	async UniTask LongClickDetection(CancellationToken taskCancellationToken)
	{
		await UniTask.Delay(TimeSpan.FromMilliseconds(timeToShortClick), cancellationToken: taskCancellationToken);

        Debug.Log("Long Click Detected");
		buttonState = ButtonState.WaitingForRelease;
	}

	async UniTask DoubleClickOrNormal(CancellationToken taskCancellationToken)
	{
        await UniTask.Delay(TimeSpan.FromMilliseconds(timeToDoubleClick), cancellationToken: taskCancellationToken);

        Debug.Log("Short Click Detected");
		buttonState = ButtonState.Idle;
	}
	
	async UniTask DoubleClickDetection(CancellationToken taskCancellationToken)
	{
		await UniTask.Delay(TimeSpan.FromMilliseconds(timeToShortClick));

		if (taskCancellationToken.IsCancellationRequested)
		{
            Debug.Log("Double Click Detected");
        } else
		{
			Debug.Log("Long Click Detected");
        }

		buttonState = ButtonState.Idle;
    }
}