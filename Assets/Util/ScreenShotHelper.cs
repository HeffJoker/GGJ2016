using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SF_Tools.Util
{
    public class ScreenShotHelper : MonoBehaviour
    {
        #region Editor Properties

        public GameObject[] ObjsToCapture;
        public Camera CameraToUse;
        public string OutputFolder = string.Empty;

        #endregion

        #region Private Members

        #endregion

        #region Public Interface

        public void DoScreenShots()
        {
            StartCoroutine(DoScreenshots_Coroutine());
        }

        private IEnumerator DoScreenshots_Coroutine()
        {
            for (int i = 0; i < ObjsToCapture.Length; ++i)
                ObjsToCapture[i].SetActive(false);

            for (int i = 0; i < ObjsToCapture.Length; ++i)
            {
                GameObject currObj = ObjsToCapture[i];
                currObj.SetActive(true);

                if (CameraToUse != null)
                {
                    //CameraToUse.transform.position = currObj.transform.position;
                    CameraToUse.transform.position = new Vector3(currObj.transform.position.x, currObj.transform.position.y, CameraToUse.transform.position.z);

                    TakeScreenShot(currObj.name);

                    yield return null;
                }

                currObj.SetActive(false);
            }

            for (int i = 0; i < ObjsToCapture.Length; ++i)
                ObjsToCapture[i].SetActive(true);
        }

        public void TakeScreenShot(string filename)
        {
            string output = OutputFolder + filename + ".png";
            Application.CaptureScreenshot(output);

            return;

            int width = Screen.width;
            int height = Screen.height;

            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            UnityEngine.Object.DestroyImmediate(tex);


            if (File.Exists(output))
                File.Delete(output);

            File.WriteAllBytes(output, bytes);
        }

        #endregion

    }
}
