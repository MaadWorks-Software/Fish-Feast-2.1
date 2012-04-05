// Fish.cs
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
	public class Fish
	{
			
			
			// Private fields
			private int _type;
		
			public bool RightFaced;
			

			public Fish(int type, Point pos, bool rightfaced) {
				Type = type;
				Pos = pos;
				RightFaced = rightfaced;
			
			}
		
			public int Speed {
				get;
				set;
			}
		
			
			/// <summary>
			/// Gets or sets the type of fish from 0 to 3.
			/// </summary>
			/// <value>
			/// The fish type an integer from 0..3.
			/// </value>
			public int Type {
				get 
				{
					return this._type;
				}
				set 
				{
					if (value < 0 || value > 3) 
						this._type = 0;
					this._type = value;
				}
			}
			
			public Point Pos {
				get;
				set;
			}
	}
}

