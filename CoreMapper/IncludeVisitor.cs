using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CoreMapper
{
    internal sealed class IncludeVisitor : ExpressionVisitor
    {
        private readonly List<IEnumerable<string>> _includes = new List<IEnumerable<string>>();
        private IEnumerable<string> _thenIncludes;

        internal IEnumerable<IncludePath> GetIncludes(IQueryable query)
        {
            _includes.Clear();
            Visit(query.Expression);

            return Helper.ParseIncludes(_includes);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "IncludeSpan")
            {
                foreach (var arg in node.Arguments)
                {
                    var spanList =
                        (IEnumerable<object>)Helper.GetPropertyValue(Helper.GetPropertyValue(arg, "Value"),
                            "SpanList");
                    foreach (var span in spanList)
                    {
                        var navs = (IEnumerable<string>)Helper.GetFieldValue(span, "Navigations");
                        _includes.Add(navs);
                    }
                }
            }
            else if (node.Method.Name == "Include")
            {
                var path = Helper.GetMemberPath(node.Arguments[1]);
                if (_thenIncludes != null)
                {
                    path.AddRange(_thenIncludes);
                    _thenIncludes = null;
                }
                _includes.Add(path);
            }
            if (node.Method.Name == "ThenInclude")
            {
                var path = Helper.GetMemberPath(node.Arguments[1]);
                if (_thenIncludes == null)
                {
                    _thenIncludes = path;
                }
                else
                {
                    _thenIncludes = path.Concat(_thenIncludes);
                }
                    
            }

            return base.VisitMethodCall(node);
        }
    }
}
