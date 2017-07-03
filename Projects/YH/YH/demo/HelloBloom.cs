﻿using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;


namespace YH
{
	public class HelloBloom : Application
	{
		public HelloBloom() : base("HelloBloom")
		{

		}

		public override void Start(Window wnd)
		{
			base.Start(wnd);

            //
			GL.Viewport(0, 0, wnd.Width, wnd.Height);
			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
			GL.Enable(EnableCap.DepthTest);

            //
			mCube = new Cube();
			mSphere = new Sphere();

            //
			mCamera = new Camera(new Vector3(0.0f, 0.0f, 5.0f), new Vector3(0.0f, 1.0f, 0.0f), 90.0f, Camera.PITCH);
			mCameraController = new CameraController(mAppName, mCamera);

            //
            shader = new GLProgram(@"Resources/bloom.vs", @"Resources/bloom.frag");
            shaderLight = new GLProgram(@"Resources/bloom.vs", @"Resources/light_box.frag");
            shaderBlur = new GLProgram(@"Resources/blur.vs", @"Resources/blur.frag");
            shaderBloomFinal = new GLProgram(@"Resources/bloom_final.vs", @"Resources/bloom_final.frag");

			// Set samplers
			shaderBloomFinal.Use();
            GL.Uniform1(shaderBloomFinal.GetUniformLocation("scene"), 0);
            GL.Uniform1(shaderBloomFinal.GetUniformLocation("bloomBlur"), 1);

			//
			// Light sources
			// - Positions
			//std::vector<glm::vec3> lightPositions;
			mLightPositions.Add(new Vector3(0.0f, 0.5f, 1.5f)); // back light
			mLightPositions.Add(new Vector3(-4.0f, 0.5f, -3.0f));
			mLightPositions.Add(new Vector3(3.0f, 0.5f, 1.0f));
			mLightPositions.Add(new Vector3(-.8f, 2.4f, -1.0f));
			// - Colors
			//std::vector<glm::vec3> lightColors;
			mLightColors.Add(new Vector3(5.0f, 5.0f, 5.0f));
			mLightColors.Add(new Vector3(5.5f, 0.0f, 0.0f));
			mLightColors.Add(new Vector3(0.0f, 0.0f, 15.0f));
			mLightColors.Add(new Vector3(0.0f, 1.5f, 0.0f));

			// Load textures
			woodTexture = new GLTexture2D(@"Resources/Texture/wood.png"); //loadTexture(FileSystem::getPath("resources/textures/wood.png").c_str());
			containerTexture = new GLTexture2D(@"Resources/Texture/container2.png"); //loadTexture(FileSystem::getPath("resources/textures/container2.png").c_str());


			/*
			mCube = new Cube();
			mQuad = new Quad();
			mSphere = new Sphere();

			mCamera = new Camera(new Vector3(0.0f, 0.0f, 5.0f), new Vector3(0.0f, 1.0f, 0.0f), 90.0f, Camera.PITCH);
			mCamera.MovementSpeed *= 2.0f;

			mCameraController = new CameraController(mAppName, mCamera);

			//
			mHDRFBO = new GLHDRFramebuffer(wnd.Width, wnd.Height);

			//
			mLightingShader = new GLProgram(@"Resources/lighting.vs", @"Resources/lighting.frag");
			mHDRShader = new GLProgram(@"Resources/hdr.vs", @"Resources/hdr.frag");

			//
			mWoodTexture = new GLTexture2D(@"Resources/Texture/wood.png");

			//
			mLightPositions.Add(new Vector3(0.0f, 0.0f, 49.5f));
			mLightPositions.Add(new Vector3(-1.4f, -1.9f, 9.0f));
			mLightPositions.Add(new Vector3(0.0f, -1.8f, 4.0f));
			mLightPositions.Add(new Vector3(0.8f, -1.7f, 6.0f));

			//
			mLightColors.Add(new Vector3(200.0f, 200.0f, 200.0f));
			mLightColors.Add(new Vector3(0.1f, 0.0f, 0.0f));
			mLightColors.Add(new Vector3(0.0f, 0.0f, 0.2f));
			mLightColors.Add(new Vector3(0.0f, 0.1f, 0.0f));

			//
			mLampShader = new GLProgram(@"Resources/lamp.vs", @"Resources/lamp.frag");

			//
			GL.Viewport(0, 0, wnd.Width, wnd.Height);
			GL.ClearColor(0.1f, 0.1f, 0.1f, 0.1f);
			GL.Enable(EnableCap.DepthTest);
			*/
		}

		public override void Update(double dt)
		{
			base.Update(dt);
		}

		public override void Draw(double dt, Window wnd)
		{
            /*
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, mHDRFBO.mHDRFBO);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(mCamera.Zoom),
																  (float)wnd.Width / (float)wnd.Height,
																  0.1f, 100.0f);
			var view = mCamera.GetViewMatrix();

			var model = Matrix4.CreateTranslation(0, 0, 0);

			mLightingShader.Use();
			GL.UniformMatrix4(mLightingShader.GetUniformLocation("projection"), false, ref projection);
			GL.UniformMatrix4(mLightingShader.GetUniformLocation("view"), false, ref view);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, mWoodTexture.getTextureId());

			for (var i = 0; i < mLightPositions.Count; ++i)
			{
				GL.Uniform3(mLightingShader.GetUniformLocation("lights[" + i + "].Position"), mLightPositions[i]);
				GL.Uniform3(mLightingShader.GetUniformLocation("lights[" + i + "].Color"), mLightColors[i]);
			}
			GL.Uniform3(mLightingShader.GetUniformLocation("viewPos"), mCamera.Position);
			model = Matrix4.CreateTranslation(0.0f, 0.0f, 25.0f);
			model = Matrix4.CreateScale(5.0f, 5.0f, 55.0f) * model;
			GL.UniformMatrix4(mLightingShader.GetUniformLocation("model"), false, ref model);
			GL.Uniform1(mLightingShader.GetUniformLocation("inverse_normals"), 1);
			mCube.Draw();


			//
			mLampShader.Use();
			GL.UniformMatrix4(mLampShader.GetUniformLocation("projection"), false, ref projection);
			GL.UniformMatrix4(mLampShader.GetUniformLocation("view"), false, ref view);

			for (var i = 0; i < mLightPositions.Count; ++i)
			{
				var pos = mLightPositions[i];
				model = Matrix4.CreateTranslation(pos);
				model = Matrix4.CreateScale(0.1f) * model;
				GL.UniformMatrix4(mLampShader.GetUniformLocation("model"), false, ref model);

				var color = mLightColors[i];
				GL.Uniform3(mLampShader.GetUniformLocation("set_color"), color);

				mSphere.Draw();
			}

			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			//
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			mHDRShader.Use();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, mHDRFBO.mColorBuffer);
			GL.Uniform1(mHDRShader.GetUniformLocation("hdr"), mUseHDR ? 1 : 0);
			GL.Uniform1(mHDRShader.GetUniformLocation("exposure"), mExposure);
			mQuad.Draw();
			*/
		}

		public override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);

			if (e.Key == OpenTK.Input.Key.H)
			{
				//mUseHDR = !mUseHDR;
			}
		}

		public override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Key == OpenTK.Input.Key.Plus)
			{
				//mExposure += (float)(2.0 * mDeltaTime);
			}
			else if (e.Key == OpenTK.Input.Key.Minus)
			{
				//mExposure -= (float)(2.0 * mDeltaTime);
				//mExposure = (mExposure >= 0.0f ? mExposure : 0.0f);
			}
		}

		private Camera mCamera = null;
		private Cube mCube = null;
		private Sphere mSphere = null;
		private GLProgram shader = null;//("bloom.vs", "bloom.frag");
		private GLProgram shaderLight = null;//("bloom.vs", "light_box.frag");
		private GLProgram shaderBlur = null;//("blur.vs", "blur.frag");
		private GLProgram shaderBloomFinal = null;//("bloom_final.vs", "bloom_final.frag");
        private GLTexture2D woodTexture = null;
        private GLTexture2D containerTexture = null;
		



		//

		//private Camera mCamera = null;
		//private GLHDRFramebuffer mHDRFBO = null;
		//private GLProgram mLightingShader = null;
		//private GLProgram mHDRShader = null;
		//private GLTexture2D mWoodTexture = null;
		private List<Vector3> mLightPositions = new List<Vector3>();
		private List<Vector3> mLightColors = new List<Vector3>();
		//private float mExposure = 1.0f;
		//private bool mUseHDR = true;
		//private GLProgram mLampShader = null;
	}
}
