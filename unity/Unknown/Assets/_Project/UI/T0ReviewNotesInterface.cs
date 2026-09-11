using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tide.UI
{
    public sealed class ReviewNotesView
    {
        public string Text, Placeholder, Help, Question, QuestionLabel;
        public Action<string> Changed;
        public Texture2D CardTexture;
    }

    public sealed partial class T0Interface
    {
        InputField reviewEditor;
        bool reviewEditing;
        public event Action<bool> TextEntryChanged;
        public Func<bool> ReviewCompositionActive;
        public bool TextEntryActive => reviewEditing;
        public InputField ReviewEditor => reviewEditor;
        public CanvasGroup ReviewPanelGroup { get; private set; }
        public CanvasGroup ReviewQuestionGroup { get; private set; }

        void RenderReviewNotes(Transform parent, ReviewNotesView model)
        {
            if (model == null) return;
            FlowText(parent, model.Help, 16, ink);
            var box = Panel("review-note-editor", parent, Vector2.zero, Vector2.one, Color.white);
            ReviewPanelGroup = box.gameObject.AddComponent<CanvasGroup>();
            if (model.CardTexture != null)
            {
                var card = Rect("Card paper", box, Vector2.zero, Vector2.one);
                var image = card.gameObject.AddComponent<RawImage>();
                image.texture = model.CardTexture;
                image.raycastTarget = false;
            }
            var height = box.gameObject.AddComponent<LayoutElement>();
            height.minHeight = height.preferredHeight = 190 * scale;
            Text("Draft", box, "", 20, ink, new Vector2(.025f, .045f), new Vector2(.975f, .955f));
            Text("Placeholder", box, model.Placeholder, 20, new Color(.4f, .4f, .4f), new Vector2(.025f, .045f), new Vector2(.975f, .955f));
            var text = box.Find("Draft").GetComponent<Text>();
            text.supportRichText = false;
            text.raycastTarget = false;
            var placeholder = box.Find("Placeholder").GetComponent<Text>();
            placeholder.raycastTarget = false;
            var field = box.gameObject.AddComponent<ReviewNoteInputField>();
            field.CompositionActive = () => ReviewCompositionActive?.Invoke() ?? false;
            field.ExitRequested = EndReviewEditing;
            reviewEditor = field;
            reviewEditor.textComponent = text;
            reviewEditor.placeholder = placeholder;
            reviewEditor.lineType = InputField.LineType.MultiLineNewline;
            reviewEditor.characterLimit = 4096;
            reviewEditor.text = model.Text ?? "";
            reviewEditor.onValueChanged.AddListener(value => model.Changed?.Invoke(value));
            reviewEditor.onEndEdit.AddListener(_ => SetReviewEditing(false));
            var selection = box.gameObject.AddComponent<ReviewEditorSelection>();
            selection.Selected = () => SetReviewEditing(true);
            selection.Deselected = () => SetReviewEditing(false);
            // The explicit edit button enters typing. Rebuilding the screen never activates the field.
            if (!string.IsNullOrEmpty(model.Question))
            {
                var question = Panel("review-note-question-panel", parent, Vector2.zero, Vector2.one, new Color(.96f, .95f, .89f));
                question.gameObject.AddComponent<LayoutElement>().minHeight = 120 * scale;
                ReviewQuestionGroup = question.gameObject.AddComponent<CanvasGroup>();
                Text("QuestionLabel", question, model.QuestionLabel, 15, new Color(.38f, .18f, .07f), new Vector2(.025f, .74f), new Vector2(.975f, .97f));
                Text("Question", question, model.Question, 19, ink, new Vector2(.025f, .05f), new Vector2(.975f, .72f));
                question.Find("QuestionLabel").GetComponent<Text>().raycastTarget = false;
                question.Find("Question").GetComponent<Text>().raycastTarget = false;
            }
        }

        void SetReviewEditing(bool active)
        {
            if (reviewEditing == active) return;
            reviewEditing = active;
            TextEntryChanged?.Invoke(active);
        }

        public void BeginReviewEditing()
        {
            if (reviewEditor == null) return;
            SetReviewEditing(true);
            EventSystem.current?.SetSelectedGameObject(reviewEditor.gameObject);
            reviewEditor.ActivateInputField();
        }

        public void EndReviewEditing()
        {
            if (reviewEditor != null) reviewEditor.DeactivateInputField();
            SetReviewEditing(false);
            if (!Focus("review-note-save")) Focus("review-note-edit");
        }

        void ClearReviewEditor()
        {
            if (reviewEditor != null) reviewEditor.DeactivateInputField();
            SetReviewEditing(false);
            reviewEditor = null;
            ReviewPanelGroup = null;
            ReviewQuestionGroup = null;
        }
    }

    // UGUI's normal Escape path restores its pre-edit text. Notes retain the draft instead.
    public sealed class ReviewNoteInputField : InputField
    {
        public Func<bool> CompositionActive;
        public Action ExitRequested;
        readonly Event pending = new Event();
        public override void OnUpdateSelected(BaseEventData data)
        {
            if (!isFocused) return;
            while (Event.PopEvent(pending))
            {
                if (pending.rawType == EventType.KeyDown)
                {
                    ProcessReviewKey(pending);
                }
                else if ((pending.type == EventType.ValidateCommand || pending.type == EventType.ExecuteCommand) && pending.commandName == "SelectAll")
                {
                    selectionAnchorPosition = 0;
                    selectionFocusPosition = text.Length;
                    UpdateLabel();
                }
            }
            data.Use();
        }

        public void ProcessReviewKey(Event key)
        {
            // macOS can send a character event after the key event already ended editing.
            if (!isFocused) return;
            bool composing = CompositionActive?.Invoke() ?? false;
            if (key.keyCode == KeyCode.Escape || key.keyCode == KeyCode.Tab || key.character == '\t')
            {
                if (!composing) ExitRequested?.Invoke();
                return;
            }
            // Candidate navigation/confirmation belongs to the IME, not to the draft.
            if (composing && (key.character == 0 || key.keyCode == KeyCode.Return || key.keyCode == KeyCode.KeypadEnter)) { UpdateLabel(); return; }
            if (KeyPressed(key) == EditState.Finish) DeactivateInputField();
            UpdateLabel();
        }
    }

    public sealed class ReviewEditorSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public Action Selected, Deselected;
        public void OnSelect(BaseEventData data) => Selected?.Invoke();
        public void OnDeselect(BaseEventData data) => Deselected?.Invoke();
    }
}
