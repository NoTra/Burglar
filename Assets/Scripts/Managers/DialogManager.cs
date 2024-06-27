using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

namespace burglar.managers
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager Instance { get; private set; }

        public GameObject DialogPanel;
        public GameObject TextBox;
        public GameObject CustomButton;
        public GameObject AnswerContainer;
        public GameObject WaitingForContinueButton;
        public bool isTalking = false;

        Text nametag;
        Text message;
        List<string> tags;
        static Choice choiceSelected;

        private Story _story;

        public bool WaitingForAnswer = false;
        public bool isAnswerMode = false;

        private RectTransform _dialogPanelRectTransform;
        public Vector2 DialogOffScreenPosition; // Position when the dialog is off-screen
        private Vector2 _dialogOnScreenPosition;
        [SerializeField] private AnimationCurve _animationDialogStart = new AnimationCurve();
        [SerializeField] private float _slideDuration = 0.8f;


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
        void Start()
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

            while (elapsedTime < _slideDuration)
            {
                _dialogPanelRectTransform.anchoredPosition = Vector2.Lerp(startingPosition, _dialogOnScreenPosition,
                    _animationDialogStart.Evaluate(elapsedTime / _slideDuration));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _dialogPanelRectTransform.anchoredPosition = _dialogOnScreenPosition;
        }

        public IEnumerator StartDialog()
        {
            // Move DialogPanel out of the screen

            if (DialogPanel != null)
            {
                TutoManager.Instance._player._playerInput.DeactivateInput();

                DialogPanel.SetActive(true);
                yield return StartCoroutine(SlideIn());

                yield return StartCoroutine(AdvanceDialogue());

                yield return null;
                Debug.Log("Fin du dialogue !");
            }
        }

        public void SetStory(Story story)
        {
            _story = story;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !WaitingForAnswer)
            {
                // Is there more to the story? (outside answers needed)
                if (_story.canContinue)
                {
                    WaitingForContinueButton.SetActive(false);
                    nametag.text = "Andy";
                    StartCoroutine(AdvanceDialogue());

                    // Are there any choices?
                    if (_story.currentChoices.Count != 0)
                    {
                        StartCoroutine(ShowChoices());
                    }
                }
                else
                {
                    FinishDialogue();
                }
            }
        }

        // Finished the Story (Dialogue)
        private void FinishDialogue()
        {
            DialogPanel.SetActive(false);
            TutoManager.Instance._player._playerInput.ActivateInput();

            StopAllCoroutines();

            ResetDialogBox();
        }

        // Advance through the story 
        private IEnumerator AdvanceDialogue()
        {
            var currentSentence = _story.Continue();

            // ParseTags();
            // StopAllCoroutines();

            yield return StartCoroutine(TypeSentence(currentSentence));
            yield return StartCoroutine(ShowChoices());
        }

        // Type out the sentence letter by letter and make character idle if they were talking
        private IEnumerator TypeSentence(string sentence)
        {
            nametag.text = "Andy";
            message.text = "";
            foreach (var letter in sentence.ToCharArray())
            {
                message.text += letter;
                yield return null;
            }
            /*CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();
            if (tempSpeaker.isTalking)
            {
                SetAnimation("idle");
            }*/

            yield return null;
        }

        // Create then show the choices on the screen until one got selected
        private IEnumerator ShowChoices()
        {
            // Clear AnswerContainer's child
            for (var i = AnswerContainer.transform.childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(AnswerContainer.transform.GetChild(i).gameObject);
            }

            var choices = _story.currentChoices;

            if (choices.Count > 0)
            {
                for (var i = 0; i < choices.Count; i++)
                {
                    GameObject temp = Instantiate(CustomButton, AnswerContainer.transform);
                    temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = choices[i].text;
                    temp.AddComponent<burglar.utility.Selectable>();
                    temp.GetComponent<burglar.utility.Selectable>().element = choices[i];
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

            yield return new WaitUntil(() => { return choiceSelected != null; });

            yield return StartCoroutine(AdvanceFromDecision());
        }

        // Tells the story which branch to go to
        public static void SetDecision(object element)
        {
            choiceSelected = (Choice)element;
            TutoManager.Instance.GetStory().ChooseChoiceIndex(choiceSelected.index);
            DialogManager.Instance.WaitingForAnswer = false;
        }

        // After a choice was made, turn off the panel and advance from that choice
        private IEnumerator AdvanceFromDecision()
        {
            AnswerContainer.SetActive(false);
            for (var i = 0; i < AnswerContainer.transform.childCount; i++)
            {
                Destroy(AnswerContainer.transform.GetChild(i).gameObject);
            }

            choiceSelected =
                null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.

            yield return StartCoroutine(AdvanceDialogue());
        }

        /*** Tag Parser ***/
        /// In Inky, you can use tags which can be used to cue stuff in a game.
        /// This is just one way of doing it. Not the only method on how to trigger events. 
        void ParseTags()
        {
            tags = _story.currentTags;
            foreach (string t in tags)
            {
                var prefix = t.Split(' ')[0];
                var param = t.Split(' ')[1];

                switch (prefix.ToLower())
                {
                    case "anim":
                        SetAnimation(param);
                        break;
                    case "color":
                        SetTextColor(param);
                        break;
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
                    Debug.Log($"{_color} is not available as a text color");
                    break;
            }
        }

        private void ResetDialogBox()
        {
            nametag = TextBox.transform.GetChild(0).GetComponent<Text>();
            message = TextBox.transform.GetChild(1).GetComponent<Text>();
            tags = new List<string>();
            choiceSelected = null;

            nametag.text = "";
            message.text = "";
        }
    }
}