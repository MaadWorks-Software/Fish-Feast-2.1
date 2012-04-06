//  Main.cs
//  
//  Author:
//       Devon J Weaver <devon.weaver@gmail.com>
// 
//  Copyright (c) 2010 MaadWorks Software
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.



using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace FishFeast
{
	class MainClass
	{
		static Surface sfcMain;
		static Surface sfcBackground;
		static Surface sfcLogo;
		static Surface sfcInfo;
		static Surface sfcScoreHeader;
		
		static bool GameStarted;
		// static bool PlayerAlive = true;  //  changed to isAlive a bool property in PlayerFish class, TODO: remove completely
		static int DeathTimer = 0;
		static Point DeathPoint = new Point(0,0);
		
		static List<Surface> sfcFishR = new List<Surface>(); //Going Left to Right or facing right
		static List<Surface> sfcFishL = new List<Surface>(); //Going right to left or facing left
		static List<Surface> sfcNumber = new List<Surface>(); // Holds the graphics for the score
		// TODO: need list to hold all power up items
		
		
		//  added all player related properties to the playerFish class
		static PlayerFish playerFish = new PlayerFish();	//  instance of PlayerFish
		static Point PlayerPos = new Point(256, 512);		// TODO: remove 
		// static int PlayerScore = 0;							//  change to Score property in Playerfish class, TODO remove completely

		static List<Fish> AIFish = new List<Fish>();
		static int LastFishCreation = 0;
				
		public static void Main(string[] args)
		{
			
			// Setup all graphics and windows
			// then run it!
			sfcMain = Video.SetVideoMode(1024, 768);
			loadMedia();
			
			Events.Quit += new EventHandler<QuitEventArgs>(Events_Quit);
			Events.Tick += new EventHandler<TickEventArgs>(Events_Tick);
			Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(Events_KeyboardUp);

			Events.Run();
		}
		
		static void Events_Quit(object sender, QuitEventArgs e) {
			Events.QuitApplication();
		}

		static void Events_Tick(object sender, TickEventArgs e) {
			Rectangle enemy_rect;
			
			
			
			if (GameStarted) {				
				if (LastFishCreation + 1000 < Timer.TicksElapsed) {
					CreateFish();
				}
			
				
				if (Keyboard.IsKeyPressed(Key.UpArrow)) {
					playerFish.Pos.Y -= 10;
				} else if (Keyboard.IsKeyPressed(Key.DownArrow)) {
					playerFish.Pos.Y += 10;
				}

				if (Keyboard.IsKeyPressed(Key.LeftArrow)) {
					playerFish.Pos.X -= 10;
				} else if (Keyboard.IsKeyPressed(Key.RightArrow)) {
					playerFish.Pos.X += 10;
				}
			
				sfcMain.Blit(sfcBackground);
				sfcMain.Blit(sfcScoreHeader, new Point(0,708));
				DoScore();
				
				// check if player's fish is alive?
				// TODO: need to reference the PlayerFish.isAlive when inplemented
				if (playerFish.IsAlive) sfcMain.Blit(sfcFishR[playerFish.Type], playerFish.Pos);
				else 
				{
					sfcMain.Blit(sfcFishL[4], DeathFloat(playerFish.Pos));	
				}
				
				// TODO: Recode for left or right facing fish surfaces............
				
				Rectangle player_rect = new Rectangle(playerFish.Pos.X, playerFish.Pos.Y,	sfcFishR[playerFish.Type].Width, sfcFishR[playerFish.Type].Height);
			
				for (int i = AIFish.Count - 1; i >= 0; --i) {
					Fish computerfish = AIFish[i];
					
					if (computerfish.RightFaced == true) 
					{
						computerfish.Pos.X += computerfish.Speed;
						enemy_rect = new Rectangle(computerfish.Pos.X, computerfish.Pos.Y, sfcFishR[computerfish.Type].Width, sfcFishR[computerfish.Type].Height);
					} else
					{
						computerfish.Pos.X -= computerfish.Speed;
						enemy_rect = new Rectangle(computerfish.Pos.X, computerfish.Pos.Y, sfcFishL[computerfish.Type].Width, sfcFishL[computerfish.Type].Height);
					}
					if (enemy_rect.IntersectsWith(player_rect)) {
						if (computerfish.Type < playerFish.Type) {
							AIFish.RemoveAt(i);
							playerFish.addScore(1);
						} else if (computerfish.Type > playerFish.Type) {
							playerFish.IsAlive = false;
							
						}
					}
					if (computerfish.RightFaced == true) sfcMain.Blit(sfcFishR[computerfish.Type], computerfish.Pos);
					else sfcMain.Blit(sfcFishL[computerfish.Type], computerfish.Pos);
				}

			} else {
				sfcMain.Blit(sfcBackground);
				sfcMain.Blit(sfcLogo, new Point(254, 236));
				sfcMain.Blit(sfcInfo, new Point(254, 400));
			}
			
			sfcMain.Update();
		}
		
		static void Events_KeyboardUp(object sender, KeyboardEventArgs e) {
			if (e.Key == Key.Space) {
				if (!GameStarted) {
					GameStarted = true;
				} else {
					// game already started!
				}
			}
		}
		
		static void CreateFish() {
			Random rand = new Random();

			int fishtype = 0;
			bool rightfaced;
			int fishx;
			int fishy;

			switch (rand.Next(20)) {
				case 14:
				case 15:
				case 16:
					fishtype = 1;
					break;
				case 17:
				case 18:
					fishtype = 2;
					break;
				case 19:
					fishtype = 3;
					break;
			}
			//create right or left facing fish
			if (rand.Next(6) % 2 == 0) 
			{		
				fishx = 0;
				fishy = rand.Next(700);
				rightfaced = true;
				
			} else
			{
				fishx = 1024;
				fishy = rand.Next(700);
				rightfaced = false;

			}
			
			Fish newfish = new Fish(fishtype, new Point(fishx, fishy), rightfaced);
	
			// now set the speed to 5, with a little bit of randomness!
			newfish.Speed = 5 + rand.Next(-3, 3);
			AIFish.Add(newfish);

			LastFishCreation = Timer.TicksElapsed;
		}
		
		
		static Point DeathFloat(Point initpoint)
		{
			
			
			if (DeathTimer == 0)
			{
				DeathTimer = 1;
				DeathPoint = initpoint;
				return initpoint;
			}
			else if (DeathTimer < 26)
			{
				DeathTimer++;
				DeathPoint.X += 5;
				DeathPoint.Y += 2;
				return DeathPoint;
			} else
			{
				if (DeathTimer == 51) DeathTimer = 1; //reset after 50 cycles. 
				DeathTimer++;
				DeathPoint.X -= 5;
				DeathPoint.Y += 1;
				return DeathPoint;
			}
		
		}
		
		/// <summary>
		/// Displays the score based on the player score.
		/// </summary>
		static void DoScore() 
		{
			int x = 180;
			int digit = 0;
			List<int> scoreDigits = new List<int>();
			int i = 0;
			
			for (int displayScore = playerFish.Score; displayScore > 0; displayScore /= 10)
			{
				digit = displayScore % 10;
				scoreDigits.Add(digit);
				i++;
			}
				
			
			for (i = scoreDigits.Count - 1 ;i > -1; i--) 
			{
				digit = scoreDigits[i];
				sfcMain.Blit(sfcNumber[digit], new Point(x, 708));
				x += 60;
			}
			scoreDigits.Clear();
		}
		
		
		/// <summary>
		/// Loads the media.
		/// </summary>
		static void loadMedia() {
			
			sfcBackground = new Surface("media/background.jpg");
			sfcLogo = new Surface("media/logo.png");
			sfcInfo = new Surface("media/Info.png");
			sfcScoreHeader = new Surface("media/scoreHeader.png");
			
			sfcFishL.Add(new Surface("media/fish1.png"));
			sfcFishL.Add(new Surface("media/fish2.png"));
			sfcFishL.Add(new Surface("media/fish3.png"));
			sfcFishL.Add(new Surface("media/fish4.png"));
			sfcFishL.Add(new Surface("media/DeadPlayerFish.png"));
			
			sfcFishR.Add(new Surface("media/fish1_r.png"));
			sfcFishR.Add(new Surface("media/fish2_r.png"));
			sfcFishR.Add(new Surface("media/fish3_r.png"));
			sfcFishR.Add(new Surface("media/fish4_r.png"));
			
			sfcNumber.Add(new Surface("media/0.png"));
			sfcNumber.Add(new Surface("media/1.png"));              
			sfcNumber.Add(new Surface("media/2.png"));
			sfcNumber.Add(new Surface("media/3.png"));
			sfcNumber.Add(new Surface("media/4.png"));
			sfcNumber.Add(new Surface("media/5.png"));
			sfcNumber.Add(new Surface("media/6.png"));
			sfcNumber.Add(new Surface("media/7.png"));
			sfcNumber.Add(new Surface("media/8.png"));
			sfcNumber.Add(new Surface("media/9.png"));
			
			// TODO: need to add loading of graphic to a surface list
		}
	}
}