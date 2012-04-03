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
		static bool PlayerAlive = true;  
		static int DeathTimer = 0;
		static Point DeathPoint = new Point(0,0);
		
	//	static List<Surface> sfcFish = new List<Surface>();
		static List<Surface> sfcFishR = new List<Surface>(); //Going Left to Right or facing right
		static List<Surface> sfcFishL = new List<Surface>(); //Going right to left or facing left
		static List<Surface> sfcNumber = new List<Surface>();
		
		static int PlayerFish = 1;
		static Point PlayerPos = new Point(256, 512);
		static int PlayerScore = 0;

		static List<Fish> AIFish = new List<Fish>();
		static int LastFishCreation = 0;
				
		public static void Main(string[] args)
		{
			sfcMain = Video.SetVideoMode(1024, 768);
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
					PlayerPos.Y -= 10;
				} else if (Keyboard.IsKeyPressed(Key.DownArrow)) {
					PlayerPos.Y += 10;
				}

				if (Keyboard.IsKeyPressed(Key.LeftArrow)) {
					PlayerPos.X -= 10;
				} else if (Keyboard.IsKeyPressed(Key.RightArrow)) {
					PlayerPos.X += 10;
				}
			
				sfcMain.Blit(sfcBackground);
				sfcMain.Blit(sfcScoreHeader, new Point(0,708));
				DoScore();
				
				// check if player's fish is alive?
				if (PlayerAlive) sfcMain.Blit(sfcFishR[PlayerFish], PlayerPos);
				else 
				{
					sfcMain.Blit(sfcFishL[4], DeathFloat(PlayerPos));	
				}
				
				//Recode for left or right facing fish surfaces............
				
				Rectangle player_rect = new Rectangle(PlayerPos.X, PlayerPos.Y,	sfcFishR[PlayerFish].Width, sfcFishR[PlayerFish].Height);
			
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
						if (computerfish.Type < PlayerFish) {
							AIFish.RemoveAt(i);
							PlayerScore += 1;
						} else if (computerfish.Type > PlayerFish) {
							PlayerAlive = false;
							
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
		
		static void DoScore() 
		{
			string StringScore = PlayerScore.ToString();
			int x = 180;
			
			if (PlayerScore == 0) StringScore = "0";
			
			for (int i = 0; i < StringScore.Length; i++)
			{
				string num = Char.ConvertFromUtf32(Convert.ToInt32(StringScore[i]));
				int y = int.Parse(num);
				Console.WriteLine("debug: "+ y);
				sfcMain.Blit(sfcNumber[y], new Point(x, 708));
				x += 60;
			}
			
		}
	}
}