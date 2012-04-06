// PlayerFish.cs
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

using System;
using System.Drawing;

namespace FishFeast
{
	public class PlayerFish : Fish
	{
		
		
		/// <summary>
		/// Initializes a new instance of the <see cref="FishFeast.PlayerFish"/> class.
		/// </summary>
		public PlayerFish() : base(1, new Point(256, 512), true) {
			this.GrowthSize = 1;
			this.IsAlive = true;
			
		}
		
		/// <summary>
		/// Gets or sets the size of the growth.
		/// </summary>
		/// <value>
		/// The multipler of the growth.
		/// </value>
		public int GrowthSize { 
			
			get;
			set;
		}
		
		/// <summary>
		/// Gets or sets the score.
		/// </summary>
		/// <value>
		/// The score.
		/// </value>
		public int Score {
			get;
			set;
		}
		
		
		/// <summary>
		/// Gets or sets a value indicating whether this instance is alive.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is alive; otherwise, <c>false</c>.
		/// </value>
		public bool IsAlive {
			get;
			set;
		}
	
		/// <summary>
		/// Adds to the score. Pass a negative number to subtract.
		/// </summary>
		/// <param name='score'>
		/// integer to be added to Score of this instance
		/// </param>
		public void addScore(int score) {
	
			this.Score += score;
		}	
		
	}
	
	
}

