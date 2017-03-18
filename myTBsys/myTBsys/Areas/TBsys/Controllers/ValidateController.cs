using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace myTBsys.Areas.TBsys.Controllers
{
    public class ValidateController : Controller
    {
        // GET: TBsys/Validate
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            Other.ValidateCode vCode = new Other.ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;//关键点
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}