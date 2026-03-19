# PauMora-MultiClickSkillButton

Thresholds:

- Short click < 200 ms
- Long click > 200 ms
- Double click 100 ms from short click

Button progression:

- The button starts as idle.
- When we click the button goes to detecting long press and waits a delay of 200 ms
- If its not canceled then we fire the long click
- If it its cancelled by releasing the button the button enters waiting for double click state which checks if it will be a short click or a double click
- If the button is clicked while in the time of the double click we enter a check to see if the double click will be activated or if its cancelled by triggering a long click

There are two scripts: 
- MultiSkillButton: Handles the logic (the important one)
- ButtonArt: Changes color of the button depending if its hovering, pressed... 
