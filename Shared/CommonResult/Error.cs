using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CommonResult
{
    public class Error
    {
        public string Code { get;  }

        public string Description { get;  }

        public  ErrorType Type { get;   }
        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }
         
        #region  Static Factory Method

        public static Error Failure(string code = "General. Failure", string description = "حدث خطأ غير متوقع، حاول مرة أخرى لاحقًا")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "General .Validation", string description = "يرجى التأكد من صحة البيانات المدخلة")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error NotFound(string code = "General .NotFound", string description = "العنصر المطلوب غير موجود")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "General .Unauthorized", string description = "يجب تسجيل الدخول أولاً")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
        public static Error Forbidden(string code = "General .Forbidden", string description = "ليس لديك صلاحية لتنفيذ هذا الإجراء")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCrendentials(string code = "General .InvalidCrendentails", string description = "بيانات تسجيل الدخول غير صحيحة")
        {
            return new Error(code, description, ErrorType.InvalidCrendentials);
        }
        #endregion
    }
}
