using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace tinki2
{
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		//declaro variables
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Color colorpajaro;
		//declaro LOS SPRITE DEL PERSONAJE,ENEMIGO,FONDO,VIDAS Y MONEDA
		Texture2D SPRITEpajaroROJO;
		Texture2D SPRITEpajaroENEMIGO;
		Texture2D SPRITEparaMONEDA;
		Texture2D SPRITEparaBOMBAS;
		Texture2D fondo;
		Texture2D VIDA1;
		Texture2D VIDA2;
		Texture2D VIDA3;
		Texture2D spriteBONUS;
		//variable de la clase de fondo en movimiento
		DESPLAZAMIENTOdeFONDO fondoenmovimiento;
		ESTADOjuego estadojuego;
		//variables de manejo de pantalla
		int ANCHOdeFONDO;
		int ALTOdeFONDO;
		//declaro variables de animacion
		PAJAROVOLANDO pajaroanimado;
		Vector2 POSICIONpajaroROJO;
		Vector2 posicionBONUS;
		//defino variables para colision ENTRE PAJAROS personaje bombas Y MONEDAS
		Rectangle rectangulopersonaje;
		Rectangle rectanguloenemigo;
		Rectangle rectangulomonedas;
		Rectangle rectanguloBomba;
		Rectangle rectanguloBONUS;
		int velocidadPersonaje = 7;
		int VelocidadFondo = 6;
		int velocidadBONUS = 4;
		//defino variables para vidas
		int CONTvidas = 3;
		int Puntos = 0;
		int Puntos2 = 0;
		int TIEMPOtrascurrido;
		int TIEMPOtrascurrido2;
		int TIEMPOtrascurrido3;
		int TIEMPOtrascurrido4;
		int tiempoTRANCURRIDO5;
		bool estado = false;
		bool CONTAR = false;
		// Variable para manipular música
		Song musicadefondo;
		// Vaiable para manipular efectos de sonido
		public SoundEffect efectodeSonidoMONEDA;
		public SoundEffect efectodeSonidoBOMBAexplota;
		public SoundEffect efectodeSonidoBOMBAcayendo;
		// Una lista de vector2 para las pocisiones X,Y de las BOMBAS 
		List<Vector2> posicionesBOMBAS = new List<Vector2> ();
		double probabilidadBOMBAS = 0.002;
		// 3% 
		int velocidadBOMBAS = 3;
		Random aleatorio3 = new Random ();
		//una lista de vector2 para las posiciones x,y de las monedas
		List<Vector2> posicionmonedas = new List<Vector2> ();
		double probabilidadmonedas = 0.0060;
		//%3
		int velocidadmonedas = 2;
		Random aleatorio2 = new Random ();
		// Una lista de vector2 para las pocisiones X,Y de los pajaros a esquivar
		List<Vector2> posicionesEnemigo = new List<Vector2> ();
		double probabilidadEnemigo = 0.0050;
		// 3% 
		int velocidadEnemigo = 4;
		Random aleatorio = new Random ();
		//VECTOR POSICION PARA DECLARAR PUNTAJE NIVEL....
		Vector2 posicion1, POSICION3, POSICIONvida1, POSICIONvida2, POSICIONvida3;
		// SpriteFont: Tipo de variable que manipula Fuentes
		SpriteFont fuente1;
		SpriteFont fuente2;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "../../Content";
			//hago esto para controlar el ancho y el alto de la pantalla
			ANCHOdeFONDO = 1050;
			ALTOdeFONDO = 600;
			graphics.PreferredBackBufferWidth = ANCHOdeFONDO;
			graphics.PreferredBackBufferHeight = ALTOdeFONDO;
			graphics.IsFullScreen = true;		
		}

		protected override void Initialize ()
		{   
			//inicializo el fondo en movimiento
			fondoenmovimiento = new DESPLAZAMIENTOdeFONDO (); 
			estadojuego = ESTADOjuego.NOcorriendo;
			//POSICION DE FUENTE1 Y FUENTE2
			posicion1 = new Vector2 (800, 20);
			POSICION3 = new Vector2 (450, 0);
			POSICIONvida1 = new Vector2 (0, 0);
			POSICIONvida2 = new Vector2 (50, 0);
			POSICIONvida3 = new Vector2 (100, 0);
			fondoenmovimiento = new DESPLAZAMIENTOdeFONDO ();
			posicionBONUS = new Vector2 (500, 50);
			base.Initialize ();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);
			//cargo sprite fondo,PERSONAJE,ENEMIGOS Y MONEDAS Y BONUS del content
			fondo = Content.Load<Texture2D> ("FONDO/FONDO");
			SPRITEpajaroROJO = Content.Load<Texture2D> ("PERSONAJE/PAJARITOS5");
			SPRITEpajaroENEMIGO = Content.Load<Texture2D> ("ENEMIGOS/ENEMIGO1");
			SPRITEparaMONEDA = Content.Load<Texture2D> ("MONEDA/MONEDA");
			SPRITEparaBOMBAS = Content.Load<Texture2D> ("BOMBAS/bomba");
			spriteBONUS = Content.Load<Texture2D> ("BONUS/BONUSimagen");
			//cargo las fuentes del content
			fuente1 = Content.Load<SpriteFont> ("FUENTES/fuente1"); // Asignamos el XNB (.Spritefont convertido)
			fuente2 = Content.Load<SpriteFont> ("FUENTES/fuente2");
			//CARGO SPRITE DE PAJARITO VIDAS
			VIDA1 = Content.Load<Texture2D> ("VIDAS/VIDAS");
			VIDA2 = Content.Load<Texture2D> ("VIDAS/VIDAS");
			VIDA3 = Content.Load<Texture2D> ("VIDAS/VIDAS");
			//CREO FONDO DESPLAZANDOSE
			fondoenmovimiento.Initialize (fondo, 0, 0, DIRECCION.leftrigth, VelocidadFondo, ANCHOdeFONDO, ALTOdeFONDO);

			//Sonido de Fondo: Tema musical
			musicadefondo = Content.Load<Song> ("SONIDO/SonidoFondo.wav");
			MediaPlayer.Play (musicadefondo);
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.2f;
			//Efectos
			efectodeSonidoMONEDA = Content.Load<SoundEffect> ("SONIDO/SonidoMmoneda.wav");
			efectodeSonidoBOMBAexplota = Content.Load<SoundEffect> ("SONIDO/bombaexplota.wav");
			efectodeSonidoBOMBAcayendo = Content.Load<SoundEffect> ("SONIDO/bombacayendo.wav");

			//CREO LA ANIMACION
			pajaroanimado = new PAJAROVOLANDO ();
			Vector2 spritePos = new Vector2 (100, 100);

			pajaroanimado.Initialize (SPRITEpajaroROJO, spritePos, 174, 168, 5, 80, Color.White, 0.5f, true);
			/*
             * Donde
             * 1) playerTexture: es el grupo de sprite a animar.
             * 2) spritePos: Coordenadas de ubicación del sprite.
             * 3) 106: Ancho del sprite.
             * 4) 110: Alto del sprite.
             * 5) 6: Cantidad de sprites
             * 6) 80: Tiempo de refresco o muestreo de una imagen a otra.
             * 7) Color.White: Despliega la imagen en su color real White = transparente.
             * 8) 1.0f: escala del sprite.
             * 9) true: repite indefinidamente, false: solo una.
             */
			//TODO: use this.Content to load your game content here 
		}

		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update (GameTime gameTime)
		{
			colorpajaro = Color.White;
			//salir con clic en cerrar ventana
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit ();
			//TOMO ESTADO DEL TECLADO
			KeyboardState keyboard = Keyboard.GetState ();
			//salir con la tecla escape
			if (keyboard.IsKeyDown (Keys.Escape))
				this.Exit ();

			//controlo los estados del juego 
			if (estadojuego == ESTADOjuego.PasarNivel) {
				if (keyboard.IsKeyDown (Keys.Enter)) {
					estadojuego = ESTADOjuego.corriendo;
					POSICIONpajaroROJO = new Vector2 (100, 100);
					velocidadPersonaje = 7;
					probabilidadmonedas = 0.0065;
					velocidadmonedas = 3;
					velocidadEnemigo = 6;
					probabilidadEnemigo = 0.010;
					VelocidadFondo = 9;
					velocidadBOMBAS = 4;
					probabilidadBOMBAS = 0.003;
					TIEMPOtrascurrido = 0;
					TIEMPOtrascurrido2 = 0;
					TIEMPOtrascurrido3 = 0;
				}
			}
			if (estadojuego == ESTADOjuego.NOcorriendo || estadojuego == ESTADOjuego.termino) {
				//veo si el usuario preciona enter para q comienze el juego 
				if (keyboard.IsKeyDown (Keys.Enter)) {
					POSICIONpajaroROJO = new Vector2 (100, 100);
					if (estadojuego == ESTADOjuego.termino) {
						Puntos = 0;
						CONTvidas = 3;
						TIEMPOtrascurrido3 = 0;
						TIEMPOtrascurrido = 0;
						TIEMPOtrascurrido2 = 0;
						fondo = Content.Load<Texture2D> ("FONDO/FONDO");
					}
					estadojuego = ESTADOjuego.corriendo;
				}
			} else if ((estadojuego == ESTADOjuego.corriendo)||(estadojuego == ESTADOjuego.BONUS)) {
				//SONIDO  SONIDO   SONIDO  SONIDO 
				KeyboardState teclado = Keyboard.GetState ();
				if (teclado.IsKeyDown (Keys.Escape))
					this.Exit ();
				//Modifico Volumen
				if (teclado.IsKeyDown (Keys.D1))
					MediaPlayer.Volume = 0.1f;
				if (teclado.IsKeyDown (Keys.D2))
					MediaPlayer.Volume = 0.2f;
				if (teclado.IsKeyDown (Keys.D3))
					MediaPlayer.Volume = 0.3f;
				if (teclado.IsKeyDown (Keys.D4))
					MediaPlayer.Volume = 0.4f;
				if (teclado.IsKeyDown (Keys.D5))
					MediaPlayer.Volume = 0.5f;
				if (teclado.IsKeyDown (Keys.D6))
					MediaPlayer.Volume = 0.6f;
				if (teclado.IsKeyDown (Keys.D7))
					MediaPlayer.Volume = 0.7f;
				if (teclado.IsKeyDown (Keys.D8))
					MediaPlayer.Volume = 0.8f;
				if (teclado.IsKeyDown (Keys.D9))
					MediaPlayer.Volume = 1.0f;
				//FIN SONIDO  FIN SONIDO FIN SONIDO

				// Actualizar tiempo transcurrido para que caiga el pajarito rojo personaje
				TIEMPOtrascurrido += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
				if (TIEMPOtrascurrido > 18) {
					POSICIONpajaroROJO.Y += velocidadPersonaje;
					// Reinicio el tiempo  transcurrido a cero para q vuelva a caer
					TIEMPOtrascurrido = 0;
				}
				//controla el movimiento del pajaro rojo personaje
				if (keyboard.IsKeyDown (Keys.Up))
					POSICIONpajaroROJO.Y -= velocidadPersonaje;
				if (keyboard.IsKeyDown (Keys.Right))
					POSICIONpajaroROJO.X += velocidadPersonaje;
				if (keyboard.IsKeyDown (Keys.Left))
					POSICIONpajaroROJO.X -= velocidadPersonaje;

				// El método Clamp evita que el personaje salga de la pantalla  
				POSICIONpajaroROJO.X = MathHelper.Clamp (POSICIONpajaroROJO.X, 0, 950);
				POSICIONpajaroROJO.Y = MathHelper.Clamp (POSICIONpajaroROJO.Y, 30, 510);

				//CONTROLES PARA BOMBAS

				// aparecen BOMBAS SEGUN según la probabilidad 
				if (estadojuego != ESTADOjuego.BONUS) {
					if (aleatorio3.NextDouble () < probabilidadBOMBAS) { 
						float x = MathHelper.Clamp ((float)aleatorio3.NextDouble () *
							Window.ClientBounds.Width, 5, 995);
						posicionesBOMBAS.Add (new Vector2 (x, 0)); 
						efectodeSonidoBOMBAcayendo.Play ();
					}
					// actualizar cada BOMBA
					for (int K = 0; K < posicionesBOMBAS.Count; K++) { 
						// actualizo las posiciones de las BOMBAS
						posicionesBOMBAS [K] = new Vector2 (posicionesBOMBAS [K].X,
						                                    posicionesBOMBAS [K].Y + velocidadBOMBAS);
						// obtener el rectangulo de laS BOMBAS
						rectanguloBomba = new Rectangle ((int)posicionesBOMBAS [K].X, (int)posicionesBOMBAS [K].Y,
						                                 SPRITEparaBOMBAS.Width / 2, SPRITEparaBOMBAS.Height / 2);
						// obtener el rectángulo de la persona 
						rectangulopersonaje = new Rectangle ((int)POSICIONpajaroROJO.X, (int)POSICIONpajaroROJO.Y,
						                                     SPRITEpajaroROJO.Width / 14, SPRITEpajaroROJO.Height / 2);
						// eliminar las bombas cuando salen de la pantalla 
						if (posicionesBOMBAS [K].Y > Window.ClientBounds.Width) { 
							posicionesBOMBAS.RemoveAt (K);
							// decrecemos i, por que hay una bomba menos 
							K--;
						}
						//me fijo si se cruza con alguna bomba con el personaje
						if (rectangulopersonaje.Intersects (rectanguloBomba)) {
							CONTvidas = 0;
							posicionesBOMBAS.RemoveAt (K);
							K--;
							efectodeSonidoBOMBAexplota.Play ();
						}
					}
				}
				//CONTROLES MONEDA CONTROLES MONEDA

				// aparecen nuevas monedas según la probabilidad 
				if (aleatorio2.NextDouble () < probabilidadmonedas) { 
					float Y = MathHelper.Clamp ((float)aleatorio2.NextDouble () * //PARA Q LAS MONEDAS NO SE SALGAN
						Window.ClientBounds.Height, 30, 580);     //DEL ANCHO DE LA PANTALLA
					posicionmonedas.Add (new Vector2 (1000, Y)); 
				}
				// actualizar cada moneda
				for (int j = 0; j < posicionmonedas.Count; j++) { 
					// actualizo las posiciones de las monedas
					posicionmonedas [j] = new Vector2 (posicionmonedas [j].X - velocidadmonedas,
					                                   posicionmonedas [j].Y);
					// obtener el rectangulo de la moneda
					rectangulomonedas = new Rectangle ((int)posicionmonedas [j].X, (int)posicionmonedas [j].Y,
					                                   SPRITEparaMONEDA.Width / 2, SPRITEparaMONEDA.Height / 2);
					// obtener el rectángulo del personaje 
					rectangulopersonaje = new Rectangle ((int)POSICIONpajaroROJO.X, (int)POSICIONpajaroROJO.Y,
					                                     SPRITEpajaroROJO.Width / 14, SPRITEpajaroROJO.Height / 2);
					// eliminar las monedas cuando salen de la pantalla 
					if (posicionmonedas [j].X == -20) { 
						posicionmonedas.RemoveAt (j);
						// decrecemos j, por que hay una moneda menos 
						j--;
					}
					//me fijo si se cruza con alguna moneda y lo elimino de la lista
					if (rectangulopersonaje.Intersects (rectangulomonedas)) {
						posicionmonedas.RemoveAt (j);
						j--;
						Puntos = Puntos + 10;
						efectodeSonidoMONEDA.Play ();
					}
				}
				//CONTROLES DE LOS ENEMIGOS CONTROLES DE LOS ENEMIGOS
				if (estadojuego != ESTADOjuego.BONUS) {
					// aparecen nuevos enemigos según la probabilidad 
					if (aleatorio.NextDouble () < probabilidadEnemigo) { 
						float Y = MathHelper.Clamp ((float)aleatorio.NextDouble () * //PARA Q LOS PAJAROS ENEMIGOS
							Window.ClientBounds.Height, 30, 580);  //NO SALGAN DEL ANCHO DE LA PANTALLA
						posicionesEnemigo.Add (new Vector2 (990, Y)); 
					}
					// actualizar cada enemigo
					for (int i = 0; i < posicionesEnemigo.Count; i++) { 
						// actualizo las posiciones de los pajaros enemigos 
						posicionesEnemigo [i] = new Vector2 (posicionesEnemigo [i].X - velocidadEnemigo,
						                                     posicionesEnemigo [i].Y);
						// obtener el rectangulo del enemigo
						rectanguloenemigo = new Rectangle ((int)posicionesEnemigo [i].X,
						                                   (int)posicionesEnemigo [i].Y,
						                                   SPRITEpajaroENEMIGO.Width / 2, SPRITEpajaroENEMIGO.Height / 2);
						// obtener el rectángulo del personaje
						rectangulopersonaje = new Rectangle ((int)POSICIONpajaroROJO.X, (int)POSICIONpajaroROJO.Y,
						                                     SPRITEpajaroROJO.Width / 14, SPRITEpajaroROJO.Height / 2);
						// eliminar los enemigos cuando salen de la pantalla 
						if (posicionesEnemigo [i].X == -20) { 
							posicionesEnemigo.RemoveAt (i);
							// decrecemos i, por que hay un enemigo menos 
							i--;
						}
						//descuento vidas si choca con el enemigo
						if (rectangulopersonaje.Intersects (rectanguloenemigo)) {
							colorpajaro = Color.Aqua;
							//tomo tiempo para que la interseccion saque una vida
							TIEMPOtrascurrido3 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
							// evaluar colisión con el personaje y cambiamos el color
							if (TIEMPOtrascurrido3 > 150) {
								CONTvidas--;
								TIEMPOtrascurrido3 = 0;
							}
						}
					}
					//SACO VIDA CUANDO TOCA EL PISO
					TIEMPOtrascurrido2 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
					if (rectangulopersonaje.Y > 480) {
						colorpajaro = Color.Aqua;
						if (TIEMPOtrascurrido2 > 60) {
							CONTvidas--;
						}
						TIEMPOtrascurrido2 = 0;
					}
					//pregunto por las vidas cuantas tiene
					if (CONTvidas <= 0) {
						estadojuego = ESTADOjuego.termino;
					}
					//para darle la opcion de pausa con f1
					if (keyboard.IsKeyDown (Keys.F1) && estado == false) {
						estadojuego = ESTADOjuego.NOcorriendo;
						estado = true;
					}
					estado = false;
					//condicion para Pasar de nivel
					if (Puntos == 30) {
						estadojuego = ESTADOjuego.PasarNivel;
						fondo = Content.Load<Texture2D> ("FONDO/FONDO2");
						Puntos2 = Puntos;
						Puntos = Puntos + 1000;
					}
					//para que caiga el bonus
					if (Puntos == 1050) {
						tiempoTRANCURRIDO5 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
						if (tiempoTRANCURRIDO5 > 18) {
							posicionBONUS.Y += velocidadBONUS;
							// Reinicio el tiempo  transcurrido a cero para q vuelva a caer
							tiempoTRANCURRIDO5 = 0;
						}
						//CREO      EL BONUS    BONUS    BONUS   BONUS  BONUS
						// obtener el rectángulo del personaje
						rectangulopersonaje = new Rectangle ((int)POSICIONpajaroROJO.X, (int)POSICIONpajaroROJO.Y,
						                                     SPRITEpajaroROJO.Width / 14, SPRITEpajaroROJO.Height / 2);
						// obtener el rectángulo del sprite del bonus
						rectanguloBONUS = new Rectangle ((int)posicionBONUS.X, (int)posicionBONUS.Y,
						                                 spriteBONUS.Width / 7, spriteBONUS.Height / 2);

						if (rectangulopersonaje.Intersects (rectanguloBONUS)) {
							estadojuego = ESTADOjuego.BONUS;
							fondo = Content.Load<Texture2D> ("FONDO/fondoBonus");
							probabilidadmonedas = 0.1;
							CONTAR = true;
						}
					}
				}
				if (Puntos == 1300) {
					fondo = Content.Load<Texture2D> ("FONDO/FONDO2");
					estadojuego = ESTADOjuego.corriendo;
					probabilidadmonedas = 0.0065;
				}
				/*	if (CONTAR == true) {
						TIEMPOtrascurrido4 += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
						if (TIEMPOtrascurrido4 == 300) {
							fondo = Content.Load<Texture2D> ("FONDO/FONDO2");
							probabilidadmonedas = 0.0060;
							estadojuego = ESTADOjuego.corriendo;
							CONTAR = false;
						}
					}
*/
					pajaroanimado.Update (gameTime, POSICIONpajaroROJO, colorpajaro);
					fondoenmovimiento.Update (gameTime, fondo);
			}
			base.Update (gameTime);	
		}

		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
			//dibujo FONDO en pantalla
			fondoenmovimiento.Draw (spriteBatch);

			spriteBatch.Begin ();
			if (estadojuego == ESTADOjuego.corriendo) {
				//dibujo FONDO en pantalla
				fondoenmovimiento.Draw (spriteBatch);
				//dibujo fuentes
				spriteBatch.DrawString (fuente1, "PUNTAJE: " + Puntos, posicion1, Color.Aquamarine);
				if (Puntos < 100) {
					spriteBatch.DrawString (fuente2, "NIVEL 1", POSICION3, Color.BlueViolet);
				} else if (Puntos > 100) {
					spriteBatch.DrawString (fuente2, "NIVEL 2", POSICION3, Color.BlueViolet);
				}
				//DIBUJO PAJARITOS DE VIDAS
				if (CONTvidas == 3) { 
					spriteBatch.Draw (VIDA1, POSICIONvida1, Color.White);
					spriteBatch.Draw (VIDA2, POSICIONvida2, Color.White);
					spriteBatch.Draw (VIDA3, POSICIONvida3, Color.White);
				}
				if (CONTvidas == 2) { 
					spriteBatch.Draw (VIDA1, POSICIONvida1, Color.White);
					spriteBatch.Draw (VIDA2, POSICIONvida2, Color.White);
				}
				if (CONTvidas == 1) { 
					spriteBatch.Draw (VIDA1, POSICIONvida1, Color.White);
				}
				//DIBUJO BONUS
				if (Puntos == 1050) {
					spriteBatch.Draw (spriteBONUS, posicionBONUS, Color.White);
				}
				// Dibujo enemigos
				foreach (Vector2 posicionEnemigo in posicionesEnemigo) { 
					spriteBatch.Draw (SPRITEpajaroENEMIGO, posicionEnemigo, Color.White);
				}
				//dibujo MONEDAS
				foreach (Vector2  posicionMONEDAS in posicionmonedas) { 
					spriteBatch.Draw (SPRITEparaMONEDA, posicionMONEDAS, Color.White);
				}
				// dibujo bombas
				foreach (Vector2 posicionesbombas in posicionesBOMBAS) { 
					spriteBatch.Draw (SPRITEparaBOMBAS, posicionesbombas, Color.White);
				}
				//DIBUJO PERSONAJE
				pajaroanimado.Draw (spriteBatch, colorpajaro);
			} else if (estadojuego == ESTADOjuego.NOcorriendo) {
				//CUANDO EL JUEGO NO ESTA CORRIENDO
				spriteBatch.DrawString (fuente1, "presione ENTER para empezar", new Vector2 (100, 300), Color.Beige);
			} else if (estadojuego == ESTADOjuego.termino) {
				//para q el juego vuelva a empazar
				spriteBatch.DrawString (fuente1, "HAS PERDIDO VUELVE A INTENTARLO", new Vector2 (250, 200), Color.Beige);
				spriteBatch.DrawString (fuente1, "TU PUNTAJE FUE : " + Puntos, new Vector2 (250, 300), Color.Beige);
				spriteBatch.DrawString (fuente1, "presione ENTER para empezar de nuevo", new Vector2 (250, 400), Color.Beige);
			} else if (estadojuego == ESTADOjuego.PasarNivel) {
				spriteBatch.DrawString (fuente1, "HAS PASADO DE NIVEL !!!!!!!!!!!!", new Vector2 (250, 200), Color.Beige);
				spriteBatch.DrawString (fuente1, "TU PUNTAJE FUE :                     " + Puntos2, new Vector2 (250, 300), Color.Beige);
				spriteBatch.DrawString (fuente1, "PUNTOS POR PASAR DE PANTALLA + 1000  ", new Vector2 (250, 350), Color.Beige);
				spriteBatch.DrawString (fuente1, "presione ENTER para CONTINUAR CON EL NIVEL 2", new Vector2 (250, 450), Color.Beige);
			}
			if (estadojuego == ESTADOjuego.BONUS) {
				//DIBUJO PERSONAJE
				pajaroanimado.Draw (spriteBatch, colorpajaro);
				//dibujo MONEDAS
				foreach (Vector2  posicionMONEDAS in posicionmonedas) { 
					spriteBatch.Draw (SPRITEparaMONEDA, posicionMONEDAS, Color.White);
				}
				spriteBatch.DrawString (fuente1, "PUNTAJE: " + Puntos, posicion1, Color.Aquamarine);
			}
			spriteBatch.End ();
			base.Draw (gameTime);
		}
	}
}


