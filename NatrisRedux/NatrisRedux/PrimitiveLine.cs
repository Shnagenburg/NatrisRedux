﻿using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NatrisRedux
{
    /// <summary>
    /// A class to make primitive 2D objects out of lines. Courtesy of MessiahAndrew from http://forums.create.msdn.com/forums/t/7414.aspx
    /// </summary>
    public class PrimitiveLine
    {
        Texture2D pixel;
        ArrayList vectors;

        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        /// <summary>
        /// Gets/sets the colour of the primitive line object.
        /// </summary>
        public Color Colour;


        /// <summary>
        /// Gets/sets the position of the primitive line object.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Gets/sets the render depth of the primitive line object (0 = front, 1 = back)
        /// </summary>
        public float Depth;

        /// <summary>
        /// Gets the number of vectors which make up the primtive line object.
        /// </summary>
        public int CountVectors
        {
            get
            {
                return vectors.Count;
            }
        }

        /// <summary>
        /// Creates a new primitive line object.
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device object to use.</param>
        public PrimitiveLine(GraphicsDevice GraphicsDevice, SpriteBatch SpriteBatch, Color color)
        {
            // create pixels
            pixel = new Texture2D(GraphicsDevice, 1, 1, true, SurfaceFormat.Color); 
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixels[0] = color;
            pixel.SetData<Color>(pixels);

            Colour = color;
            Position = new Vector2(0, 0);
            Depth = 0;

            vectors = new ArrayList();
            graphicsDevice = GraphicsDevice;
            spriteBatch = SpriteBatch;
        }

        /// <summary>
        /// Called when the primive line object is destroyed.
        /// </summary>
        ~PrimitiveLine()
        {
        }

        /// <summary>
        /// Adds a vector to the primive live object.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        public void AddVector(Vector2 vector)
        {
            vectors.Add(vector);
        }

        /// <summary>
        /// Insers a vector into the primitive line object.
        /// </summary>
        /// <param name="index">The index to insert it at.</param>
        /// <param name="vector">The vector to insert.</param>
        public void InsertVector(int index, Vector2 vector)
        {
            vectors.Insert(index, vectors);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="vector">The vector to remove.</param>
        public void RemoveVector(Vector2 vector)
        {
            vectors.Remove(vector);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="index">The index of the vector to remove.</param>
        public void RemoveVector(int index)
        {
            vectors.RemoveAt(index);
        }

        /// <summary>
        /// Clears all vectors from the primitive line object.
        /// </summary>
        public void ClearVectors()
        {
            vectors.Clear();
        }

        /// <summary>
        /// Renders the primtive line object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to render the primitive line object.</param>
        public void Render(SpriteBatch spriteBatch)
        {
            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                Vector2 vector1 = (Vector2)vectors[i - 1];
                Vector2 vector2 = (Vector2)vectors[i];

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                // stretch the pixel between the two vectors
                spriteBatch.Draw(pixel,
                    Position + vector1,
                    null,
                    Colour,
                    angle,
                    Vector2.Zero,
                    new Vector2(distance, 1),
                    SpriteEffects.None,
                    Depth);
            }
        }

        /// <summary>
        /// Creates a circle starting from 0, 0.
        /// </summary>
        /// <param name="radius">The radius (half the width) of the circle.</param>
        /// <param name="sides">The number of sides on the circle (the more the detailed).</param>
        public void CreateCircle(float radius, int sides)
        {
            vectors.Clear();

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for (float theta = 0; theta < max; theta += step)
            {
                vectors.Add(new Vector2(radius * (float)Math.Cos((double)theta),
                    radius * (float)Math.Sin((double)theta)));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2(radius * (float)Math.Cos(0),
                    radius * (float)Math.Sin(0)));
        }

        public void CreateRectangle(Rectangle myRectangle)
        {


            Vector2 TopLeft = new Vector2(myRectangle.Left, myRectangle.Top);
            Vector2 TopRight = new Vector2(myRectangle.Right, myRectangle.Top);
            Vector2 BottomLeft = new Vector2(myRectangle.Left, myRectangle.Bottom);
            Vector2 BottomRight = new Vector2(myRectangle.Right, myRectangle.Bottom);

            PrimitiveLine Topbrush = new PrimitiveLine(graphicsDevice,spriteBatch, Colour);
            Topbrush.AddVector(TopLeft);
            Topbrush.AddVector(TopRight);

            PrimitiveLine Bottombrush = new PrimitiveLine(graphicsDevice, spriteBatch, Colour);
            Bottombrush.AddVector(BottomLeft);
            Bottombrush.AddVector(BottomRight);

            PrimitiveLine Leftbrush = new PrimitiveLine(graphicsDevice, spriteBatch, Colour);
            Leftbrush.AddVector(TopLeft);
            Leftbrush.AddVector(BottomLeft);

            PrimitiveLine Rightbrush = new PrimitiveLine(graphicsDevice, spriteBatch, Colour);
            Rightbrush.AddVector(BottomRight);
            Rightbrush.AddVector(TopRight);

            Topbrush.Render(spriteBatch);
            Bottombrush.Render(spriteBatch);
            Leftbrush.Render(spriteBatch);
            Rightbrush.Render(spriteBatch);          
        }


        //redo this, it doesn't work atm
        public void CreateThickRectangle(Rectangle myRectangle, int thickness)
        {
            if (thickness < 1 || thickness > 10) return;

            for (int i = 0 - thickness; i < thickness; i++)
            {
                Rectangle currentRect = new Rectangle(myRectangle.X + i, myRectangle.Y + i, myRectangle.Width + Math.Abs(i), myRectangle.Height + Math.Abs(i));
                CreateRectangle(currentRect);
            }

        }

        public void CreateFilledRect(Rectangle myRect, Color myColor)
        {
            spriteBatch.Draw(pixel, myRect, myColor);
        }
    }
}