// PlayerFish.cs
//  
//  Author:
//       Devon J Weaver <devon.weaver@gmail.com>
// 
//  Copyright (c) 2012 MaadWorks Software
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
	/// <summary>
	/// Base object.
	/// </summary>
	public class BaseObject
	{
		/// <summary>
		/// The position of the object
		/// </summary>
		public Point Pos;
		
		/// <summary>
		/// Gets or sets the speed.
		/// </summary>
		/// <value>
		/// The speed.
		/// </value>
		public int Speed {
			get;
			set;
		}
		
		public BaseObject ()
		{
		}
	}
}

