using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSytemData.Dto
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ResultDto<T> Success(T data, string message = "Success")
        {
            return new ResultDto<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ResultDto<T> Fail(string message)
        {
            return new ResultDto<T> { IsSuccess = false, Message = message };
        }

    }
}
