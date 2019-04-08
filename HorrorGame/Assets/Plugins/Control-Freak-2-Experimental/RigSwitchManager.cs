#if false
using UnityEngine;
using System.Collections.Generic;

namespace ControlFreak2
{
[ExecuteInEditMode()]
public class RigSwitchManager : MonoBehaviour 
	{
	public InputRig targetRig;		

	public List<RigSwitchProfile>
		profileList;
		
	public List<AffectedRigSwitch>
		affectedSwitchList;
		


	// ------------------
	public RigSwitchManager() : base()
		{
		this.profileList = new List<RigSwitchProfile>(4);
		}
	

	// ----------------
	void OnEnable()
		{ 
		if (this.targetRig == null)
			this.targetRig = this.GetComponent<InputRig>();
			
		if ((this.targetRig != null) && this.targetRig.CanBeUsed())
			{
			if (this.profileList.Count > 0)
				this.ApplyProfile(0);
			}
		}



	// -----------------
	public void ApplyProfile(string profileName)
		{
		int id = this.GetProfileId(profileName);
		if (id < 0)
			{
#if UNITY_EDITOR
			Debug.LogError("RigSwitchManager [" + this.name + "] dosen't have [" + profileName + "] profile defined!");
#endif			
			return;
			}

		return this.ApplyProfile(id);
		}


	// -----------------
	public void ApplyProfile(int profileId)
		{
		if (this.targetRig == null)
			{
#if UNITY_EDITOR
			Debug.LogError("RigSwitchManager [" + this.name + "] isn't connected to InputRig!");
#endif			
			return;
			}
			
		if ((profileId < 0) || (profileId >= this.profileList.Count))
			{
#if UNITY_EDITOR
			Debug.LogError("RigSwitchManager [" + this.name + "] - out of bounds profile id : " + profileId + " (Max = " + (this.profileList.Count - 1) + ").");
#endif			
			return;
			}

		this.profileList[profileId].Apply(this.targetRig);
		}




	// -----------------
	protected int GetProfileId(string name)
		{
		return this.profileList.FindIndex(x => (x.name.Equals(name, System.StringComparison.OrdinalIgnoreCase)));
		}

		

	// -----------------
	[System.Serializable]
	public class AffectedRigSwitch
		{
		public string 
			swtichName;
		private int 
			switchId;

		// ---------------
		public AffectedRigSwitch()
			{
			this.switchName = string.Empty;
			this.switchId = 0;
			}


		// ------------------
		public void SetState(InputRig rig, bool state)	
			{
			rig.SetSwitchState(this.swtichName, ref this.switchId, state);
			}
		}
		
		

	// --------------------
	[System.Serializable]
	public class RigSwitchProfile
		{	
		public string 
			name;
		public List<bool> 	
			stateList;
			

		// --------------
		public RigSwitchProfile() 
			{
			this.name		= string.Empty;
			this.stateList	= new List<bool>(4);
			}

		// ----------------
		public void Apply(InputRig rig)
			{
			if (rig == null)
				return;

			for (int i = 0; i < this.stateList.Count; ++i)
				this.stateList[i].Apply(rig);
			}
		}


/*
	// --------------------
	[System.Serializable]
	public class RigSwitchProfile
		{	
		public string 
			name;
		public List<SwitchState> 	
			switchList;
			

		// --------------
		public RigSwitchProfile() 
			{
			this.name		= string.Empty;
			this.switchList	= new List<SwitchState>(4);
			}

		// ----------------
		public void Apply(InputRig rig)
			{
			if (rig == null)
				return;

			for (int i = 0; i < this.switchList.Count; ++i)
				this.switchList[i].Apply(rig);
			}
		}
		


	// ----------------
	[System.Serializable]
	public class SwitchState
		{
		public string 
			switchName;	
		public bool
			switchState;
		private int 
			switchId;

		// --------------
		public void Apply(InputRig rig)
			{
			rig.SetSwitchState(this.switchName, ref this.switchId, this.switchState);
			}
		}
	*/

	}
}

#endif
