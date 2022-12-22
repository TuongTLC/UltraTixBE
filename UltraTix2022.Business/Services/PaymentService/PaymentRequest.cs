﻿
using System.Net;
using System.Text;

namespace UltraTix2022.API.UltraTix2022.Business.Services.PaymentService
{
    class PaymentRequest
    {
        public PaymentRequest()
        {
        }
        public static string sendPaymentRequest(string endpoint, string postJsonString)
        {

            try
            {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = "";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }


                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
