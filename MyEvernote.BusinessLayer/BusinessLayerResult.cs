using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class BusinessLayerResult<T> where T : class
    {
        public List<KeyValuePair<ErrorCode, string>> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors = new List<KeyValuePair<ErrorCode, string>>();
        }

        public void AddError(ErrorCode code, string message)
        {
            Errors.Add(new KeyValuePair<ErrorCode, string>(code, message));
        }
    }
}
