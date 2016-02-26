using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace  tinki2
{
	class DESPLAZAMIENTOdeFONDO
	{
		private Texture2D texture;
		private Rectangle rectangulo1;
		private Rectangle rectangulo2;
		private DIRECCION direccion;
		private int x1;
		private int x2;

		private int velocidad;
		private int ANCHOFONDO;
		private int ALTOFONDO;

		public void Initialize(Texture2D texture,int x, int y,DIRECCION direccion,
		                       int velocidad,int ANCHOFONDO,int ALTOFONDO)                      
		{
			this.texture = texture;
			this.x1 = x;

			this.direccion = direccion;
			this.velocidad = velocidad;
			this.ANCHOFONDO = ANCHOFONDO;
			this.ALTOFONDO = ALTOFONDO;
			//inicializo los rectangulos donde se va a mover el fondo
			this.rectangulo1 = new Rectangle(x1,0,ANCHOFONDO,ALTOFONDO);
			this.x2 = x1 + rectangulo1.Width;
			this.rectangulo2 = new Rectangle(x2,0,ANCHOFONDO,ALTOFONDO);
		}
		public void Update(GameTime gametime,Texture2D texture)
		{
			this.texture = texture;
			//declaro con la velocidad que se mueve el fondo
			x1 -= velocidad;
			x2 -= velocidad;
			//reconstruyo los rectangulos en los cuales se va a mover el fondo
			rectangulo1 = new Rectangle(x1,0,ANCHOFONDO,ALTOFONDO);
			rectangulo2 = new Rectangle(x2,0,ANCHOFONDO,ALTOFONDO);

			//declaro condicion que hace que el fondo vuelva a empezar cuando termina
			if (rectangulo1.X + rectangulo1.Width == 0)
			{
				x1 = ANCHOFONDO;
			}
			if (rectangulo2.X + rectangulo1.Width == 0)
			{
				x2 = ANCHOFONDO;
			}

		}
		public void Draw(SpriteBatch spritebatch)
		{
			//pinto fondo y le pasa la imgen y sus posiciones
			spritebatch.Draw(texture,rectangulo1,Color.White);
			spritebatch.Draw(texture,rectangulo2,Color.White);
		}
	}
}

