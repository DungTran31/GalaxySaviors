INCLUDE globals.ink
{ player_nickname == "":
    ~ player_nickname = "The Savior"
}
->boss_defeat_scene3
=== boss_defeat_scene3 ===
The Red Planet trembles as the massive Guardian of Flame falls before you, its fiery core dimming as it takes its final breath. The sky, once filled with swirling embers, begins to clear, revealing a deep, blood-red horizon. #speaker:Narrator #layout:left #audio:1_3_raw

The remaining inhabitants, who had been hiding from the chaos, slowly emerge from their shelters. They approach you cautiously, their eyes wide with a mixture of fear and awe. One of them, an elder with a voice like crackling fire, steps forward. #speaker:Narrator #layout:left #audio:1_3_raw

"Once again, you have proven your strength, <b><color=\#FFDF00>{player_nickname}</color></b>. Our world is free from the chains of destruction, thanks to you," the elder says, bowing deeply. "What will you do now, great warrior?" #speaker:Narrator #layout:left #audio:1_3_raw

+ [We must rebuild this planet.]
    You look around at the ruined landscape. "This world has suffered enough. We must begin the process of rebuilding, ensuring that it never faces such terror again."
    The elder nods. "We will follow your lead. Together, we will restore our home to its former glory." #speaker:Narrator #layout:left #audio:1_3_raw
    -> next_mission
+ [My journey is not over.]
    You shake your head. "There are other worlds out there that need saving. My journey is far from over." #speaker:Narrator #layout:left #audio:1_3_raw
    The elder looks at you with understanding. "You are truly a guardian of the stars. May the winds of fate guide you to your next victory." #speaker:Narrator #layout:left #audio:1_3_raw
    -> next_mission
+ [I seek the truth behind these attacks.]
    You narrow your eyes, deep in thought. "These attacks are not random. I must find out who or what is orchestrating this chaos." #speaker:Narrator #layout:left #audio:1_3_raw
    The elder places a hand on your shoulder. "The path you seek is dangerous, but if anyone can uncover the truth, it is you, {player_nickname}. Go with our blessings." #speaker:Narrator #layout:left #audio:1_3_raw
    -> next_mission

=== next_mission ===
As you prepare to leave the Red Planet, the inhabitants gather to see you off. They present you with a gift, a small token of their gratitude – a relic from their ancient civilization, said to hold the power of the flames that once burned so fiercely here. #speaker:Narrator #layout:left #audio:1_3_raw

You take the relic, feeling its warmth pulsing in your hand. "Thank you," you say, as you board your ship. #speaker:Narrator #layout:left #audio:1_3_raw

The engines roar to life, and soon you are lifting off, the Red Planet growing smaller behind you. The stars stretch out before you, and you know that your journey is far from over. #speaker:Narrator #layout:left #audio:1_3_raw

-> END
