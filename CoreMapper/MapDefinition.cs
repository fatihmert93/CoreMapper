using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CoreMapper
{
    public sealed class MapDefinition<TIn,TOut> : IMapDefinition<TIn,TOut>
    {
        private readonly Lazy<Func<TIn, MapContext, TOut>> _lazyMapperWithCache;
        private readonly Lazy<Func<TIn, TOut, MapContext, TOut>> _lazyPopulator;

        public MapDefinition(Expression<Func<TIn,MapContext,TOut>> projector)
        {
            InType = typeof(TIn);
            Projector = projector;
            
            _lazyMapperWithCache = new Lazy<Func<TIn, MapContext, TOut>>();
        }


        public Type InType { get; }
        public Expression<Func<TIn, MapContext, TOut>> Projector { get; }
        public Func<TIn, MapContext, TOut> Mapper { get; }
        public Func<TIn, MapContext, TOut> MapperWithCache { get; }
        public Func<TIn, TOut, MapContext, TOut> Populator { get; }

        LambdaExpression IMapDefinition.Projector => Projector;

        Delegate IMapDefinition.Mapper => Mapper;

        Delegate IMapDefinition.MapperWithCache => MapperWithCache;
    }

    public interface IMapDefinition<TIn, TOut> : IMapDefinition
    {
        new Expression<Func<TIn,MapContext,TOut>> Projector { get; }
        new Func<TIn, MapContext,TOut> Mapper { get; }
        new Func<TIn,MapContext, TOut> MapperWithCache { get; }
        Func<TIn,TOut,MapContext,TOut> Populator { get; }
    }

    public interface IMapDefinition
    {
        Type InType { get; }
        LambdaExpression Projector { get; }
        Delegate Mapper { get; }
        Delegate MapperWithCache { get; }
    }
}
