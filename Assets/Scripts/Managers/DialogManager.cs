using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

namespace burglar.managers
{
    [Serializable]
    public class Emotion
    {
        public string emotion;
        public Sprite sprite;
    }
    
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance { get; private set; }

        public GameObject DialogPanel;
        public GameObject TextBox;
        public GameObject CustomButton;
        public GameObject AnswerContainer;
        public GameObject WaitingForContinueButton;
        public Image CharacterImage;
        public bool isTalking = false;

        Text nametag;
        Text message;
        List<string> tags;
        static Choice choiceSelected;

        private Story _story;

        public bool WaitingForAnswer = false;
        public bool isAnswerMode = false;
        public bool isInDialog;

        private RectTransform _dialogPanelRectTransform;
        public Vector2 DialogOffScreenPosition; // Position when the dialog is off-screen
        private Vector2 _dialogOnScreenPosition;
        [SerializeField] private AnimationCurve _animationDialogStart = new AnimationCurve();
        [SerializeField] private float _slideDuration = 0.8f;
        
        // InkfileCaughtAgent
        public TextAsset inkFileCaughtAgent;

        private Coroutine _dialogTypingCoroutine;
        private Coroutine _choicesCoroutine;
        private Coroutine _advanceDialogueCoroutine;
        private Coroutine _slideInCoroutine;
        private Coroutine _advanceFromDecisionCoroutine;
        
        public List<Emotion> AvailableEmotions;
        
        public string currentEmotion = "happy";

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            ResetDialogBox();

            _dialogPanelRectTransform = DialogPanel.GetComponent<RectTransform>();

            // Get current resolution
            var currentResolution = Screen.currentResolution;

            DialogOffScreenPosition = new Vector2(0, -currentResolution.height);

            // Store Dialog Position
            _dialogOnScreenPosition = _dialogPanelRectTransform.anchoredPosition;

            // Move it, to hide it
            _dialogPanelRectTransform.anchoredPosition = DialogOffScreenPosition;

            // Hide WaitingForAnswerButton initially
            if (WaitingForContinueButton != null)
            {
                WaitingForContinueButton.SetActive(false);
            }
        }

        private IEnumerator SlideIn()
        {
            var elapsedTime = 0f;
            var startingPosition = DialogOffScreenPosition;

            // Play sound effect
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundDialogSlideIn);
            
            // Start sliding animation
            while (elapsedTime < _slideDuration)
            {
                _dialogPanelRectTransform.anchoredPosition = Vector2.Lerp(startingPosition, _dialogOnScreenPosition,
                    _animationDialogStart.Evaluate(elapsedTime / _slideDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _dialogPanelRectTransform.anchoredPosition = _dialogOnScreenPosition;
            
            _slideInCoroutine = null;
        }

        public IEnumerator StartDialog()
        {
            // Move DialogPanel out of the screen
            if (DialogPanel is null) yield break;
            
            // Trigger DialogStartEvent
            EventManager.OnDialogStart();
            
            isInDialog = true;

            DialogPanel.SetActive(true);
            
            _slideInCoroutine = StartCoroutine(SlideIn());
            yield return _slideInCoroutine;
                
            _advanceDialogueCoroutine = StartCoroutine(AdvanceDialogue());
            yield return _advanceDialogueCoroutine;
        }

        public void SetStory(Story story)
        {
            _story = story;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || WaitingForAnswer || isTalking || _story == null || _slideInCoroutine != null) return;
            
            // Is there more to the story? (outside answers needed)
            if (_story.canContinue)
            {
                WaitingForContinueButton.SetActive(false);
                nametag.text = "Andy";
                _advanceDialogueCoroutine = StartCoroutine(AdvanceDialogue());

                // Are there any choices?
                if (_story.currentChoices.Count != 0)
                {
                    _choicesCoroutine = StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }

        // Finished the Story (Dialogue)
        private void FinishDialogue()
        {
            DialogPanel.SetActive(false);

            _story = null;
            isInDialog = false;

            StopAllDialogCoroutines();

            ResetDialogBox();
            
            // Trigger DialogEndEvent
            EventManager.OnDialogEnd();
        }

        private void StopAllDialogCoroutines()
        {
            // Stop all dialog coroutines
            if (_dialogTypingCoroutine != null)
            {
                StopCoroutine(_dialogTypingCoroutine);
            }
            
            if (_choicesCoroutine != null)
            {
                StopCoroutine(_choicesCoroutine);
            }
            
            if (_advanceDialogueCoroutine != null)
            {
                StopCoroutine(_advanceDialogueCoroutine);
            }
            
            if (_slideInCoroutine != null)
            {
                StopCoroutine(_slideInCoroutine);
            }
            
            if (_advanceFromDecisionCoroutine != null)
            {
                StopCoroutine(_advanceFromDecisionCoroutine);
            }
        }

        // Advance through the story 
        private IEnumerator AdvanceDialogue()
        {
            if (!_story.canContinue) yield break;
            
            var currentSentence = _story.Continue();

            ParseTags();
            StopAllDialogCoroutines();

            // Store typeSentence coroutine in _dialogTypingCoroutine and yield return at the same time
            _dialogTypingCoroutine = StartCoroutine(TypeSentence(currentSentence));
            
            yield return _dialogTypingCoroutine;
            
            _dialogTypingCoroutine = StartCoroutine(ShowChoices());
            
            yield return _dialogTypingCoroutine;
            
            yield return null;
        }

        // Type out the sentence letter by letter and make character idle if they were talking
        private IEnumerator TypeSentence(string sentence)
        {
            isTalking = true;
            nametag.text = "Andy";
            message.text = "";
            
            // Play sound effect
            AudioManager.Instance.sfxAudioSource.clip = AudioManager.Instance.soundTyping;
            // Loop
            AudioManager.Instance.sfxAudioSource.loop = true;
            AudioManager.Instance.sfxAudioSource.Play();
            
            foreach (var letter in sentence.ToCharArray())
            {
                message.text += letter;
                yield return null;
            }
            
            AudioManager.Instance.sfxAudioSource.Stop();
            AudioManager.Instance.sfxAudioSource.loop = false;
            
            /*CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();
            if (tempSpeaker.isTalking)
            {
                SetAnimation("idle");
            }*/
            
            isTalking = false;
            yield return null;
            
        }

        // Create then show the choices on the screen until one got selected
        private IEnumerator ShowChoices()
        {
            // Destroy all children in the AnswerContainer
            for (var i = AnswerContainer.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(AnswerContainer.transform.GetChild(i).gameObject);
            }

            var choices = _story.currentChoices;

            if (choices.Count > 0)
            {
                foreach (var t in choices)
                {
                    var temp = Instantiate(CustomButton, AnswerContainer.transform);
                    temp.transform.GetComponentInChildren<TextMeshProUGUI>().text = t.text;
                    temp.AddComponent<burglar.utility.Selectable>();
                    temp.GetComponent<burglar.utility.Selectable>().element = t;
                    temp.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        temp.GetComponent<burglar.utility.Selectable>().Decide();
                    });
                }

                WaitingForAnswer = true;
            }
            else
            {
                WaitingForContinueButton.SetActive(true);
                WaitingForAnswer = false;
            }

            AnswerContainer.SetActive(true);

            yield return new WaitUntil(() => choiceSelected != null);

            _advanceFromDecisionCoroutine = StartCoroutine(AdvanceFromDecision());
            yield return _advanceDialogueCoroutine;
        }

        // Tells the story which branch to go to
        public static void SetDecision(object element)
        {
            choiceSelected = (Choice)element;
            // TutoManager.Instance.GetStory().ChooseChoiceIndex(choiceSelected.index);
            Instance._story.ChooseChoiceIndex(choiceSelected.index);
            Instance.WaitingForAnswer = false;
        }

        // After a choice was made, turn off the panel and advance from that choice
        private IEnumerator AdvanceFromDecision()
        {
            AnswerContainer.SetActive(false);
            for (var i = 0; i < AnswerContainer.transform.childCount; i++)
            {
                Destroy(AnswerContainer.transform.GetChild(i).gameObject);
            }

            // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
            choiceSelected = null; 

            _advanceDialogueCoroutine = StartCoroutine(AdvanceDialogue());
            yield return _advanceDialogueCoroutine;
        }

        /*** Tag Parser ***/
        /// In Inky, you can use tags which can be used to cue stuff in a game.
        /// This is just one way of doing it. Not the only method on how to trigger events. 
        void ParseTags()
        {
            tags = _story.currentTags;
            
            var emotion = tags.Find(tag => tag.StartsWith("emotion:"));
            if (emotion != null)
            {
                SetEmotion(emotion);
            }
        }

        private void SetEmotion(string emotion)
        {
            var newEmotion = emotion.Split(':')[1];
            
            if (currentEmotion != newEmotion)
            {
                if (AvailableEmotions.Find(e => e.emotion == newEmotion) is { } foundEmotion)
                {
                    currentEmotion = newEmotion;
                    CharacterImage.sprite = foundEmotion.sprite;
                }
                else
                {
                    Debug.LogWarning($"Emotion {newEmotion} not found in the list of available emotions");
                }
            }
        }

        private void SetAnimation(string _name)
        {
            // CharacterScript cs = GameObject.FindObjectOfType<CharacterScript>();
            // cs.PlayAnimation(_name);
        }

        private void SetTextColor(string _color)
        {
            switch (_color)
            {
                case "red":
                    message.color = Color.red;
                    break;
                case "blue":
                    message.color = Color.cyan;
                    break;
                case "green":
                    message.color = Color.green;
                    break;
                case "white":
                    message.color = Color.white;
                    break;
                default:
                    Debug.LogWarning($"{_color} is not available as a text color");
                    break;
            }
        }

        private void ResetDialogBox()
        {
            nametag = TextBox.transform.GetChild(0).GetComponent<Text>();
            message = TextBox.transform.GetChild(1).GetComponent<Text>();
            // tags = new List<string>();
            choiceSelected = null;
            
            WaitingForContinueButton.SetActive(false);

            nametag.text = "";
            message.text = "";
        }
        
        public IEnumerator StartDialogAndWait()
        {
            StartCoroutine(StartDialog());

            // Wait until the dialogue is finished
            while (isInDialog)
            {
                yield return null;
            }
        }
    }
}