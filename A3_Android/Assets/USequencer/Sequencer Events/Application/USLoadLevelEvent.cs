using UnityEngine;
using System.Collections;

namespace WellFired
{
	[USequencerFriendlyName("Load Level")]
	[USequencerEvent("Application/Load Level")]
	public class USLoadLevelEvent : USEventBase 
	{
		public bool fireInEditor = false;
		
		public string levelName = "";
		public int levelIndex = -1;
		
		public override void FireEvent()
		{
			if(levelName.Length == 0 && levelIndex < 0)
			{
				Debug.LogError("You have a Load Level event in your sequence, however, you didn't give it a level to load.");
				return;
			}
			
			if(levelIndex >= Application.levelCount)
			{
				Debug.LogError("You tried to load a level that is invalid, the level index is out of range.");
				return;
			}
			
			if(!Application.isPlaying && !fireInEditor)
			{
				Debug.Log("Load Level Fired, but it wasn't processed, since we are in the editor. Please set the fire In Editor flag in the inspector if you require this behaviour.");
				return;
			}
			
			if(levelName.Length != 0)
				Application.LoadLevel(levelName);
		
			if(levelIndex != -1)
				Application.LoadLevel(levelIndex);
		}
			
		public override void ProcessEvent(float deltaTime)
		{
	
		}
	}
}