﻿/**
 * Copyright (C) 2005-2014 by Rivello Multimedia Consulting (RMC).                    
 * code [at] RivelloMultimediaConsulting [dot] com                                                  
 *                                                                      
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the      
 * "Software"), to deal in the Software without restriction, including  
 * without limitation the rights to use, copy, modify, merge, publish,  
 * distribute, sublicense, and#or sell copies of the Software, and to   
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:                                            
 *                                                                      
 * The above copyright notice and this permission notice shall be       
 * included in all copies or substantial portions of the Software.      
 *                                                                      
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,      
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF   
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR    
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.                                      
 */
// Marks the right margin of code *******************************************************************


//--------------------------------------
//  Imports
//--------------------------------------
using UnityEngine;
using com.rmc.exceptions;
using System.Collections;
using com.rmc.projects.animation_monitor;



//--------------------------------------
//  Namespace
//--------------------------------------
using com.rmc.projects.paddle_soccer.mvcs.model.data;
using com.rmc.utilities;
using com.rmc.projects.paddle_soccer.mvcs.view.ui;
using com.rmc.projects.paddle_soccer.mvcs;


namespace com.rmc.projects.paddle_soccer.components
{
	
	//--------------------------------------
	//  Namespace Properties
	//--------------------------------------
	
	//--------------------------------------
	//  Class Attributes
	//--------------------------------------
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	public class PaddleComponent : MonoBehaviour 
	{
		
		//--------------------------------------
		//  Properties
		//--------------------------------------
		
		// GETTER / SETTER

		
		/// <summary>
		/// Gets or sets a value indicating whether this
		/// <see cref="com.rmc.projects.paddle_soccer.components.PaddleComponent"/> is spinning.
		/// </summary>
		/// <value><c>true</c> if is spinning; otherwise, <c>false</c>.</value>
		public bool isSpinning
		{ 
			get{
				return _animator.GetBool ("isSpinning_boolean");
			}
			set
			{
				_animator.SetBool ("isSpinning_boolean", value);
			}
		}



		/// <summary>
		/// The _target y_float.
		/// </summary>
		public float targetY
		{ 
			get{
				return _yPosition_lerptarget.targetValue;
			}
			set
			{
				_yPosition_lerptarget.targetValue = value;
				
			}
		}


		
		// PUBLIC
		
		/// <summary>
		/// The velocity.
		/// </summary>
		public Vector2 velocity;

		
		// PUBLIC STATIC
		
		// PRIVATE
		/// <summary>
		/// When the turret spinning_lerptarget.
		/// </summary>
		private LerpTarget _yPosition_lerptarget;

		
		/// <summary>
		/// When the ANIMATION NAMES
		/// </summary>
		public const string ANIMATION_NAME_PADDLE_RED_ANIMATION 		= "PaddleRedAnimation";	
		public const string ANIMATION_NAME_PADDLE_BLUE_ANIMATION 		= "PaddleBlueAnimation";	
		
		/// <summary>
		/// When the _move speed_float.
		/// </summary>
		private float _moveSpeed_float;



		/// <summary>
		/// The _last position_vector3.
		/// </summary>
		private Vector3 _lastPosition_vector3;

		/// <summary>
		/// The _starting x_ position.
		/// </summary>
		private float _startingXPosition_float;

		/// <summary>
		/// The _animator.
		/// </summary>
		private Animator _animator;
		
		
		/// <summary>
		
		// PRIVATE STATIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------	
		
		///<summary>
		///	Use this for initialization
		///</summary>
		public void Start () 
		{
			
			_yPosition_lerptarget 			= new LerpTarget (0, 0, -10, 10, 0.5f);
			_animator = GetComponent <Animator>();
			_startingXPosition_float = gameObject.transform.position.x;

			
		}
		
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		public void OnDestroy ()
		{


		}


		///<summary>
		///	Called once per frame
		///</summary>
		void Update () 
		{

			//ROTATE THE BARREL IF FIRING
			if (true) {
				_yPosition_lerptarget.lerpCurrentToTarget (Time.deltaTime);
				_doMoveToTarget();
			} 



		}
		

		
		// PUBLIC

		
		/// <summary>
		/// Dos the tween to starting position.
		/// </summary>
		/// <param name="aOffsetX_float">A offset x_float.</param>
		public void doTweenToStartingPosition (float aOffsetX_float)
		{
			//
			Hashtable moveTo_hashtable 					= new Hashtable();
			moveTo_hashtable.Add(iT.MoveTo.x,			_startingXPosition_float);
			moveTo_hashtable.Add(iT.MoveTo.delay,  		0);
			moveTo_hashtable.Add(iT.MoveTo.time,  		0.25f);
			moveTo_hashtable.Add(iT.MoveTo.easetype, 	iTween.EaseType.linear);
			iTween.MoveTo (gameObject, 					moveTo_hashtable);
		}

		/// <summary>
		/// Dos the tween to starting position.
		/// </summary>
		/// <param name="aOffsetX_float">A offset x_float.</param>
		public void doTweenToOffscreenPosition (float aOffsetX_float)
		{

			//
			gameObject.transform.position 				= new Vector3 (_startingXPosition_float, gameObject.transform.position.y, gameObject.transform.position.z);
			Hashtable moveTo_hashtable 					= new Hashtable();
			moveTo_hashtable.Add(iT.MoveTo.x,			_startingXPosition_float + aOffsetX_float);
			moveTo_hashtable.Add(iT.MoveTo.delay,  		0);
			moveTo_hashtable.Add(iT.MoveTo.time,  		0.25f);
			moveTo_hashtable.Add(iT.MoveTo.easetype, 	iTween.EaseType.linear);
			iTween.MoveTo (gameObject, 					moveTo_hashtable);
		}

		/// <summary>
		/// Dos the spin once.
		/// </summary>
		public void doSpinOnce () 
		{
			
			StartCoroutine (doSpinOnce_Coroutine());
			
		}


		/// <summary>
		/// Dos the spin once.
		/// 
		/// NOTE:  Animator hinges on the parameter isSpinning_boolean
		/// 		So we toggle that true, then upon animation completion, false
		/// 
		/// </summary>
		/// <returns>The spin once.</returns>
		private IEnumerator doSpinOnce_Coroutine () 
		{
			
			isSpinning = true;
			
			yield return new WaitForSeconds (_animator.GetCurrentAnimatorStateInfo(0).length);
			
			isSpinning = false;
			
		}
		
		// PUBLIC STATIC
		
		// PRIVATE
		/// <summary>
		/// Do move to target.
		/// </summary>
		private void _doMoveToTarget ()
		{
			//
			transform.position = new Vector3 (
				transform.position.x, 
				_getNewYPositionOnscreen(), 
				transform.position.z);

			//STORE FOR VELOCITY CHECK
			velocity = transform.position - _lastPosition_vector3;
			_lastPosition_vector3 = transform.position;
		}


		/// <summary>
		/// _gets the new Y position onscreen.
		/// 
		/// NOTE: We also 'reset' the lerp target 
		/// 
		/// </summary>
		private float _getNewYPositionOnscreen ()
		{
			float newYPosition_float = _yPosition_lerptarget.current;
			if (newYPosition_float < Constants.PADDLE_Y_MINIMUM) {
				newYPosition_float = Constants.PADDLE_Y_MINIMUM;
				_yPosition_lerptarget.targetValue = _yPosition_lerptarget.current;
			} else if (newYPosition_float > Constants.PADDLE_Y_MAXIMUM) {
				newYPosition_float = Constants.PADDLE_Y_MAXIMUM;
				_yPosition_lerptarget.targetValue = _yPosition_lerptarget.current;
			}
			

			return newYPosition_float;
		}
		
		// PRIVATE STATIC
		
		// PRIVATE COROUTINE
		
		// PRIVATE INVOKE
		
		//--------------------------------------
		//  Events 
		//		(This is a loose term for -- handling incoming messaging)
		//
		//--------------------------------------

		
	}
}

