﻿using System;
using OpenTK;
using OpenTK.Graphics;

namespace YH
{
	//=============================================================================================
	public class Window : OpenTK.GameWindow
	{
		public Window(int w, int h) : base
		(
		w, // initial width
		h, // initial height
		new GraphicsMode(32, 24, 8, 0),//GraphicsMode.Default, //public GraphicsMode (ColorFormat color, int depth, int stencil, int samples);
		"OpenTK.GameWindow",  // initial title
		GameWindowFlags.Default,
		DisplayDevice.Default,
		4, // OpenGL major version
		0, // OpenGL minor version
		GraphicsContextFlags.ForwardCompatible)
		{
			//public GraphicsMode (ColorFormat color, int depth, int stencil, int samples);
			//mCurrentApplication = new HelloTriangle();
			//mCurrentApplication = new HelloTexture2D();
			//mCurrentApplication = new HelloTransform();
			//mCurrentApplication = new HelloCoordinateSystem();
			//mCurrentApplication = new HelloCamera();
			//mCurrentApplication = new HelloColors();
			//mCurrentApplication = new HelloBasicLighting();
			//mCurrentApplication = new HelloMaterials();
			//mCurrentApplication = new HelloLightingMaps();
			//mCurrentApplication = new HelloLightCasters();
			//mCurrentApplication = new HelloMultipleLights();
			//mCurrentApplication = new HelloDepthTesting1();
			//mCurrentApplication = new HelloDepthTesting2();
			//mCurrentApplication = new HelloStencilTesting();
			mCurrentApplication = new HelloBlending1();

			Title = mCurrentApplication.mAppName;
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			if (!mCurrentApplication.isStarted())
			{
				mCurrentApplication.Start(this);
			}

			mCurrentApplication.Update(e.Time);
			mCurrentApplication.Draw(e.Time, this);

            SwapBuffers();
		}

		protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (mCurrentApplication != null)
			{
				mCurrentApplication.OnKeyDown(e);
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);

			if (mCurrentApplication != null)
			{
				mCurrentApplication.OnKeyPress(e);
			}
		}

		protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (mCurrentApplication != null)
			{
				mCurrentApplication.OnKeyUp(e);
			}
		}

		protected override void OnMouseMove(OpenTK.Input.MouseMoveEventArgs e)
		{
			base.OnMouseMove(e);

			if (mCurrentApplication != null)
			{
				mCurrentApplication.OnMouseMove(e);
			}
		}

		private Application mCurrentApplication;
	}

	 
}


	 

