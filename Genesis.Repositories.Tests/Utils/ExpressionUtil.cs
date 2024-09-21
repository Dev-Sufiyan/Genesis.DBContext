using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Genesis.Repositories.Tests.Utils
{
    public static class ExpressionUtil
    {
        public static bool IsEqual(this Expression expr1, Expression expr2)
        {
            if (expr1 == null || expr2 == null) return false;
            return expr1.ToString() == expr2.ToString();
        }
    }
}
