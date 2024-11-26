using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Datamodels.Utils
{
    internal class ParameterReplaceVisitor : ExpressionVisitor
    {
        public ParameterExpression Target { get; set; }

        public ParameterExpression Replacement { get; set; }

        protected override Expression VisitParameter(ParameterExpression node) => node == Target ? Replacement : base.VisitParameter(node);
    }
}
