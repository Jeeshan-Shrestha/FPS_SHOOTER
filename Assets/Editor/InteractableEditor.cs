
using UnityEditor;

[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;


        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.promptMessage = EditorGUILayout.TextField("Prompt Message",interactable.promptMessage);
            EditorGUILayout.HelpBox("EventOnlyInteract can only use Unity Events",MessageType.Info);
            if (interactable.gameObject.GetComponent<InteractionEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();

            if (interactable.useEvents)
            {
                if (interactable.gameObject.GetComponent<InteractionEvent>() == null)
                    interactable.gameObject.AddComponent<InteractionEvent>();
            }
            else
            {
                if (interactable.gameObject.GetComponent<InteractionEvent>() != null)
                {
                    DestroyImmediate(interactable.gameObject.GetComponent<InteractionEvent>());
                }
            }
        }
    }
}
