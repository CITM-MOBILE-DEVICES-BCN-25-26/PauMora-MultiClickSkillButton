using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiSkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	enum ButtonState
	{
		ShortClick,
		DoubleClick,
		LongClick
	}

	CancellationTokenSource cancellationTokenSource;
	CancellationTokenSource doubleClickCancellationTokenSource;
	
	[SerializeField] int timeToShortClick = 200;
	[SerializeField] int millisecondsDoubleClickTime = 100;

	ButtonState buttonState;

	int buttonTimesPressed = 0;

	bool isBusy = false;
	private void OnDisable()
	{
		if (cancellationTokenSource != null) cancellationTokenSource.Cancel();
		if (doubleClickCancellationTokenSource != null) doubleClickCancellationTokenSource.Cancel();
    }

	public async void OnPointerDown(PointerEventData eventData)
	{
		buttonTimesPressed++;
		
		if (buttonTimesPressed == 2)
		{
            doubleClickCancellationTokenSource.Cancel();
        }

		if (isBusy) return;

		isBusy = true;
	 
		if (cancellationTokenSource != null)
		{
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
        }

		if (doubleClickCancellationTokenSource != null)
		{
            doubleClickCancellationTokenSource.Cancel();
            doubleClickCancellationTokenSource.Dispose();
        }

            cancellationTokenSource = new CancellationTokenSource();
        doubleClickCancellationTokenSource = new CancellationTokenSource();

		buttonState = await ButtonLogic(cancellationTokenSource.Token, doubleClickCancellationTokenSource.Token);

		switch (buttonState)
		{
			case ButtonState.ShortClick:
				Debug.Log("Short Click");
				break;
			case ButtonState.DoubleClick:
				Debug.Log("Double Click");
				break;
			case ButtonState.LongClick:
				Debug.Log("Long Click");
				break;
		}
		
		ButtonReturnIdle();
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if (cancellationTokenSource != null) cancellationTokenSource.Cancel();
	}

	async UniTask<ButtonState> ButtonLogic(CancellationToken taskCancellationToken, CancellationToken doubleCancellationToken)
	{
		float holdTime = 0f;

		while (!taskCancellationToken.IsCancellationRequested)
		{
			holdTime += Time.unscaledDeltaTime;

			await UniTask.Yield();
		}

		if (TimeSpan.FromSeconds(holdTime) > TimeSpan.FromMilliseconds(timeToShortClick))
		{
			return ButtonState.LongClick;
		}
		else 
		{
			await UniTask.Delay(TimeSpan.FromMilliseconds(millisecondsDoubleClickTime), cancellationToken: doubleCancellationToken).SuppressCancellationThrow();

			if (buttonTimesPressed >= 2)
			{
				return ButtonState.DoubleClick;
			}
			else
			{
				return ButtonState.ShortClick;
			}
		}
	}
	private void ButtonReturnIdle()
	{
		buttonTimesPressed = 0;
		isBusy = false;
	}
}
