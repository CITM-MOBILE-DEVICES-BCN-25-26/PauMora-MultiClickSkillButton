# PauMora-MultiClickSkillButton

Thresholds:

- Short click < 200 ms
- Long click > 200 ms
- Double click 100 ms from short click

Button progression:

First it checks if the time is bigger than the short click threshold (200 ms)
If its shorter than the short click treshhold then we know its a short or a double click
Then it waits at most 100 ms to see if there is a double click
If there is a double click it stops the delay and instantly returns double click
If there is no double click the delay runs out and a short click is triggered.

There are two scripts: 
- MultiSkillButton: Handles the logic (the important one)
- ButtonArt: Changes color of the button depending if its hovering, pressed... 
