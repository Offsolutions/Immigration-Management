using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Security.Cryptography;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using Consoller.Areas.Auth.Models;

namespace onlineportal.Areas.AdminPanel.Models
{
    public class Helper
    {
        dbcontext db = new dbcontext();
        public string _FileName;
        const string passphrase = "password";
        public string uploadfile(HttpPostedFileBase file)
        {
            try
            {
                if (file!=null)
                {
                    _FileName = Path.GetFileName(file.FileName);

                    _FileName = Guid.NewGuid().ToString().Substring(0, 4) + _FileName;
                    string _path = (HttpContext.Current.Server.MapPath("/UploadedFiles/" + _FileName));

                    file.SaveAs(_path);
                    return _FileName;
                }
                else
                {
                    return "False";
                }

               
            }
            catch (Exception e)
            {
                return "False";
            }
        }
       

        public string EncryptData(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();

                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public string DecryptData(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
        public string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
        public string SendSMS(string User, string sender, string to, string message,string pass)
        {
            string stringpost = "uname=" + User + "&pass=" + pass + "&send=" + sender + "&dest=" + to + "&msg=" + message;
            //Response.Write(stringpost)
            string functionReturnValue = "";
           // functionReturnValue = "";

            HttpWebRequest objWebRequest = null;
            HttpWebResponse objWebResponse = null;
            StreamWriter objStreamWriter = null;
            StreamReader objStreamReader = null;

            try
            {
                string stringResult = null;

                objWebRequest = (HttpWebRequest)WebRequest.Create("http://103.247.98.91/API/SendMsg.aspx?");
                //domain name: Domain name Replace With Your Domain  
                objWebRequest.Method = "Post";

                // Response.Write(objWebRequest)

                // Use below code if you want to SETUP PROXY.
                //Parameters to pass: 1. ProxyAddress 2. Port
                //You can find both the parameters in Connection settings of your internet explorer.


                // If You are In the proxy Then You Uncomment the below lines and Enter IP And Port Number


                //System.Net.WebProxy myProxy = new System.Net.WebProxy("192.168.1.108", 6666);
                //myProxy.BypassProxyOnLocal = true;
                //objWebRequest.Proxy = myProxy;

                objWebRequest.ContentType = "application/x-www-form-urlencoded";

                objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());
                objStreamWriter.Write(stringpost);
                objStreamWriter.Flush();
                objStreamWriter.Close();

                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();


                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();

                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                stringResult = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                return (stringResult);
            }
            catch (Exception ex)
            {
                return (ex.ToString());

            }
            finally
            {
                if ((objStreamWriter != null))
                {
                    objStreamWriter.Close();
                }
                if ((objStreamReader != null))
                {
                    objStreamReader.Close();
                }
                objWebRequest = null;
                objWebResponse = null;

            }
        }

        //public string smssetting(string mobile, string Message)
        //{
        //    var setting = db.EmailSettings.ToList();
        //    if (setting != null)
        //    {
                
        //           // SendSMS(setting[0].SmsUser, setting[0].password, mobile, Message, "Trans", setting[0].Api);
                
        //        return ("Done");
        //    }
        //    return ("Done");

        //}
        public string PopulateBody(string userName, string title, string url, string description)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/App_Data/Template/ConfirmEmail.txt")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{Description}", description);
            return body;
        }
        //public void SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        //{
        //    dbcontext db = new dbcontext();
        //    var office = db.EmailSettings.ToList();

        //    using (MailMessage mailMessage = new MailMessage(office[0].Email, recepientEmail))
        //    {

        //        mailMessage.Subject = subject;
        //        mailMessage.Body = body;
        //        mailMessage.IsBodyHtml = true;

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        NetworkCredential NetworkCred = new NetworkCredential(office[0].Email, office[0].Password);
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = NetworkCred;
        //        smtp.Port = 587;
        //        smtp.Send(mailMessage);
        //    }
        //}
        #region Resize Image
        private static Bitmap ResizeImage(System.Drawing.Image imgPhoto, int Height, int Width)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)Math.Ceiling(sourceWidth * nPercent);
            int destHeight = (int)Math.Ceiling(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, imgPhoto.PixelFormat);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.White);

            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            grPhoto.DrawImage(imgPhoto, rect, new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            grPhoto.Dispose();
            return bmPhoto;
        }
        #endregion
        public string Resize(HttpPostedFileBase file,int height,int width)
        {
            try
            {
                if (file != null)
                {
                    _FileName = Path.GetFileName(file.FileName);

                    _FileName = Guid.NewGuid().ToString().Substring(0, 4) + _FileName;
                    string _path = (HttpContext.Current.Server.MapPath("/UploadedFiles/" + _FileName));
                    
                    Stream Buffer2 = file.InputStream;
                    System.Drawing.Image Image2 = System.Drawing.Image.FromStream(Buffer2);
                    Bitmap bmp2 = ResizeImage(Image2, height ,width);
                    bmp2.Save(_path, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return _FileName;
                }
                else
                {
                    return "False";
                }


            }
            catch (Exception e)
            {
                return "False";
            }
        }
        public string Permission()
        {
            return HttpContext.Current.User.IsInRole("Receptionist") ? Receptionist() : HttpContext.Current.User.IsInRole("Franchisee")?Franchisee() :"Admin"  ;
        }
        //public TimeSpan time()
        //{
        //    string franchid = Permission();
        //    tblreceptionist rr = db.tblreceptionists.FirstOrDefault(x => x.rid == franchid);
        //    return rr.
        //}
        public string RoleName()
        {
            var name = (HttpContext.Current.User.Identity.IsAuthenticated? HttpContext.Current.User.Identity.Name : "Guest");
                    tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == name).FirstOrDefault();
            return rr.name;
    
        }
        public string apicall(string url)
        {
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();
            StreamReader sr = new StreamReader(httpres.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();
            return results;
        }
        public string Franchisee()
        {
            return HttpContext.Current.User.Identity.Name;
        }
        public string contact(int app)
        {
            var inquiryid = db.Applications.FirstOrDefault(x => x.ApplicationNo == app).InquiryId;
            online onlines = db.onlines.FirstOrDefault(x => x.inquiryid == inquiryid);
            return onlines.Mobile;

        }
        public string Receptionist()
        {
            string a= HttpContext.Current.User.Identity.Name;
            tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == a).First();
            return rr.franchid.ToString();
        }
        public string Teacher()
        {
            var name = (HttpContext.Current.User.Identity.IsAuthenticated ? HttpContext.Current.User.Identity.Name : "Guest");
            tblreceptionist rr = db.tblreceptionists.Where(x => x.rid == name).FirstOrDefault();
            return rr.rid;
        }
        public string ConvertNumbertoWords(int number)
        {
            if (number == 0)
                return "ZERO";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "AND ";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public void sendsms(string phone,string Message)
        {
            tblsms sms = new tblsms();
            sms = db.tblsms.FirstOrDefault();
            if (sms != null)
            {
                if (phone != null)
                {
                   
                    string msg = Message;
                    string result = apicall("http://sms.sms.officialsolutions.in/sendSMS?username=" + sms.Username + "&message=" + msg + "&sendername=" + sms.Senderid + "&smstype=TRANS&numbers=" +phone + "&apikey=" + sms.Api + "");
                }
            }
        }
        public Boolean Checklock()
        {
            SQLHelper objsql = new SQLHelper();
            string time24 = DateTime.Now.AddHours(5).AddMinutes(33).ToString("HH:mm:ss");
            DataTable dt = objsql.GetTable("select * from tblreceptionists where starttime<='" + time24 + "' and endtime>='" + time24 + "' and rid='" + Permission() + "'");
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
    public enum NotificationEnumeration
    {
        Success,
        Error,
        Warning
    }


}
