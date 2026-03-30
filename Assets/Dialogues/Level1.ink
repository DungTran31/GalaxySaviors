INCLUDE globals.ink

{ player_nickname == "": -> boss_defeat | -> already_chose }

=== boss_defeat ===
The mighty foe lies defeated at your feet. The residents gather around, their faces filled with gratitude and awe. One of the elders steps forward. #speaker:Narrator #layout:left #audio:1_3_raw
"Your bravery has saved us all. From now on, you shall be known by a name fitting of your heroism. What shall we call you?"

+ [The Savior]
    ~ player_nickname = "The Savior"
    The elder nods. "The Savior it is. May your deeds be remembered for generations to come."
    -> END
+ [The Guardian]
    ~ player_nickname = "The Guardian"
    The elder smiles. "The Guardian, protector of our land. A title well deserved."
    -> END
+ [The Champion]
    ~ player_nickname = "The Champion"
    The elder raises a hand in salute. "The Champion! Your strength and courage are unmatched."
    -> END

=== already_chose ===
You already have a name: {player_nickname}.
-> END
