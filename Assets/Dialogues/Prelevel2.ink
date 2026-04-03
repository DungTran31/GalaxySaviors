INCLUDE globals.ink
{ player_nickname == "":
    ~ player_nickname = "The Savior"
}
-> start
=== start ===
The planet is safe... for now. But something feels wrong. #speaker:Narrator #layout:left #audio:1_3_raw

"We stopped one disaster... but not the cause." #speaker:Bro #portrait:ghost4 #layout:left #audio:low

<b><color=\#FFDF00>{player_nickname}</color></b> look at the star map. Several signals appear. #speaker:Player #portrait:ghost #layout:right #audio:high

+ [Check nearby planet]
    -> next

+ [Ignore and rest]
    You try to rest... but the signals keep blinking. #speaker:Narrator #layout:left #audio:1_3_raw
    -> next

=== next ===
"There are multiple distress signals." #speaker:Bro #portrait:ghost4 #layout:left #audio:low
"Then we don't have time to waste." #speaker:Player #portrait:ghost #layout:right #audio:high

-> END