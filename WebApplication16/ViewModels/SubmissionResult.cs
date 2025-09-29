using System.Collections.Generic;

namespace WebApplication16.ViewModels
{
    /// <summary>
    /// مدل برای بازگشت نتیجه عملیات ثبت فرم.
    /// </summary>
    public class SubmissionResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}



//namespace WebApplication16.ViewModels
//{
//    public class SubmissionResult
//    {
//        public bool IsSuccess { get; private set; }
//        public List<string> Errors { get; set; } = new List<string>();

//        //public static SubmissionResult Success() => new() { IsSuccess = true };

//        public static SubmissionResult Failed(params string[] errors)
//        {
//            var result = new SubmissionResult { IsSuccess = false };
//            if (errors != null)
//            {
//                result.Errors.AddRange(errors);
//            }
//            return result;
//        }
//        public bool Success { get; set; }
//    }
//}

