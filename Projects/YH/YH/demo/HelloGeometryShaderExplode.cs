﻿﻿
using System;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;

namespace YH
{
	public class HelloGeometryShaderExplode : Application
	{
		public HelloGeometryShaderExplode() : base("HelloGeometryShaderExplode")
		{
		}

		public override void Start(Window wnd)
		{
			base.Start(wnd);

			mCube = new Cube();
			mFloor = new Floor();

			mCamera = new Camera(new Vector3(0.0f, 0.0f, 5.0f), new Vector3(0.0f, 1.0f, 0.0f), Camera.YAW, Camera.PITCH);
			mCameraController = new CameraController(mAppName, mCamera);
			mShader = new GLProgram(@"Resources/advanced.vs", @"Resources/advanced.frag");
			mCubeTexture = new GLTexture2D(@"Resources/Texture/wall.jpg");
			mFloorTexture = new GLTexture2D(@"Resources/Texture/metal.png");

			//
			mDepthFunction.Add(DepthFunction.Less);
			mDepthFunction.Add(DepthFunction.Always);
			mDepthFunction.Add(DepthFunction.Never);
		}

		public override void Update(double dt)
		{
			base.Update(dt);
		}

		public override void Draw(double dt, Window wnd)
		{
			GL.Viewport(0, 0, wnd.Width, wnd.Height);
			GL.ClearColor(Color.Gray);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if (mUseDepthTest)
			{
				GL.Enable(EnableCap.DepthTest);
			}
			else
			{
				GL.Disable(EnableCap.DepthTest);
			}

			GL.DepthFunc(mDepthFunction[mDepthFuncIndex]);

			var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(mCamera.Zoom), (float)wnd.Width / (float)wnd.Height, 0.1f, 100.0f);
			var view = mCamera.GetViewMatrix();

			mShader.Use();

			GL.BindTexture(TextureTarget.Texture2D, mCubeTexture.getTextureId());

			GL.UniformMatrix4(mShader.GetUniformLocation("projection"), false, ref projection);
			GL.UniformMatrix4(mShader.GetUniformLocation("view"), false, ref view);

			Matrix4 model = Matrix4.CreateTranslation(-1.0f, 0.0f, -1.0f);
			model = Matrix4.CreateScale(0.5f) * model;
			GL.UniformMatrix4(mShader.GetUniformLocation("model"), false, ref model);
			mCube.Draw();

			model = Matrix4.CreateTranslation(2.0f, 0.0f, 0.0f);
			model = Matrix4.CreateScale(0.5f) * model;
			GL.UniformMatrix4(mShader.GetUniformLocation("model"), false, ref model);
			mCube.Draw();

			GL.BindTexture(TextureTarget.Texture2D, mFloorTexture.getTextureId());
			model = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
			GL.UniformMatrix4(mShader.GetUniformLocation("model"), false, ref model);
			mFloor.Draw();
		}

		public override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (e.Key == OpenTK.Input.Key.Plus)
			{
				++mDepthFuncIndex;
				mDepthFuncIndex = mDepthFuncIndex >= mDepthFunction.Count ? 0 : mDepthFuncIndex;
			}
			else if (e.Key == OpenTK.Input.Key.Minus)
			{
				--mDepthFuncIndex;
				mDepthFuncIndex = mDepthFuncIndex < 0 ? 0 : mDepthFuncIndex;
			}
			else if (e.Key == OpenTK.Input.Key.C)
			{
				mUseDepthTest = !mUseDepthTest;
			}
			else if (e.Key == OpenTK.Input.Key.Space)
			{
				mUseDepthTest = true;
				mDepthFuncIndex = 0;
			}
		}

		private Cube mCube = null;
		private Floor mFloor = null;
		private Camera mCamera = null;
		private GLProgram mShader = null;
		private GLTexture2D mCubeTexture = null;
		private GLTexture2D mFloorTexture = null;
		private bool mUseDepthTest = true;
		private int mDepthFuncIndex = 0;
		private List<DepthFunction> mDepthFunction = new List<DepthFunction>();
	}

    /*
	public class HelloGeometryShaderExplode : Application
	{
		public HelloGeometryShaderExplode() : base("HelloGeometryShaderExplode")
		{

		}

		public override void Start(Window wnd)
		{
			base.Start(wnd);

			mCamera = new Camera(new Vector3(0.0f, 0.0f, 3.0f), new Vector3(0.0f, 1.0f, 0.0f), Camera.YAW, Camera.PITCH);
			mCameraController = new CameraController(mAppName, mCamera);

			mProgram = new GLProgram(@"Resources/explode.vs", @"Resources/explode.frag", @"Resources/explode.gs");
			//mProgram = new GLProgram(@"Resources/explode.vs", @"Resources/explode.frag", "");

			mCube = new Cube();
            mSphere = new Sphere();

            mDiffuseMap = new GLTexture2D(@"Resources/Texture/container2.png");

            GL.ClearColor(Color.Gray);
            GL.Enable(EnableCap.DepthTest);
		}

		public override void Update(double dt)
		{
			base.Update(dt);
		}

		public override void Draw(double dt, Window wnd)
		{
			GL.Viewport(0, 0, wnd.Width, wnd.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(mCamera.Zoom),
																 (float)wnd.Width / (float)wnd.Height,
																 0.1f, 100.0f);
			var view = mCamera.GetViewMatrix();

            mProgram.Use();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, mDiffuseMap.getTextureId());
            GL.Uniform1(mProgram.GetUniformLocation("texture_diffuse1"), 0);

			Matrix4 model = Matrix4.CreateTranslation(0, 0, 0);
            GL.UniformMatrix4(mProgram.GetUniformLocation("model"), false, ref model);
            GL.Uniform1(mProgram.GetUniformLocation("time"), (float)(mTotalRuningTime / 20.0f));
			mCube.Draw();
            mSphere.Draw();
		}

		public override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			base.OnKeyUp(e);
		}

		private GLProgram mProgram = null;
        private Cube mCube = null;
        private Sphere mSphere = null;
		private Camera mCamera = null;
        private GLTexture2D mDiffuseMap = null;
	}
    */
}
