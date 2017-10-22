using UnityEngine;
using System.Collections;

public class CloudTexTools
{
	/// <summary>
	/// Flips the texture horizontally.
	/// </summary>
	/// <returns>The flipped texture.</returns>
	/// <param name="original">The texture to flip.</param>
	public static Texture2D FlipTexture(Texture2D original)
	{
		Texture2D flipped = new Texture2D(original.width, original.height, TextureFormat.ARGB32, false);
		
		int xN = original.width;
		int yN = original.height;
		
		for(int i = 0; i < xN; i++)
		{
			for(int j = 0; j < yN; j++) 
			{
				flipped.SetPixel(xN - i - 1, j, original.GetPixel(i,j));
			}
		}
		
		flipped.Apply();
		
		return flipped;
	}

    /// <summary>
	/// Draws arrowed line on Texture2D.
	/// </summary>
	/// <param name="a_Texture">The texture.</param>
	/// <param name="x1">The first x value.</param>
	/// <param name="y1">The first y value.</param>
	/// <param name="x2">The second x value.</param>
	/// <param name="y2">The second y value.</param>
	/// <param name="a_Color">The color.</param>
    /// <param name="arrowHeadLen">The length of the arrow head. 20px by default</param>
    /// <param name="arrowHeadAngle">The angle between line and arrow head. 30 degrees by default</param>

    public static void DrawArrow(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color, int arrowHeadLen, int arrowHeadAngle)
    {
		DrawLine(a_Texture, x1, y1, x2, y2, a_Color);

        float angle = Mathf.Atan2(y1 - y2, x1 - x2) * Mathf.Rad2Deg;

        float x = x2 + arrowHeadLen * Mathf.Cos((angle - arrowHeadAngle) * Mathf.Deg2Rad);
        float y = y2 + arrowHeadLen * Mathf.Sin((angle - arrowHeadAngle) * Mathf.Deg2Rad);

        DrawLine(a_Texture, x2, y2, (int)x, (int)y, a_Color);

        x = x2 + arrowHeadLen * Mathf.Cos((angle + arrowHeadAngle) * Mathf.Deg2Rad);
        y = y2 + arrowHeadLen * Mathf.Sin((angle + arrowHeadAngle) * Mathf.Deg2Rad);

        DrawLine(a_Texture, x2, y2, (int)x, (int)y, a_Color);
    }

    /// <summary>
    /// Draws rectangle on Texture2D.
    /// </summary>
    /// <param name="a_Texture">The texture.</param>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="w">The width.</param>
    /// <param name="h">The height.</param>
    /// <param name="a_Color">The color.</param>
    public static void DrawRect(Texture2D a_Texture, int x, int y, int w, int h, Color a_Color)
	{
		DrawLine(a_Texture, x, y, x + w - 1, y, a_Color);  // top
		DrawLine(a_Texture, x + w - 1, y, x + w - 1, y + h - 1, a_Color);  // right
		DrawLine(a_Texture, x, y + h - 1, x + w - 1, y + h - 1, a_Color);  // bottom
		DrawLine(a_Texture, x, y, x, y + h - 1, a_Color);  // left
	}

	/// <summary>
	/// Draws line on Texture2D.
	/// </summary>
	/// <param name="a_Texture">The texture.</param>
	/// <param name="x1">The first x value.</param>
	/// <param name="y1">The first y value.</param>
	/// <param name="x2">The second x value.</param>
	/// <param name="y2">The second y value.</param>
	/// <param name="a_Color">The color.</param>
	public static void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color)
	{
		int width = a_Texture.width;
		int height = a_Texture.height;
		
		y1 = height - y1;
		y2 = height - y2;
		
		int dy = y2 - y1;
		int dx = x2 - x1;

		int stepy = 1;
		if (dy < 0) 
		{
			dy = -dy; 
			stepy = -1;
		}
		
		int stepx = 1;
		if (dx < 0) 
		{
			dx = -dx; 
			stepx = -1;
		}
		
		dy <<= 1;
		dx <<= 1;
		
		if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
			for(int x = -1; x <= 1; x++)
				for(int y = -1; y <= 1; y++)
					a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
		
		if (dx > dy) 
		{
			int fraction = dy - (dx >> 1);
			
			while (x1 != x2) 
			{
				if (fraction >= 0) 
				{
					y1 += stepy;
					fraction -= dx;
				}
				
				x1 += stepx;
				fraction += dy;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					for(int x = -1; x <= 1; x++)
						for(int y = -1; y <= 1; y++)
							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		else 
		{
			int fraction = dx - (dy >> 1);
			
			while (y1 != y2) 
			{
				if (fraction >= 0) 
				{
					x1 += stepx;
					fraction -= dy;
				}
				
				y1 += stepy;
				fraction += dx;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					for(int x = -1; x <= 1; x++)
						for(int y = -1; y <= 1; y++)
							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		
	}
	
}
