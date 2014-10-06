﻿using UnityEngine;
using System.Collections;

public class Thrust : MonoBehaviour {

	public float maxThrust = 200f;
	public float throttleSensitivity = 100f;

	// Boost
	public AnimationCurve boostCurve;

	// Public maxima
	public float boostLength = 1.5f;
	public float boostCooldownLength = 0.5f;
	public float boostThrust = 2.5f;

	// Private instantaneous values
	private bool boosting = false;
	private float boostTime = 0f;
	private float boostCooldown = 0f;
	private float boostPower = 0f;

	private float thrustPower = 1f;
	private float xThrustPower = 100f;
	private float yThrustPower = 100f;
	private float zThrustPower = 100f;

	private float xThrust = 0f;
	private float yThrust = 0f;
	private float zThrust = 0f;

	private float disruptTime = 0f;

	private Vector3 velocity;

	void Start(){
		thrustPower = maxThrust;
		boostCurve = new AnimationCurve(new Keyframe(0f, 0f),
										new Keyframe(0.1f, 1f), 
										new Keyframe(0.8f, 1f), 
										new Keyframe(1.0f, 0f));
	}

	void adjustThrust(float adjustment){
		thrustPower += adjustment * throttleSensitivity;
		if(thrustPower > maxThrust){
			thrustPower = maxThrust;
		}
		if(thrustPower < 0){
			thrustPower = 0;
		}
	}

	void XThrust(float thrust){
		xThrust = thrust;
	}

	void YThrust(float thrust){
		yThrust = thrust;
	}

	void ZThrust(float thrust){
		zThrust = thrust;
	}

	void Boost(){
		if(!boosting){
			if(boostCooldown <= 0){
				Debug.Log("Boost begin");
				boosting = true;
				boostTime = 0f;
			}else{
				Debug.Log("Boost cooldown not finished.");
			}
		}else{
			Debug.Log("Still boosting.");
		}
	}

	private void manageBoost(){
		if(boosting){
			boostPower = boostCurve.Evaluate(boostTime/boostLength);
			boostTime += Time.deltaTime;
			if(boostTime >= boostLength){
				boosting = false;
				boostTime = 0f;
				boostCooldown = boostCooldownLength;
				Debug.Log("Boost end");
			}
		}else if(boostCooldown > 0f){
			boostCooldown -= Time.deltaTime;
		}
	}

	void FixedUpdate(){
		adjustThrust(Input.GetAxis("Mouse ScrollWheel"));
		manageBoost();
		if(disruptTime <= 0f){
			rigidbody.AddRelativeForce(Vector3.right * (xThrust * xThrustPower * thrustPower * (1f + boostPower * boostThrust)) * Time.deltaTime);
			rigidbody.AddRelativeForce(Vector3.up * (yThrust * yThrustPower * thrustPower * (1f + boostPower * boostThrust)) * Time.deltaTime);
			rigidbody.AddRelativeForce(Vector3.forward * (zThrust * zThrustPower * thrustPower * (1f + boostPower * boostThrust)) * Time.deltaTime);
		}else{
			disruptTime -= Time.deltaTime;
			if(disruptTime < 0f){
				disruptTime = 0f;
			}
		}
	}

	public void disrupt(float time){
		disruptTime += time;
	}
}
