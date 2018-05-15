using System.Linq.Expressions;

namespace CoreMapper
{
    public class ExpressionProvider : IExpressionProvider
    {
        public MemberBinding CreateMemberBinding(MapperMember outMember, MapperMember inMember, ParameterExpression inObjPrm,
            ParameterExpression mapContextPrm)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IExpressionProvider
    {
        MemberBinding CreateMemberBinding(MapperMember outMember, MapperMember inMember, ParameterExpression inObjPrm,
            ParameterExpression mapContextPrm);
    }
}