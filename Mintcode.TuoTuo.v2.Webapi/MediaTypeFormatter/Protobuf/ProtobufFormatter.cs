using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.TuoTuo.v2.Webapi
{
    public class ProtobufFormatter : MediaTypeFormatter
    {
        private static readonly MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("application/x-protobuf");

        public ProtobufFormatter()
        {
            SupportedMediaTypes.Add(mediaType);
        }

        public override bool CanReadType(Type type)
        {
            return typeof(IMessage).IsAssignableFrom(type);
        }

        public override bool CanWriteType(Type type)
        {
            return typeof(IMessage).IsAssignableFrom(type);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            #region 老方法

            //var obj = (IMessage)Activator.CreateInstance(type);
            //Byte[] bytes = new Byte[readStream.Length];
            //return readStream.ReadAsync(bytes, 0, bytes.Length)
            //    .ContinueWith(s =>
            //    {
            //        obj.MergeFrom(bytes);
            //        return (object)obj;
            //    });


            #endregion


            var tcs = new TaskCompletionSource<object>();
            Byte[] bytes = new Byte[readStream.Length];
            readStream.ReadAsync(bytes, 0, bytes.Length)
                .ContinueWith(s =>
                {
                    try
                    {
                        var obj = (IMessage)Activator.CreateInstance(type);
                        obj.MergeFrom(bytes);
                        tcs.SetResult(obj);
                    }
                    catch (Exception ex)
                    {
                        tcs.SetException(ex);
                    }

                });
            return tcs.Task;
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {

            var tcs = new TaskCompletionSource<object>();
            var obj = (IMessage)value;
            Byte[] bytes = obj.ToByteArray();
            writeStream.WriteAsync(bytes, 0, bytes.Length).ContinueWith(s =>
            {
                try
                {
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

            });
            return tcs.Task;
        }

    }
}
