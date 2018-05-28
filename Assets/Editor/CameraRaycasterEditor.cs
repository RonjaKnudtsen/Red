using UnityEditor;

// TODO consider changing to a property drawer
[CustomEditor(typeof(CameraRaycaster))]
public class CameraRaycasterEditor : Editor {
    bool isLayerPrioritiesUnfolded = true; // store the UI state

    // 1 serialize raycaster ^ cause thats the type. 
    // 2 Then do stuff in memory, third. 
    // 3 Applymodified properties ("de-serialzie")

    public override void OnInspectorGUI() {
        serializedObject.Update(); // Serialize cameraRaycaster instance

        //Draws the folder AND saves the value, true/false
        //saves if the editor is folded out ("drawer with name "layer properties", is opened")
        isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities");

        if (isLayerPrioritiesUnfolded) {
            //Indent the next level. 
            EditorGUI.indentLevel++;
            {
                //here we set the indented items
                BindArraySize(); //Paint array size
                BindArrayElements(); //Paint elements.
            }
            //Here we undent it, closing it. 
            EditorGUI.indentLevel--;
            
        }

        serializedObject.ApplyModifiedProperties(); // De-serialize back to cameraRaycaster (and create undo point)
    }

    void BindArraySize() {
        //1 Find the CURRENT layer priorities size. 
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        // 2 Paint an int field labeled size, store the number of current layer priorities in the size.
        // 3 Set initial value (of the array size field) based on what is in the camera raycasters array. 
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);
        // 4 If it is changed (the user goes in and changes array size). 
        if (requiredArraySize != currentArraySize) {
            // 5 Set the new size.
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

    void BindArrayElements() {
        //1 find current array size
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        //2 Loop through them and set the appropriate string
        for (int i = 0; i < currentArraySize; i++) {
            // Make a string to find the property. 
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
            //LayerField is the list of layers set in the unity editor (where we have walkable etc).
            //Add to the guilayout and also save the value. 
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }
}
