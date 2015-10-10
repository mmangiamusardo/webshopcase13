using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebShopCase.API.Service
{
    public static class Helper
    {
        public static string ToBase64(this byte[] picture)
        {
            if (picture!=null && picture.Length >= 78) 
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(picture, 78, picture.Length - 78); // strip out 78 byte OLE header (don't need to do this for normal images)
                    string imageBase64 = Convert.ToBase64String(ms.ToArray());
                    return string.Format("data:image/jpg;base64,{0}", imageBase64);
                }
            }
            return String.Empty;
            
        }
    }
}
