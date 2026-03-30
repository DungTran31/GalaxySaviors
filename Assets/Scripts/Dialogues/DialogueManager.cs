using UnityEngine;
using DungTran31.Utilities;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DungTran31.Core;
using DungTran31.Audio;

namespace DungTran31.Dialogues
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Header("Params")]
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Load Globals JSON")]
        [SerializeField] private TextAsset loadGlobalsJSON;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject notiText; 
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private GameObject continueIcon;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private TextMeshProUGUI displayNameText;
        [SerializeField] private Animator portraitAnimator;
        private Animator layoutAnimator;

        [Header("Choices UI")]
        [SerializeField] private GameObject[] choices;
        private TextMeshProUGUI[] choicesText;

        [Header("Audio")]
        [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
        [SerializeField] private DialogueAudioInfoSO[] audioInfos;
        [SerializeField] private bool makePredictable;
        private DialogueAudioInfoSO currentAudioInfo;
        private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
        private AudioSource audioSource;

        private Story currentStory;
        private Coroutine displayLineCoroutine;
        private DialogueVariables dialogueVariables;

        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";
        private const string LAYOUT_TAG = "layout";
        private const string AUDIO_TAG = "audio";

        public int DialogueCount { get; private set; }
        public bool DialogueIsPlaying { get; private set; }
        private bool canContinueToNextLine = false;

        protected override void Awake()
        {
            base.Awake();
            if (notiText != null)
            {
                notiText.SetActive(true);
            }
            dialogueVariables = new DialogueVariables(loadGlobalsJSON);

            audioSource = this.gameObject.AddComponent<AudioSource>();
            currentAudioInfo = defaultAudioInfo;
        }

        private void Start()
        {
            DialogueCount = 1;
            DialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            // get the layout animator
            layoutAnimator = dialoguePanel.GetComponent<Animator>();

            // get all of the choices text 
            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            foreach (var choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                index++;
            }

            InitializeAudioInfoDictionary();
        }

        private void InitializeAudioInfoDictionary()
        {
            audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>
            {
                { defaultAudioInfo.id, defaultAudioInfo }
            };
            foreach (DialogueAudioInfoSO audioInfo in audioInfos)
            {
                audioInfoDictionary.Add(audioInfo.id, audioInfo);
            }
        }

        private void SetCurrentAudioInfo(string id)
        {
            audioInfoDictionary.TryGetValue(id, out DialogueAudioInfoSO audioInfo);
            if (audioInfo != null)
            {
                this.currentAudioInfo = audioInfo;
            }
            else
            {
                Debug.LogWarning("Failed to find audio info for id: " + id);
            }
        }

        private void Update()
        {
            if(!DialogueIsPlaying)
            {
                return;
            }

            // handle continuing to the next line in the dialogue when submit is pressed
            if (canContinueToNextLine
                && currentStory.currentChoices.Count == 0
                && InputManager.Instance.GetSubmitPressed())
            {
                ContinueStory();
            }
        }

        public void EnterDialogueMode(TextAsset inkJSON)
        {
            currentStory = new Story(inkJSON.text);
            if(notiText != null)
            {
                notiText.SetActive(false);
            }
            dialoguePanel.SetActive(true);
            DialogueIsPlaying = true;
            dialogueVariables.StartListening(currentStory);

            // reset portrait, layout, and speaker
            displayNameText.text = "???";
            portraitAnimator.Play("default");
            layoutAnimator.Play("right");

            ContinueStory();
        }
        
        private IEnumerator ExitDialogueMode()
        {
            yield return new WaitForSeconds(0.2f);

            DialogueCount--;
            dialogueVariables.StopListening(currentStory);
            DialogueIsPlaying = false;
            dialoguePanel.SetActive(false);
            dialogueText.text = "";

            // go back to default audio
            SetCurrentAudioInfo(defaultAudioInfo.id);
        }

        private void ContinueStory()
        {
            if (currentStory.canContinue)
            {
                // set text for the current dialogue line
                if (displayLineCoroutine != null)
                {
                    StopCoroutine(displayLineCoroutine);
                }
                string nextLine = currentStory.Continue();
                // handle case where the last line is an external function
                if (nextLine.Equals("") && !currentStory.canContinue)
                {
                    StartCoroutine(ExitDialogueMode());
                }
                // otherwise, handle the normal case for continuing the story
                else
                {
                    // handle tags
                    HandleTags(currentStory.currentTags);
                    displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
                }
            }
            else
            {
                StartCoroutine(ExitDialogueMode());
            }
        }

        private IEnumerator DisplayLine(string line)
        {
            // Ensure text wrapping is enabled
            dialogueText.enableWordWrapping = true;
            dialogueText.overflowMode = TextOverflowModes.Overflow;

            // set the text to the full line, but set the visible characters to 0
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;
            // hide items while text is typing
            continueIcon.SetActive(false);
            HideChoices();

            canContinueToNextLine = false;

            bool isAddingRichTextTag = false;

            // display each letter one at a time
            foreach (char letter in line.ToCharArray())
            {
                // if the submit button is pressed, finish up displaying the line right away
                if (InputManager.Instance.GetSubmitPressed())
                {
                    dialogueText.maxVisibleCharacters = line.Length;
                    break;
                }

                // check for rich text tag, if found, add it without waiting
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    if (letter == '>')
                    {
                        isAddingRichTextTag = false;
                    }
                }
                // if not rich text, add the next letter and wait a small time
                else
                {
                    PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                    dialogueText.maxVisibleCharacters++;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }

            // actions to take after the entire line has finished displaying
            continueIcon.SetActive(true);
            DisplayChoices();

            canContinueToNextLine = true;
        }


        private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
        {
            // set variables for the below based on our config
            AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
            int frequencyLevel = currentAudioInfo.frequencyLevel;
            float minPitch = currentAudioInfo.minPitch;
            float maxPitch = currentAudioInfo.maxPitch;
            bool stopAudioSource = currentAudioInfo.stopAudioSource;

            // play the sound based on the config
            if (currentDisplayedCharacterCount % frequencyLevel == 0)
            {
                if (stopAudioSource)
                {
                    audioSource.Stop();
                }
                AudioClip soundClip;
                // create predictable audio from hashing
                if (makePredictable)
                {
                    int hashCode = currentCharacter.GetHashCode();
                    // sound clip
                    int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                    soundClip = dialogueTypingSoundClips[predictableIndex];
                    // pitch
                    int minPitchInt = (int)(minPitch * 100);
                    int maxPitchInt = (int)(maxPitch * 100);
                    int pitchRangeInt = maxPitchInt - minPitchInt;
                    // cannot divide by 0, so if there is no range then skip the selection
                    if (pitchRangeInt != 0)
                    {
                        int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                        float predictablePitch = predictablePitchInt / 100f;
                        audioSource.pitch = predictablePitch;
                    }
                    else
                    {
                        audioSource.pitch = minPitch;
                    }
                }
                // otherwise, randomize the audio
                else
                {
                    // sound clip
                    int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                    soundClip = dialogueTypingSoundClips[randomIndex];
                    // pitch
                    audioSource.pitch = Random.Range(minPitch, maxPitch);
                }

                // play sound
                audioSource.PlayOneShot(soundClip);
            }
        }

        private void HideChoices()
        {
            foreach (GameObject choiceButton in choices)
            {
                choiceButton.SetActive(false);
            }
        }

        private void HandleTags(List<string> currentTags)
        {
            // loop through each tag and handle it accordingly
            foreach (string tag in currentTags)
            {
                // parse the tag
                string[] splitTag = tag.Split(':');
                if (splitTag.Length != 2)
                {
                    Debug.LogError("Tag could not be appropriately parsed: " + tag);
                }
                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();

                // handle the tag
                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        displayNameText.text = tagValue;
                        break;
                    case PORTRAIT_TAG:
                        portraitAnimator.Play(tagValue);
                        break;
                    case LAYOUT_TAG:
                        layoutAnimator.Play(tagValue);
                        break;
                    case AUDIO_TAG:
                        SetCurrentAudioInfo(tagValue);
                        break;
                    default:
                        Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                        break;
                }
            }
        }

        private void DisplayChoices()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            // defensive check to make sure our UI can support the number of choices coming in
            if (currentChoices.Count > choices.Length)
            {
                Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                    + currentChoices.Count);
            }

            int index = 0;
            // enable and initialize the choices up to the amount of choices for this line of dialogue
            foreach (Choice choice in currentChoices)
            {
                choices[index].SetActive(true);
                choicesText[index].text = choice.text;
                index++;
            }
            // go through the remaining choices the UI supports and make sure they're hidden
            for (int i = index; i < choices.Length; i++)
            {
                choices[i].SetActive(false);
            }
            StartCoroutine(SelectFirstChoice());
        }

        private IEnumerator SelectFirstChoice()
        {
            // Event System requires we clear it first, then wait
            // for at least one frame before we set the current selected object.
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(choices[0]);
        }

        public void MakeChoice(int choiceIndex)
        {
            if (canContinueToNextLine)
            {
                currentStory.ChooseChoiceIndex(choiceIndex);
                InputManager.Instance.RegisterSubmitPressed(); // this is specific to my InputManager script
                ContinueStory();
            }
        }

        public Ink.Runtime.Object GetVariableState(string variableName)
        {
            dialogueVariables.Variables.TryGetValue(variableName, out Ink.Runtime.Object variableValue);
            if (variableValue == null)
            {
                Debug.LogWarning("Ink Variable was found to be null: " + variableName);
            }
            return variableValue;
        }

        // This method will get called anytime the application exits.
        // Depending on your game, you may want to save variable state in other places.
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            dialogueVariables?.SaveVariables();
        }
    }
}
