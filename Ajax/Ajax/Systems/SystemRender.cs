using Ajax.Components;
using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Ajax.Objects;
using Ajax.OBJLoader;
using Ajax.Systems;
using Ajax;

namespace Ajax.Systems
{
    public class SystemRender : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_ROTATION | ComponentTypes.COMPONENT_SCALE | ComponentTypes.COMPONENT_GEOMETRY);

        private Matrix4 model;
        private Vector3 position, rotation, scale;
        private Camera camera;

        protected int pgmID;
        protected int skyID;
        protected int vsID;
        protected int skyvsID;
        protected int fsID;
        protected int skyfsID;
        protected int uniform_stex;
        protected int uniform_mmodelviewproj;
        protected int uniform_mmodel;
        protected int uniform_eyePos, uniform_skyTexture;

        public SystemRender(ref Camera camera)
        {
            pgmID = GL.CreateProgram();
            LoadShader("Shaders/vs.glsl", ShaderType.VertexShader, pgmID, out vsID);
            LoadShader("Shaders/fs.glsl", ShaderType.FragmentShader, pgmID, out fsID);
            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));

            skyID = GL.CreateProgram();
            LoadShader("Shaders/skybox.vert", ShaderType.VertexShader, skyID, out skyvsID);
            LoadShader("Shaders/skybox.frag", ShaderType.FragmentShader, skyID, out skyfsID);
            GL.LinkProgram(skyID);
            Console.WriteLine(GL.GetProgramInfoLog(skyID));

            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_mmodelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            uniform_eyePos = GL.GetUniformLocation(skyID, "eyePos");
            uniform_skyTexture = GL.GetUniformLocation(skyID, "sky");

            this.camera = camera;
        }

        void LoadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        public string Name
        {
            get { return "SystemRender"; }
        }

        public void OnAction(Entity entity)
        {
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent geometryComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_GEOMETRY;
                });
                Geometry geometry = ((ComponentGeometry)geometryComponent).Geometry();

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent rotationComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_ROTATION;
                });

                IComponent scaleComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_SCALE;
                });

                rotation = ((ComponentRotation)rotationComponent).Rotation;
                position = ((ComponentPosition)positionComponent).Position;
                scale = ((ComponentScale)scaleComponent).Scale;
                model = Matrix4.CreateTranslation(position);
                model *= Matrix4.CreateRotationX(rotation.X);
                model *= Matrix4.CreateRotationY(rotation.Y);
                model *= Matrix4.CreateRotationZ(rotation.Z);
                model *= Matrix4.CreateScale(scale);

                Draw(model, geometry);
            }
        }

        public void Draw(Matrix4 model, Geometry geometry)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.UniformMatrix4(uniform_mmodel, false, ref model);
            Matrix4 modelViewProjection = model * camera.view * camera.projection;
            GL.UniformMatrix4(uniform_mmodelviewproj, false, ref modelViewProjection);

            geometry.Render();

            GL.UseProgram(0);
        }

        public void Skybox()
        {
            GL.UseProgram(skyID);
            GL.Uniform3(uniform_eyePos, ref camera.cameraPosition);

            // coordinates provided from learnopengl.com
            float[] skyboxVertices = {
                -1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,

                -1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,

                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,

                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,

                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,

                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f
            };

            uint skyboxVAO, skyboxVBO;
            GL.GenVertexArrays(1, out skyboxVAO);
            GL.GenBuffers(1, out skyboxVBO);
            GL.BindVertexArray(skyboxVAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, skyboxVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, skyboxVertices.Length * sizeof(float), skyboxVertices, BufferUsageHint.StaticDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            string[] skyboxTextures = new string[]
            {
                "Textures/Skybox/emeraldfog_rt.tga",
                "Textures/Skybox/emeraldfog_lf.tga",
                "Textures/Skybox/emeraldfog_up.tga",
                "Textures/Skybox/emeraldfog_dn.tga",
                "Textures/Skybox/emeraldfog_ft.tga",
                "Textures/Skybox/emeraldfog_dn.tga"
            };

            uint skyboxTex = LoadCubemap(skyboxTextures);

            GL.BindVertexArray(skyboxVAO);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.TextureCubeMap, skyboxTex);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.DepthFunc(DepthFunction.Less);
            GL.UseProgram(0);
        }


        private uint LoadCubemap(string[] skyboxTexs)
        {
            return 0;
        }
    }
}
