using UnityEngine;
using UnityEngine.UI;
using System;

public static class UnityExtMethods
{
	public static void InvertMotor(this SliderJoint2D slider)
	{
		if(slider.useMotor)
		{
			JointMotor2D motor = slider.motor;
			motor.motorSpeed *= -1;
			slider.motor = motor;
		}
	}

	public static void IncrementMaxLimits(this SliderJoint2D slider, float val)
	{
		if(slider.useLimits)
		{
			JointTranslationLimits2D limits = slider.limits;
			limits.max += val;
			slider.limits = limits;
		}
	}

    public static void ToggleActive(this GameObject gameObj)
    {
        gameObj.SetActive(!gameObj.activeSelf);
    }

    public static void ToggleSprite(this Image img, Sprite sprite)
    {
        if (img.sprite == sprite)
            img.sprite = null;
        else
            img.sprite = sprite;
    }

    /**
 * CropTexture
 * 
 * Returns a new texture, composed of the specified cropped region.
 */
    public static Texture2D CropTexture(this Texture2D pSource, int left, int top, int width, int height)
    {
        if (left < 0)
        {
            width += left;
            left = 0;
        }
        if (top < 0)
        {
            height += top;
            top = 0;
        }
        if (left + width > pSource.width)
        {
            width = pSource.width - left;
        }
        if (top + height > pSource.height)
        {
            height = pSource.height - top;
        }

        if (width <= 0 || height <= 0)
        {
            return null;
        }

        Color[] aSourceColor = pSource.GetPixels(left, top, width, height); //.GetPixels(0);

        //*** Make New
        Texture2D oNewTex = new Texture2D(width, height, TextureFormat.ARGB32, false);

        //*** Make destination array
        int xLength = width * height;
        Color[] aColor = new Color[xLength];

        /*
        int i = 0;
        for (int y = 0; y < height; y++)
        {
            int sourceIndex = (y + top) * width + left;// pSource.width + left;
            for (int x = 0; x < width; x++)
            {
                aColor[i++] = aSourceColor[sourceIndex++];
            }
        }*/

        //*** Set Pixels
        oNewTex.SetPixels(aSourceColor);
        oNewTex.Apply();

        //*** Return
        return oNewTex;
    }

    /*
	public static bool HasFlag(this SpawnObjType enumVar, SpawnObjType enumVal)
	{
		return ((enumVar & enumVal) != 0);
	}
    */
}


