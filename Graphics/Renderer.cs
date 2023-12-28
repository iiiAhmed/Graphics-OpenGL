using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint vertexBufferID;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 4f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 objectCenter;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0, 0, 0.4f, 1);
            
            float[] verts= { 
		        // 1face
                0.076f, 0.280f, 0f,
                1,0,0,
                0.227f, 0.316f, 0f,
                0,0,1,
                0.422f, 0.156f, 0f,
                1,0,0,
                0.307f, -0.049f, 0f,
                1,0,1,
                0.147f, -0.076f, 0f,
                0,0,1,
                -0.067f, 0.076f, 0f,
                0,0,1,
                0.049f, 0.244f, 0f,
                1,1,1,

                // 2right ear
                0.422f, 0.156f, 0f,
                1,0,0,
                0.227f, 0.316f, 0f,
                0,0,1,
                0.324f, 0.413f, 0f,
                1,0,1,

                //3left ear
                -0.067f, 0.076f, 0f,
                1,0,0,
                0.049f, 0.244f, 0f,
                0,0,1,
                -0.156f, 0.324f, 0f,
                1,0,1,

                //4
                0.147f, -0.076f, 0f,
                1,0,0,
                0.307f, -0.049f, 0f,
                1,0,1,
                0.209f, -0.280f, 0f,
                0,0,1,
                0.156f, -0.271f, 0f,
                1,0,1,
                
                //5
                0.147f, -0.076f, 0f,
                1,0,1,
                0.156f, -0.404f, 0f,
                1,0,0,
                -0.067f, 0.076f, 0f,
                0,0,1,

                // 6 body
                -0.084f, 0.102f, 0f,
                1,0,0,
                0.156f, -0.404f, 0f,
                1,0,1,
                -0.351f, -0.324f, 0f,
                0,0,1,
                -0.236f, 0.031f, 0f,
                1,0,1,

                //7
                -0.351f, -0.324f, 0f,
                1,0,0,
                -0.236f, 0.031f, 0f,
                1,0,1,
                -0.431f, 0.396f, 0f,
                0,0,1,

                // 8 right line
                0.209f, -0.280f, 0f,
                0.182f, -0.102f, 0f,

                // 9 left line
                0.156f, -0.404f, 0f,
                0,1,0,
               -0.200f, -0.022f, 0f,
                0,1,0,

                // 10. left eye                
                0.102f, 0.084f, 0f,
                0,0,0,
                0.111f, 0.084f, 0f,
                0,0,0,
                0.111f, 0.093f, 0f,
                0,0,0,
                0.111f, 0.102f, 0f,
                0,0,0,
                0.120f, 0.102f, 0f,
                0,0,0,
                0.129f, 0.102f, 0f,
                0,0,0,
                0.129f, 0.111f, 0f,
                0,0,0,
                0.138f, 0.111f, 0f,
                0,0,0,
                0.138f, 0.102f, 0f,
                0,0,0,
                0.147f, 0.102f, 0f,
                0,0,0,
                0.156f, 0.102f, 0f,
                0,0,0,

                // 11. right eye
                0.236f, 0.120f, 0f,
                0,0,0,
                0.244f, 0.120f, 0f,
                0,0,0,
                0.244f, 0.129f, 0f,
                0,0,0,
                0.253f, 0.129f, 0f,
                0,0,0,
                0.262f, 0.129f, 0f,
                0,0,0,
                0.271f, 0.129f, 0f,
                0,0,0,
                0.280f, 0.120f, 0f,
                0,0,0,
                0.280f, 0.111f, 0f,
                0,0,0,


                // 12. nose
                0.191f, 0.049f, 0f,
                0,0,0,
                0.209f, 0.049f, 0f,
                0,0,0,
                0.218f, 0.067f, 0f,
                0,0,0,
                0.209f, 0.076f, 0f,
                0,0,0,
                0.191f, 0.076f, 0f,
                0,0,0,
                0.182f, 0.067f, 0f,
                0,0,0,
                0.182f, 0.058f, 0f,
                0,0,0,

                // mouth
                0.173f, 0.013f, 0f,
                0,0,0,
                0.182f, 0.004f, 0f,
                0,0,0,
                0.191f, -0.004f, 0f,
                0,0,0,
                0.218f, -0.004f, 0f,
                0,0,0,
                0.227f, 0.004f, 0f,
                0,0,0,
                0.236f, 0.013f, 0f,
                0,0,0
            }; 
            
            objectCenter = new vec3(0, 0f, 0);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, -100.0f,
                0.0f, 0.0f, 1.0f,  
            };

            vertexBufferID = GPU.GenerateBuffer(verts);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(0, 0, 3f), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 10)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated_Cat
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            // 1. face
            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 7);

            // 2.right ear
            Gl.glDrawArrays(Gl.GL_POLYGON, 7, 3);

            // 3. left ear
            Gl.glDrawArrays(Gl.GL_POLYGON, 10, 3);

            // 4.
            Gl.glDrawArrays(Gl.GL_POLYGON, 13, 4);

            // 5.
            Gl.glDrawArrays(Gl.GL_POLYGON, 17, 3);

            // 6.
            Gl.glDrawArrays(Gl.GL_POLYGON, 20, 4);

            // 7.
            Gl.glDrawArrays(Gl.GL_POLYGON, 24, 3);

            // 8. right line
            Gl.glDrawArrays(Gl.GL_LINE, 27, 2);

            // 9. left line
            Gl.glDrawArrays(Gl.GL_LINE, 29, 2);

            // 10 left eye
            Gl.glDrawArrays(Gl.GL_POLYGON, 31, 10);

            // 11 right eye
            Gl.glDrawArrays(Gl.GL_POLYGON, 41, 8);

            // 12 nose
            Gl.glDrawArrays(Gl.GL_POLYGON, 49, 7);

            // 13 mouth
            Gl.glDrawArrays(Gl.GL_POLYGON, 56, 6);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }


        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            float translationSpeed = 1f;
            
            
            
            if (translationX < 5f)
            {
                translationX += deltaTime * translationSpeed;
            }



            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * objectCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 0, 1)));
            transformations.Add(glm.translate(new mat4(1),  objectCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));
            
            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }


        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
