/****************************************
	BreakableObject Editor v1.09			
	Copyright 2013 Unluck Software	
 	www.chemicalbliss.com																																				
*****************************************/
@CustomEditor (BreakableObject)
@CanEditMultipleObjects

class BreakableObjectEditor extends Editor {
    function OnInspectorGUI () {
    	EditorGUILayout.LabelField("Drag & Drop", EditorStyles.miniLabel);
    	target.fragments = EditorGUILayout.ObjectField("Fractured Object Prefab", target.fragments, Transform ,false );
    	target.breakParticles = EditorGUILayout.ObjectField("Particle System Prefab", target.breakParticles, ParticleSystem ,false);
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Seconds before removing fragment colliders (zero = never)", EditorStyles.miniLabel);   	
    	target.waitForRemoveCollider = EditorGUILayout.FloatField("Remove Collider Delay" , target.waitForRemoveCollider);
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Seconds before removing fragment rigidbodies (zero = never)", EditorStyles.miniLabel);   	
    	target.waitForRemoveRigid = EditorGUILayout.FloatField("Remove Rigidbody Delay" , target.waitForRemoveRigid);	
    	EditorGUILayout.Space();
  		EditorGUILayout.LabelField("Seconds before removing fragments (zero = never)", EditorStyles.miniLabel);   	
    	target.waitForDestroy = EditorGUILayout.FloatField("Destroy Fragments Delay" , target.waitForDestroy);	
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("Force applied to fragments after object is broken", EditorStyles.miniLabel);   
    	target.explosiveForce = EditorGUILayout.FloatField("Fragment Force" , target.explosiveForce);
    	EditorGUILayout.Space();
    	EditorGUILayout.LabelField("How hard must object be hit before it breaks", EditorStyles.miniLabel);   	
    	target.durability = EditorGUILayout.FloatField("Object Durability" , target.durability);	
    	target.mouseClickDestroy = EditorGUILayout.Toggle("Click To Break Object" , target.mouseClickDestroy);
        if (GUI.changed)
            EditorUtility.SetDirty (target);
    }
}