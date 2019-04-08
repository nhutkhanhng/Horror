// -------------------------------------------
// Control Freak 2
// Copyright (C) 2013-2016 Dan's Game Tools
// http://DansGameTools.blogspot.com
// -------------------------------------------

//! \cond

using UnityEngine;

namespace ControlFreak2.Internal
{
public struct CFRect
	{
	public float x, y, w, h;
	private const float EPSILON = 0.0001f;

	public bool		isEmpty { get { return ((this.w <= EPSILON) || (this.h <= EPSILON)); } }
	public float	xmax 	{ get { return (this.x + this.w); }  }
	public float	ymax 	{ get { return (this.y + this.h); }  }
	

	// ---------------------
	public CFRect(float x, float y, float w, float h)
		{
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
		}
	
	// -------------------
	public CFRect(Rect r)
		{
		this.x = (r.x);
		this.y = (r.y);
		this.w = (r.width);
		this.h = (r.height);
		}
	

	// ---------------------
		public void Add(float x, float y) { Add(x, y, 0.001f); }

		public void Add(float x, float y, float rad)
		{		
		if ((this.w <= 0) || (this.h <= 0))
			{
			this.x = x - rad;
			this.y = y - rad;
			this.w = rad * 2;
			this.h = rad * 2;
			}
		else
			{	
			float x1 = this.x + this.w;	
			float y1 = this.y + this.h;
			
			if 		((x - rad) < this.x)	this.x	= (x - rad);
			else if ((x + rad) > x1)		x1		= (x + rad);
			if 		((y - rad) < this.y)	this.y	= (y - rad);
			else if ((y + rad) > y1)		y1 		= (y + rad);


			this.w = (x1 - this.x);			
			this.h = (y1 - this.y);				
			}	
		}

	
	// ---------------------
	public void Add(float x, float y, float w, float h)
		{
		if ((w <= EPSILON) || (h <= EPSILON)) 
			return;

		this.Add(x,		y);
		this.Add(x + w,	y + h);
		}
	
	// ---------------------
	public void Add(CFRect rect)
		{
		if (rect.isEmpty)
			return;

		this.Add(rect.x, rect.y);
		this.Add(rect.x + rect.w,	rect.y + rect.h);
		}

	// ---------------------
	public void Add(Rect rect)
		{
		if ((rect.width <= EPSILON) || (rect.height <= EPSILON))
			return;

		this.Add(rect.x, rect.y);
		this.Add(rect.x + rect.width,	rect.y + rect.height);
		}



	// -------------------
	public bool IsOverlappedBy(CFRect rect)
		{
		return (!
			((this.xmax 	< rect.x) 	|| (this.ymax 	< rect.y) ||
			(rect.xmax		< this.x) 	|| (rect.ymax	< this.y)) );			
		}

	// ------------------
	public bool IsOverlappedBy(float x, float y, float w, float h)
		{
		return (!
			((this.xmax 	< x) 		|| (this.ymax 	< y) ||
			((x + w) 		< this.x) 	|| ((y + h)		< this.y)) );			
		}

	
	
	// ---------------------
	static public CFRect ClipInside(CFRect rect, CFRect clip)
		{
		float rx1 = rect.xmax;
		float ry1 = rect.ymax;

		if (rect.x  < clip.x) 		rect.x 	= clip.x;
		if (rx1 	> clip.xmax) 	rx1 	= clip.xmax;	 
		if (rect.y  < clip.y) 		rect.y 	= clip.y;
		if (ry1 	> clip.ymax) 	ry1 	= clip.ymax;	

		rect.w = (rx1 - rect.x);
		rect.h = (ry1 - rect.y);

		return rect; 
		}


	// ---------------------
	public Rect ToRect()
		{
		return new Rect(this.x, this.y, this.w, this.h);
		} 

	// ------------------
	override public string ToString()
		{
		return ("CFRect(" + this.x + ", " + this.y + " " + this.w + " x " + this.h + ")");
		}
	}

}


//! \endcond

