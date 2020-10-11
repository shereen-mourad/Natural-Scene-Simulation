using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
using System.IO;
using Graphics._3D_Models;
using System.Drawing.Imaging;
using System.Drawing;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        float[,] hieght;
        List<float> ground;
        Bitmap image1;
        Color pix;
        List<float> map = new List<float>();

        List<vec3> trees = new List<vec3>();

       public  float[,] HEIGHTS;
        List<float> mkx = new List<float>();

        List<float> mkz = new List<float>();

        List<vec3> g = new List<vec3>();
        int posit;
        int selc;
        int transID;
        int viewID;
        int projID;
        uint pID, vertexBufferID7;
        int EyePositionID;
        int AmbientLightID;
        int DataID;

        mat4 ProjectionMatrix;
        mat4 ViewMatrix;
        int r1, r2, r3, r4, r5 ,maxid,r6;
        float maxi = 0;
        public float Speed = 1;
        Model3D tree, grass;
        uint vertexBufferID;
        Texture terr;
        float ff, ff1;
        Random rr=new Random();
        public Camera cam;

        public md2LOL m;

        Texture tex1;
        Texture d, u, l, r, f, b, t1,sand,rock,ice,gra,rsm,water;
        mat4 modelMatrix;
        mat4 down, front, back, right, left, up, te,gg;
        uint sqID;
        uint grID;
        mat4 modelmatrix;
        vec3 day;
        vec3 night;
        float c;
        int tID;
        bool reverse;
        float t = 0;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            tex1 = new Texture(projectPath + "\\Textures\\Ground.jpg", 2);

            m = new md2LOL(projectPath + "\\ModelFiles\\zombie.md2");
            m.StartAnimation(animType_LOL.ATTACK1);
            m.rotationMatrix = glm.rotate((float)((-90.0f / 180) * Math.PI), new vec3(1, 0, 0));
            m.scaleMatrix = glm.scale(new mat4(1), new vec3(0.8f, 0.8f, 0.8f));
            //skybox
            u = new Texture(projectPath + "\\Textures\\up.png", 2, false);
            d = new Texture(projectPath + "\\Textures\\downn.png", 3, false);
            f = new Texture(projectPath + "\\Textures\\front.png", 4, false);
            l = new Texture(projectPath + "\\Textures\\left.png", 5, false);
            r = new Texture(projectPath + "\\Textures\\right.png", 6, false);
            b = new Texture(projectPath + "\\Textures\\back.png", 7, false);

          //multi texture
            sand = new Texture(projectPath + "\\Textures\\sand.jpg", 15, false);

            gra = new Texture(projectPath + "\\Textures\\grass.jpg", 18, false);

            rock = new Texture(projectPath + "\\Textures\\rock.jpg", 17, false);

            ice = new Texture(projectPath + "\\Textures\\ice.jpg", 16, false);

            //rsm
            rsm = new Texture(projectPath + "\\Textures\\g7.png", 20, false);
            //water

            water = new Texture(projectPath + "\\Textures\\water.png", 22, false);

            //light
            day = new vec3(1,1,1);

            night=new vec3(0f,0f,0.4f);

            c += 0.02f;

            //---------------------------------------------------------------------------------------------------------------------------
            float[] square = 
            {
               -1,0,1, 1,0,0, 0,0,1,1,1,
                1,0,1, 1,0,0, 1,0,1,1,1,
               -1,0,-1, 1,0,0, 0,1,1,1,1,
            
             1,0,1,  1,0,0, 1,0,1,1,1,
            -1,0,-1, 1,0,0, 0,1,1,1,1,
            1,0,-1,  1,0,0, 1,1,1,1,1
            };

            sqID = GPU.GenerateBuffer(square);

            float[] sqgr = 
            {
               -1,1,0, 1,0,0, 0,0, 0,1,0,
                1,1,0, 1,0,0, 1,0, 0,1,0,
               -1,-1,0, 1,0,0, 0,1, 0,1,0,
            
             1,1,0,  1,0,0, 1,0, 0,1,0,
            -1,-1,0, 1,0,0, 0,1, 0,1,0,
            1,-1,0,  1,0,0, 1,1, 0,1,0
            };

            grID = GPU.GenerateBuffer(sqgr);


           //skybox
            modelMatrix = glm.scale(new mat4(1), new vec3(100, 100, 100));

            down = MathHelper.MultiplyMatrices(new List<mat4>(){
               // glm.rotate(90.0f/180.0f*3.1412f,new vec3(0,0,1)),
                glm.translate(new mat4(1),new vec3(0,0,0)),
                
                glm.scale(new mat4(1), new vec3(100 ,100,100))

                });
            left = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.rotate(90.0f/180.0f*3.1412f,new vec3(0,0,1)),
                glm.translate(new mat4(1),new vec3(-1,1,0)),
                glm.scale(new mat4(1), new vec3(100 ,100,100))
                });
            right = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.rotate(90.0f/180.0f*3.1412f,new vec3(0,0,1)),
                glm.translate(new mat4(1),new vec3(1,1,0)),
                
                glm.scale(new mat4(1), new vec3(100 ,100,100))
                });

            front = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.rotate(90.0f/180.0f*3.1412f,new vec3(1,0,0)),
                glm.translate(new mat4(1),new vec3(0,1,-1)),
                glm.scale(new mat4(1), new vec3(100,100,100))
                });
            back = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.rotate(90.0f/180.0f*3.1412f,new vec3(1,0,0)),
                glm.translate(new mat4(1),new vec3(0,1,1)),
                glm.scale(new mat4(1), new vec3(100 ,100,100))
                });
            up = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.rotate(-90.0f/180.0f*3.1412f,new vec3(0,1,0)),
                glm.translate(new mat4(1),new vec3(0,2,0)),
                glm.scale(new mat4(1), new vec3(100,100,100))
                });



         

            Gl.glClearColor(0, 0, 0, 0);
            cam = new Camera();
            cam.Reset(0, 3, 70, 0, 0, 0, 0, 1, 0);

            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();

            transID = Gl.glGetUniformLocation(sh.ID, "trans");
            projID = Gl.glGetUniformLocation(sh.ID, "projection");
            viewID = Gl.glGetUniformLocation(sh.ID, "view");
            sh.UseShader();

            //multitexture
            r1 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler");

            r2 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler2");

            r3 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler3");

            r4 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler4");

            r5 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler5");

            r6 = Gl.glGetUniformLocation(sh.ID, "myTextureSampler6");

            //terrain

            modelmatrix = glm.scale(new mat4(1), new vec3(50, 50, 50));
          
            image1 = new Bitmap(projectPath + "\\Textures\\heightmap.jpg");

            int i, j;
             HEIGHTS = new float[image1.Width, image1.Height];
            for (i = 0; i < image1.Width; i++)
            {
                for (j = 0; j < image1.Height; j++)
                {
                    pix = image1.GetPixel(i, j);
                   // HEIGHTS[i, j] = HEIGHTS[i, j] * 2;
                    HEIGHTS[i, j] = ((float)pix.B/8);
                }
            }


            for (i = 0; i < image1.Width-1; i++)
            {
                for (j = 0; j < image1.Height - 1; j++)
                {
                    vec3 v1=new vec3(i,HEIGHTS[i,j],j);

                    vec3 v2 = new vec3(i+1, HEIGHTS[i+1, j], j);

                    vec3 v3 = new vec3(i, HEIGHTS[i, j+1], j+1);

                    vec3 v4 = new vec3(i+1, HEIGHTS[i+1, j+1], j+1);

                    vec3 v5 = new vec3(i+1, HEIGHTS[i+1, j], j);

                    vec3 v6 = new vec3(i, HEIGHTS[i, j+1], j+1);
                    vec3 n = v2 - v1;
                    vec3 n2= v3 - v1;
                    vec3 light = glm.cross(n, n2);
                    vec3 n3 = v2 - v4;
                    vec3 n4 = v3 - v4;
                    vec3 light2 = glm.cross(n4, n3);
                    //1
                    map.Add(i);
                    if (maxi < HEIGHTS[i, j])
                    {
                        maxi = HEIGHTS[i, j];
                    }
                    map.Add(HEIGHTS[i, j]);
                    // if (maxv < HEIGHTS[x, y])
                    map.Add(j);
                    vec2 uv1 = new vec2(1, 0);
                    map.Add(1);
                    map.Add(0);
                    map.Add(light.x);

                    map.Add(light.y);
                    map.Add(light.z);

                    //2
                    map.Add(i + 1);
                    if (maxi < HEIGHTS[i+1, j])
                    {
                        maxi = HEIGHTS[i+1, j];
                    }
                    map.Add(HEIGHTS[i + 1, j]);
                    map.Add(j);

                    vec2 uv2 = new vec2(1, 1);
                    map.Add(1);
                    map.Add(1);
                    map.Add(light.x);

                    map.Add(light.y);
                    map.Add(light.z);
                    //3
                    map.Add(i);
                    if (maxi < HEIGHTS[i, j+1])
                    {
                        maxi = HEIGHTS[i, j+1];
                    }
                    map.Add(HEIGHTS[i, j + 1]);
                    map.Add(j + 1);

                    vec2 uv3 = new vec2(0, 0);
                    map.Add(0);
                    map.Add(0);
                    map.Add(light.x);

                    map.Add(light.y);
                    map.Add(light.z);

                    //4
                    map.Add(i + 1);
                    if (maxi < HEIGHTS[i+1, j+1])
                    {
                        maxi = HEIGHTS[i+1, j+1];
                    }
                    map.Add(HEIGHTS[i + 1, j + 1]);
                    map.Add(j + 1);

                    vec2 uv4 = new vec2(0, 1);
                    map.Add(0);
                    map.Add(1);
                    map.Add(light2.x);

                    map.Add(light2.y);
                    map.Add(light2.z);

                    //5
                    map.Add(i + 1);
                    if (maxi < HEIGHTS[i+1, j])
                    {
                        maxi = HEIGHTS[i+1, j];
                    }
                    map.Add(HEIGHTS[i + 1, j]);
                    map.Add(j);

                    vec2 uv5 = new vec2(1, 1);
                    map.Add(1);
                    map.Add(1);
                    map.Add(light2.x);

                    map.Add(light2.y);
                    map.Add(light2.z);
                    //6
                    map.Add(i);
                    if (maxi < HEIGHTS[i, j+1])
                    {
                        maxi = HEIGHTS[i, j+1];
                    }
                    map.Add(HEIGHTS[i, j + 1]);
                    map.Add(j + 1);

                    vec2 uv6 = new vec2(0, 0);
                    map.Add(0);
                    map.Add(0);
                    map.Add(light2.x);

                    map.Add(light2.y);
                    map.Add(light2.z);


                }
                }
            //ground
            hieght = new float[261, 396];
            ground = new List<float>();
            for ( i = 0; i < 261 - 1; i++)
            {
                for ( j = 0; j < 396 - 1; j++)
                {
                    hieght[i, j] = (float)rr.NextDouble() * 0;
                }
            }
            for ( i = 0; i < 261 - 1; i++)
            {
                for ( j = 0; j < 396 - 1; j++)
                {
                    vec3 v1 = new vec3(i, hieght[i, j], j);
                    vec3 v2 = new vec3(i + 1, hieght[i + 1, j], j);
                    vec3 v3 = new vec3(i + 1, hieght[i + 1, j + 1], j + 1);
                    vec3 v4 = new vec3(i, hieght[i, j + 1], j + 1);
                    vec3 v5 = new vec3(i + 1, hieght[i + 1, j + 1], j + 1);
                    vec3 v6 = new vec3(i, hieght[i, j], j);

                    vec3 n = v3 - v2;
                    vec3 n2 = v1 - v2;
                    vec3 light = glm.cross(n, n2);
                    light = glm.normalize(light);


                    vec3 n3 = v3 - v4;
                    vec3 n4 = v1 - v4;
                    vec3 light2 = glm.cross(n4, n3);
                    light2 = glm.normalize(light2);


                    //v1

                    ground.Add(i);
                    ground.Add(hieght[i, j]);
                    ground.Add(j);
                    ground.Add(0);
                    ground.Add(0);
                    ground.Add(light.x);
                    ground.Add(light.y);
                    ground.Add(light.z);


                    //v2
                    ground.Add(i + 1);
                    // bitmap.Add(pixel3.R);
                    ground.Add(hieght[i + 1, j]);
                    ground.Add(j);
                    ground.Add(1);
                    ground.Add(0);
                    ground.Add(light.x);
                    ground.Add(light.y);
                    ground.Add(light.z);



                    //v4
                    ground.Add(i + 1);
                    // bitmap.Add(pixel3.R);
                    ground.Add(hieght[i + 1, j + 1]);
                    ground.Add(j + 1);
                    ground.Add(1);
                    ground.Add(1);
                    ground.Add(light.x);
                    ground.Add(light.y);
                    ground.Add(light.z);




                    //v3
                    ground.Add(i);
                    ground.Add(hieght[i, j + 1]);
                    ground.Add(j + 1);
                    ground.Add(0);
                    ground.Add(1);
                    ground.Add(light2.x);
                    ground.Add(light2.y);
                    ground.Add(light2.z);




                    //v5
                    ground.Add(i + 1);
                    ground.Add((hieght[i + 1, j + 1]));
                    // bitmap.Add(pixel3.R);
                    ground.Add(j + 1);
                    ground.Add(1);
                    ground.Add(1);
                    ground.Add(light2.x);
                    ground.Add(light2.y);
                    ground.Add(light2.z);


                    //v6
                    ground.Add(i);
                    // bitmap.Add(pixel3.R);
                    ground.Add((hieght[i, j]));
                    ground.Add(j);
                    ground.Add(0);
                    ground.Add(0);
                    ground.Add(light2.x);
                    ground.Add(light2.y);
                    ground.Add(light2.z);
                }
            }
            vertexBufferID7 = GPU.GenerateBuffer(ground.ToArray());

            //graaaas
            mkx = new List<float>();
            mkz = new List<float>();
            for(int v=0;v<255;v++)
            {
                ff = (float)rr.NextDouble()*255;
                mkx.Add(ff);

                ff1 = (float)rr.NextDouble() * 255;
                mkz.Add(ff1);

            }


            maxid = Gl.glGetUniformLocation(sh.ID, "mval");
            posit = Gl.glGetUniformLocation(sh.ID, "pos1");

            selc = Gl.glGetUniformLocation(sh.ID, "sel");
            pID = GPU.GenerateBuffer(map.ToArray());

            //treee

            tree = new Model3D();
            tree.LoadFile(projectPath + "\\ModelFiles\\static\\Tree", 10,"Tree.obj");

            grass = new Model3D();
            grass.LoadFile(projectPath + "\\ModelFiles\\static\\grass",  11,"grass.obj");

            tree.scalematrix = glm.scale(new mat4(1), new vec3(3f, 3f, 3f));
            trees.Add(new vec3(30, 18, 30));

            trees.Add(new vec3(5, 19, 5));

           trees.Add(new vec3(-2, 37, -56));

            trees.Add(new vec3(-15, 16, -10));

            trees.Add(new vec3(60, 20, 60));

           trees.Add(new vec3(-50, 10, -40));

           trees.Add(new vec3(50, 20, 50));

            trees.Add(new vec3(-70, 5, -60));

            trees.Add(new vec3(20, 5, 80));

            trees.Add(new vec3(-20, 25, -80));

            trees.Add(new vec3(-40, 12, 45));
            
            trees.Add(new vec3(-20, 1, 70));

            trees.Add(new vec3(44, 10, -20));

            sh.UseShader();

            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            vec2 data = new vec2(100, 50);
            Gl.glUniform2fv(DataID, 1, data.to_array());

            //light

            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            vec3 lightPosition = new vec3(1.0f, 20f, 4.0f);
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());
            //setup the ambient light component.
            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "ambientLight");
            vec3 ambientLight = new vec3(0.2f, 0.18f, 0.01f);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
            //setup the eye position.
            EyePositionID = Gl.glGetUniformLocation(sh.ID, "EyePosition_worldspace");
          

            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LESS);


            tID = Gl.glGetUniformLocation(sh.ID, "t");

        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            sh.UseShader();

            Gl.glUniformMatrix4fv(projID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());
            Gl.glUniformMatrix4fv(viewID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            
            Gl.glUniform3fv(EyePositionID, 1, cam.GetCameraPosition().to_array());

            if (reverse)
                t -= 0.0002f;
            else
                t += 0.0002f;
            if (t >0.2f)
                reverse = true;
            if (t < 0)
                reverse = false;


           


            Gl.glUniform1i(selc, 1);
            //light
            vec3 current = day + (night - day) * c;
            c = c + 0.00002f;
            Gl.glUniform3fv(AmbientLightID,1,current.to_array());



            //skybox

            Gl.glUniform1f(tID, 0);
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, sqID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(6 * sizeof(float)));
            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(8 * sizeof(float)));

           // d.Bind();
            Gl.glUniform1i(posit,2);
            Gl.glUniform1i(r1, d.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0+d.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D,d.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, down.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

           // u.Bind();

            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, u.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + u.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, u.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, up.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
           // r.Bind();

            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, r.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + r.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, r.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, right.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

           // l.Bind();

            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, l.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + l.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, l.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, left.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);


            //f.Bind();

            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, f.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + f.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, f.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, front.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            //b.Bind();

            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, b.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + b.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, b.mtexture);
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, back.to_array());
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, new mat4(1).to_array());
           Gl.glUniform1f(maxid,maxi);

            //treeeee

           for (int i = 0; i < 13; i++)
           {
               tree.transmatrix = glm.translate(new mat4(1), trees[i]);
               tree.Draw(transID);
           }

        
            //terraain
            te = MathHelper.MultiplyMatrices(new List<mat4>(){
               
                glm.translate(new mat4(1),new vec3(-45,0,-250)),
                glm.scale(new mat4(1), new vec3(2f,2f,0.4f))
                });
            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, te.to_array());

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, pID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);

            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));


            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(5 * sizeof(float)));
           // terr.Bind();

            Gl.glUniform1i(posit, 1);
            Gl.glUniform1i(r2, sand.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + sand.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sand.mtexture);


            Gl.glUniform1i(posit, 1);
            Gl.glUniform1i(r3, gra.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + gra.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, gra.mtexture);


            Gl.glUniform1i(posit, 1);
            Gl.glUniform1i(r4, rock.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + rock.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rock.mtexture);

            Gl.glUniform1i(posit, 1);
            Gl.glUniform1i(r5, ice.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + ice.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, ice.mtexture);

            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, image1.Height * image1.Width);


            //graaaaassss
            // rsm.Bind();

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, grID);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(6 * sizeof(float)));
            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(8 * sizeof(float)));
            Gl.glUniform1i(posit, 2);
            Gl.glUniform1i(r1, rsm.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + rsm.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, rsm.mtexture);


            Gl.glEnable(Gl.GL_BLEND);

            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);


            for (int m = 0; m < 50; m++)
            {

                gg = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.scale(new mat4(1), new vec3(8f,15f,5f)),
                glm.translate(new mat4(1),new vec3((int)mkx[m],HEIGHTS[(int)mkx[m],(int)mkz[m]]*2f,(int)mkz[m]))

               
                });


                Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, gg.to_array());

                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            }
            for (int m = 0; m < 50; m++)
            {

                gg = MathHelper.MultiplyMatrices(new List<mat4>(){
                glm.scale(new mat4(1), new vec3(8f,15f,5f)),
                glm.translate(new mat4(1),new vec3((int)mkx[m]*-1,HEIGHTS[(int)mkx[m],(int)mkz[m]]*2f,(int)mkz[m]*-1))


               
                });


                Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, gg.to_array());

                Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, 6);
            }
            Gl.glDisable(Gl.GL_BLEND);

            //water
            mat4 groundd = MathHelper.MultiplyMatrices(new List<mat4>() {
                glm.translate(new mat4(1),new vec3(-100,10,-50)),
                glm.scale(new mat4(1), new vec3(1f,1.5f, 1.5f)) });

            Gl.glUniformMatrix4fv(transID, 1, Gl.GL_FALSE, groundd.to_array());
            Gl.glUniform1i(selc, 2);
            Gl.glUniform1i(posit, 3);
            Gl.glUniform1f(tID, t);
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID7);
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(5 * sizeof(float)));

            Gl.glUniform1i(posit, 3);
            Gl.glUniform1i(r6, water.TexUnit);
            Gl.glActiveTexture(Gl.GL_TEXTURE0 + water.TexUnit);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, water.mtexture);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 0, ground.Count);
            Gl.glDisable(Gl.GL_BLEND);



            Gl.glDisableVertexAttribArray(0);
            }
       
        public void Update(float deltaTime)
        {
            cam.UpdateViewMatrix();
            ProjectionMatrix = cam.GetProjectionMatrix();
            ViewMatrix = cam.GetViewMatrix();
            m.UpdateExportedAnimation();
        }
        public void SendLightData(float red, float green, float blue, float attenuation, float specularExponent)
        {
            vec3 ambientLight = new vec3(red, green, blue);
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());
            vec2 data = new vec2(attenuation, specularExponent);
            Gl.glUniform2fv(DataID, 1, data.to_array());
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
