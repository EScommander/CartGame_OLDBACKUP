using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkSyncedObject : MonoBehaviour 
{
	public double interpolationBackTime = 0.1;

	internal struct State
	{
		internal double timestamp;
		internal Vector3 pos;
		internal Quaternion rot;
	}

	List<State> m_BufferedState = new List<State>();

	int m_TimestampCount;



	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if(stream.isWriting)
		{
			Vector3 pos = transform.localPosition;
			Quaternion rot = transform.localRotation;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);
		}
		else
		{
			Vector3 pos = Vector3.zero;
			Quaternion rot = Quaternion.identity;
			bool drift = false;
			stream.Serialize(ref pos);
			stream.Serialize(ref rot);

			State state;
			state.timestamp = info.timestamp;
			state.pos = pos;
			state.rot = rot;
			m_BufferedState.Insert(0, state);

			//Ensures length doesn't exceed 20 entries
			while(m_BufferedState.Count > 20)
			{
				m_BufferedState.RemoveAt(m_BufferedState.Count - 1);
			}

			m_TimestampCount ++;
			if(m_TimestampCount > m_BufferedState.Count -1)
				m_TimestampCount = m_BufferedState.Count -1;

			for(int i =0; i< m_TimestampCount-1; i++)
			{
				if(m_BufferedState[i].timestamp < m_BufferedState[i+1].timestamp)
					Debug.Log("State inconsistent");
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(!networkView.isMine)
		{
			double currentTime = Network.time;
			double interpolationTime = currentTime - interpolationBackTime;
			// We have a window of interpolationBackTime where we basically play 
			// By having interpolationBackTime the average ping, you will usually use interpolation.
			// And only if no more data arrives we will use extrapolation
			
			// Use interpolation
			// Check if latest state exceeds interpolation time, if this is the case then
			// it is too old and extrapolation should be used
			if (m_BufferedState[0].timestamp > interpolationTime)
			{
				for (int i=0;i<m_TimestampCount;i++)
				{
					// Find the state which matches the interpolation time (time+0.1) or use last state
					if (m_BufferedState[i].timestamp <= interpolationTime || i == m_TimestampCount-1)
					{
						// The state one slot newer (<100ms) than the best playback state
						State rhs = m_BufferedState[Mathf.Max(i-1, 0)];
						// The best playback state (closest to 100 ms old (default time))
						State lhs = m_BufferedState[i];
						
						// Use the time between the two slots to determine if interpolation is necessary
						double length = rhs.timestamp - lhs.timestamp;
						float t = 0.0F;
						// As the time difference gets closer to 100 ms t gets closer to 1 in 
						// which case rhs is only used
						if (length > 0.0001)
							t = (float)((interpolationTime - lhs.timestamp) / length);
						
						// if t=0 => lhs is used directly
						transform.localPosition = Vector3.Lerp(lhs.pos, rhs.pos, t);
						transform.localRotation = Quaternion.Slerp(lhs.rot, rhs.rot, t);
						return;
					}
				}
			}
			// Use extrapolation. Here we do something really simple and just repeat the last
			// received state. You can do clever stuff with predicting what should happen.
			else
			{
				State latest = m_BufferedState[0];
				
				transform.localPosition = latest.pos;
				transform.localRotation = latest.rot;
			}
		}
	}
}
