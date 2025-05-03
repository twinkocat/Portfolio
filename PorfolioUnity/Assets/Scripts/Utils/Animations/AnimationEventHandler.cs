using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]
public class AnimationEventHandler : MonoBehaviour
{
    public string namePrefix = "My";
    public string assetPath = "Scripts/GENERATED/";
    public AnimationEventData[] animationEvents; 

    [ContextMenu("Generate Animation Event Handler")]
    public void GenerateSource()
    {
        if (string.IsNullOrEmpty(namePrefix))
        {
            Debug.LogError("Name Prefix is empty!");
            return;
        }

        StringBuilder source = new();

        source.AppendLine($@"
using ZLinq;
using UnityEngine;

// Auto-generated animation event handler methods
public class {namePrefix}AnimationEventHandler : MonoBehaviour
{{
    private AnimationEventHandler animationEventHandler;

    private void Awake() 
    {{
        animationEventHandler = GetComponent<AnimationEventHandler>();
    }}
");

        foreach (AnimationEventData animationEvent in animationEvents)
        {
            source.AppendLine($@"
    public void {animationEvent.eventName}(AnimationEvent animationEvent)
    {{
        if (animationEvent.animatorClipInfo.weight > {animationEvent.weight}f)
        {{
            var wrapperEvent = animationEventHandler.animationEvents.AsValueEnumerable().FirstOrDefault(x => x.eventName == ""{animationEvent.eventName}"");
            if (wrapperEvent == null) return;
            wrapperEvent.@event.Invoke();
        }}
    }}
");
        }

        source.AppendLine("}");

        string folderPath = Path.Combine(Application.dataPath, assetPath);
        string fullPath = Path.Combine(Application.dataPath, $"{assetPath}{namePrefix}AnimationEventHandler.cs");
        if (!File.Exists(assetPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText(fullPath, source.ToString());
        Debug.Log($"Animation Event Handler script generated at: {fullPath}");
    }
}

[System.Serializable]
public class AnimationEventData
{
    public string eventName;
    public float weight;
    public UnityEvent @event;
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimationEventHandler))]
public class AnimationEventHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AnimationEventHandler generator = (AnimationEventHandler)target;

        if (GUILayout.Button("Generate Animation Event Handler"))
        {
            generator.GenerateSource();
            AssetDatabase.Refresh();
        }
    }
}

#endif