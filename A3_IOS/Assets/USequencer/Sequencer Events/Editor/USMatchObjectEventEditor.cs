using UnityEditor;
using UnityEngine;
using System.Collections;

namespace WellFired
{
	[CustomUSEditor(typeof(USMatchObjectEvent))]
	public class USMatchObjectEventEditor : USEventBaseEditor
	{
		public override Rect RenderEvent(Rect myArea)
		{
			USMatchObjectEvent matchObjectEvent = TargetEvent as USMatchObjectEvent;
			
			matchObjectEvent.Duration = matchObjectEvent.inCurve[matchObjectEvent.inCurve.length-1].time;
			
			// Draw our Whole Box.
			if (matchObjectEvent.Duration > 0)
			{
				//float endPosition = USControl.onvertTimeToEventPanePosition(matchObjectEvent.FireTime + matchObjectEvent.Duration);
				//myArea.width = endPosition - myArea.x;
			}
			DrawDefaultBox(myArea);
	
			using(new GUIBeginArea(myArea))
			{
				GUILayout.Label(GetReadableEventName(), DefaultLabel);
				
				if(matchObjectEvent.objectToMatch != null)
					GUILayout.Label(matchObjectEvent.objectToMatch.name, DefaultLabel);
				else
					GUILayout.Label("NULL", DefaultLabel);
			}
	
			return myArea;
		}
	}
}