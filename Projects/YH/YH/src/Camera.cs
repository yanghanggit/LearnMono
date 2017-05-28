﻿using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;

namespace YH
{
	public class Camera
	{
		public enum MOVEMENT
		{
			FORWARD,
			BACKWARD,
			LEFT,
			RIGH		
		};
		
		public Camera()
		{
			mPosition = new Vector3(0.0f, 0.0f, 0.0f);
			mWorldUp = new Vector3(0.0f, 1.0f, 0.0f);
			mYaw = YAW;
			mPitch = PITCH;
			mFront = new Vector3(0.0f, 0.0f, -1.0f);
			mMovementSpeed = SPEED;
			mMouseSensitivity = SENSITIVTY;
			mZoom = ZOOM;

            updateCamera();
		}

		private void updateCamera()
		{
			/*
			// Calculate the new Front vector
			glm::vec3 front;
			front.x = cos(glm::radians(this->Yaw)) * cos(glm::radians(this->Pitch));
			front.y = sin(glm::radians(this->Pitch));
			front.z = sin(glm::radians(this->Yaw)) * cos(glm::radians(this->Pitch));

			this->Front = glm::normalize(front);
			// Also re-calculate the Right and Up vector
			this->Right = glm::normalize(glm::cross(this->Front, this->WorldUp));  // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
			this->Up    = glm::normalize(glm::cross(this->Right, this->Front));
			*/

			//
			float x = (float)(Math.Cos(MathHelper.DegreesToRadians(mYaw)) + Math.Cos(MathHelper.DegreesToRadians(mPitch)));
			float y = (float)(Math.Sin(MathHelper.DegreesToRadians(mPitch)));
			float z = (float)(Math.Sin(MathHelper.DegreesToRadians(mYaw)) + Math.Cos(MathHelper.DegreesToRadians(mPitch)));
			Vector3 front = new Vector3(x, y, z);
			mFront = Vector3.Normalize(front);

			//
			mRight = Vector3.Normalize(Vector3.Cross(mFront, mWorldUp));
			mUp = Vector3.Normalize(Vector3.Cross(mRight, mFront));
		}

		Matrix4 GetViewMatrix() 
		{
			return Matrix4.LookAt(mPosition, mPosition + mFront, mUp);
		}

		static public readonly float YAW = -90.0f;
		static public readonly float PITCH = 0.0f;
		static public readonly float SPEED = 3.0f;
		static public readonly float SENSITIVTY = 0.25f;
		static public readonly float ZOOM = 45.0f;

		private Vector3 mPosition;
		private Vector3 mFront;
		private Vector3 mUp;
		private Vector3 mRight;
		private readonly Vector3 mWorldUp = new Vector3(0.0f, 1.0f, 0.0f);
		private float mYaw;
		private float mPitch;
		private float mMovementSpeed;
		private float mMouseSensitivity;
		private float mZoom;
	}
}