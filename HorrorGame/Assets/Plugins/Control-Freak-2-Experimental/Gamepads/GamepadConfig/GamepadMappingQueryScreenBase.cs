using UnityEngine;
using System.Collections.Generic;
using ControlFreak2;
using System.Runtime.InteropServices;
using System;

namespace ControlFreak2.GamepadOptionsUI
{
abstract public class GamepadMappingQueryScreenBase : GameState
	{
	public GamepadMappingScreenBase	
		config;

	public float
		waitForNeutralTime		= 5,
		waitForConfirmationTime = 2,
		waitForInputTime		= 10;
		//doneMessageTime			= 1;


	protected bool
		queryActive;
	protected float
		//delayElapsed,
		queryElapsed;

	protected  GamepadMappingScreenBase.GamepadElementId
		elemId;

	// --------------
	public enum QueryPhase
		{
		WAITING_FOR_NEUTRAL,
		WAITING_FOR_INPUT,	
		WAITING_FOR_CONFIRMATION,
		//DONE
		}

	// ----------------
	public enum QueryOutcome
		{
		NONE,
		SUCCESS,
		SKIP,
		CANCEL_CONFIG
		}

	protected QueryPhase
		queryPhase;

	protected QueryOutcome
		queryOutcome;




	// -------------------
	public GamepadMappingQueryScreenBase() : base()
		{

		}

	// ----------------------
	public void EnterQuery(GamepadMappingScreenBase config, GamepadMappingScreenBase.GamepadElementId elemId)
		{
		this.elemId = elemId;
		this.config = config;

		config.StartSubState(this);
		}


	// --------------------
	protected override void OnStartState (GameState parentState)
		{
		base.OnStartState(parentState);
		this.queryActive = false;
		this.queryOutcome = QueryOutcome.NONE;
		}


	// ---------------------
	protected void ReturnToConfigScreen()
		{
		switch (this.queryOutcome)
			{
			case QueryOutcome.SUCCESS :
				this.config.OnQuerySuccess();
				break;
			
			case QueryOutcome.SKIP :
				this.config.OnQuerySkip();
				break;

			case QueryOutcome.CANCEL_CONFIG :
				this.config.CancelConfiguration();
				break;
			}
				
		}

	// ------------------
	public bool IsQueryActive()
		{ return this.queryActive; }

	// -------------------	
	public QueryOutcome GetOutcome()
		{ return this.queryOutcome; }


	// ---------------------
	protected void StartActiveQuery()
		{
		this.ChangeQueryPhase(this.config.GetCurGamepadState().IsNeutral() ? 
			QueryPhase.WAITING_FOR_INPUT : QueryPhase.WAITING_FOR_NEUTRAL);

		this.queryActive = true;
		this.queryOutcome = QueryOutcome.NONE;
		}

	// -------------------
	protected void EndActiveQuery(QueryOutcome outcome)
		{
		this.queryActive = false;
		this.queryOutcome = outcome;
Debug.Log("Bse End actv quer : " + outcome);
		this.OnEndActiveQuery();
		} 


	// -------------------
	virtual protected void OnEndActiveQuery()
		{
		this.ReturnToConfigScreen();
		}

	
	// ---------------------
	public float GetQueryPhaseProgress()
		{	
		if (!this.queryActive)
			return 0;

		switch (this.queryPhase)
			{	
			case QueryPhase.WAITING_FOR_CONFIRMATION :
				return (this.queryElapsed / this.waitForConfirmationTime);

			case QueryPhase.WAITING_FOR_INPUT :
				return (this.queryElapsed / this.waitForInputTime);

			case QueryPhase.WAITING_FOR_NEUTRAL :
				return (this.queryElapsed / this.waitForNeutralTime);
			}

		return 0;
		}

	virtual protected void OnChangeQueryPhase(QueryPhase phaseId) {}



	// ---------------------	
	protected void UpdateQuery()
		{

	
		if (this.queryActive)
			{	
			this.queryElapsed += CFUtils.realDeltaTimeClamped;


			switch (this.queryPhase)
				{
				// Waiting for neutral...
	
				case QueryPhase.WAITING_FOR_NEUTRAL : 
					if (this.config.GetCurGamepadState().IsNeutral())
						{
						this.ChangeQueryPhase(QueryPhase.WAITING_FOR_INPUT);
						}
					else if (this.queryElapsed > this.waitForNeutralTime)
						{
						// If gamepad hasn't returned to neutral state that means either gamepad is faulty or user wants to get out...

						this.EndActiveQuery(QueryOutcome.CANCEL_CONFIG);
						}
		
					break;
					
	
				// Waiting for input...
	
				case QueryPhase.WAITING_FOR_INPUT :
					if (!this.config.GetCurGamepadState().IsNeutral())
						{
						this.ChangeQueryPhase(QueryPhase.WAITING_FOR_CONFIRMATION);
						}
					else if (this.queryElapsed > this.waitForInputTime)
						{
						this.EndActiveQuery(QueryOutcome.SKIP);
						}
					break;
	
					
				// Waiting for confirmation...
	
				case QueryPhase.WAITING_FOR_CONFIRMATION :
					if (this.config.GetCurGamepadState().IsNeutral())
						{
						this.ChangeQueryPhase(QueryPhase.WAITING_FOR_INPUT);
						}
	
					else if (!this.config.IsCurGamepadStateUsable())
						this.ChangeQueryPhase(QueryPhase.WAITING_FOR_NEUTRAL);
	
					else if (!this.config.GetCurGamepadState().Equals(this.config.GetPrevGamepadState()))
						this.ChangeQueryPhase(QueryPhase.WAITING_FOR_CONFIRMATION);
	
					else if (this.queryElapsed > this.waitForConfirmationTime)
						{
						if (this.config.ApplyGamepadStateToElement(this.elemId, this.config.GetCurGamepadState()))
							{
							this.EndActiveQuery(QueryOutcome.SUCCESS);
							}
						else
							{
							//this.elemConfigList[(int)this.curElementId].Clear();
							this.ChangeQueryPhase(QueryPhase.WAITING_FOR_NEUTRAL);
							}
						}
					break;
	
				}
			}
		}






	// -----------------
	public QueryPhase GetQueryPhase()
		{ return this.queryPhase; }

	// -----------------
	protected void ChangeQueryPhase(QueryPhase phase)
		{
		this.queryPhase = phase;
		this.queryElapsed = 0;

		this.OnChangeQueryPhase(phase);
		}
		

	}
}
