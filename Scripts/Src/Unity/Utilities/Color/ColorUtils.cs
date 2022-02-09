using UnityEngine;

namespace CXUtils.Common
{
	///<summary> Options for calculating luminance of the color </summary>
	public enum LumaConvertType
	{
		///<summary> calculates color using averaging </summary>
		Average,

		///<summary> Standard method </summary>
		Standard,

		///<summary> For the Sake of performance (Lose of accuracy) </summary>
		Performance
	}

	/// <summary>
	///     Color class that helps with color manipulation
	/// </summary>
	public static class ColorUtils
	{
		/// <summary>
		///     Unity doesn't have more predefined colors? Use this! :D
		///     <para>Credits: All colors from <seealso cref="https://en.wikipedia.org/wiki/Web_colors" /></para>
		/// </summary>
		public struct Colors
		{
			//Colors all From Wikipedia: Web_Colors => https://en.wikipedia.org/wiki/Web_colors

			//Red Colors
			public static readonly Color MistyRose = new Color(1, .89f, 1);
			public static readonly Color Crimson   = new Color(.86f, .07f, .23f);

			//Red Blue Colors                           
			public static readonly Color Purple     = new Color(.5f, 0, .5f);
			public static readonly Color Violet     = new Color(.93f, .5f, .93f);
			public static readonly Color DarkViolet = new Color(.49f, 0, 1);
			public static readonly Color DarkOrchid = new Color(.52f, .19f, .8f);

			//Blue Colors
			public static readonly Color MidnightBlue = new Color(.09f, .09f, .43f);
			public static readonly Color Teal         = new Color(0, .5f, .5f);
			public static readonly Color SkyBlue      = new Color(.52f, .8f, .92f);
			public static readonly Color DeepSkyBlue  = new Color(0, .74f, 1);

			//Green Colors
			public static readonly Color Lime = new Color(0, 1, 0);

			//Yellow Colors
			public static readonly Color Gold        = new Color(1f, .84f, 0);
			public static readonly Color GreenYellow = new Color(.67f, 1, .18f);

			//White To Black Colors
			public static readonly Color SlateGray = new Color(.43f, .5f, .56f);

			//Other Colors
			public static readonly Color Copper = new Color(.72f, .45f, .2f);
		}

		#region Script Methods

		#region Manipulate Colors

		/// <summary>
		///     Mapping A Color In A Color Range To Another Color Range
		/// </summary>
		public static Color Map(this Color value, Color inMin, Color inMax, Color outMin, Color outMax) =>
			new Color
			{
				r = MathUtils.Map(value.r, inMin.r, inMax.r, outMin.r, outMax.r),
				g = MathUtils.Map(value.g, inMin.g, inMax.g, outMin.g, outMax.g),
				b = MathUtils.Map(value.b, inMin.b, inMax.b, outMin.b, outMax.b),
				a = MathUtils.Map(value.a, inMin.a, inMax.a, outMin.a, outMax.a)
			};

		#endregion

		#region Brightness

		///<summary> Get's the gray scale / brightness of the color </summary>
		public static float GetBrightness(this Color color, LumaConvertType lumaConvertType = LumaConvertType.Average)
		{
			switch (lumaConvertType)
			{
				case LumaConvertType.Average:     return GetBrightnessAverage(color);
				case LumaConvertType.Standard:    return GetBrightnessStandard(color);
				case LumaConvertType.Performance: return GetBrightnessPerformance(color);

				default: throw ExceptionUtils.NotAccessible;
			}
		}

		///<summary> Get's the gray scale of the color by getting the average of the color </summary>
		public static float GetBrightnessAverage(this Color color) => (color.r + color.g + color.b) / 3f;

		const float LUMA_PRECISION_R = .2126f,
			LUMA_PRECISION_G         = .7152f,
			LUMA_PRECISION_B         = .0722f;

		///<summary> Get's the gray scale of the color using the Luminosity method </summary>
		public static float GetBrightnessStandard(this Color color) =>
			LUMA_PRECISION_R * color.r + LUMA_PRECISION_G * color.g + LUMA_PRECISION_B * color.b;

		/// <summary> Same as <see cref="GetBrightnessStandard" />, but more efficient </summary>
		public static float GetBrightnessPerformance(this Color color) =>
			(color.r + color.r + color.b + color.g + color.g + color.g) / 6f;

		///<summary> Get's the color of the GrayScale value with the given GrayScale value </summary>
		public static Color FromBrightness(float grayScale, float alpha = 1) =>
			new Color(grayScale, grayScale, grayScale, alpha);

		#endregion

		#region Convert

		const float BYTE_MAX_FLOAT = 255f;

		/// <summary>
		///     Converts 255 based Colors into unity Colors
		/// </summary>
		public static Color ByteToFloat(int r, int g, int b, int a) =>
			new Color(r / BYTE_MAX_FLOAT, g / BYTE_MAX_FLOAT, b / BYTE_MAX_FLOAT, a / BYTE_MAX_FLOAT);

		public static Color ByteToFloat(byte r, byte g, byte b, byte a) =>
			new Color(r / BYTE_MAX_FLOAT, g / BYTE_MAX_FLOAT, b / BYTE_MAX_FLOAT, a / BYTE_MAX_FLOAT);

		#endregion

		#endregion
	}
}
