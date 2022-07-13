using System.Reflection;
using Company.Framework.Aspect.Processors;

namespace Company.Framework.Aspect.Proxy
{
    public class CoreProxy<TDecoration> : DispatchProxy
    {
        private TDecoration? _decoration;

        private ISet<IPreProcessor>? _preProcessors;

        private ISet<IPostProcessor>? _postProcessors;

        public void SetDecoration(TDecoration decoration)
        {
            _decoration = decoration;
        }
        public void SetPreProcessors<TPreProcessor>(IEnumerable<TPreProcessor> preProcessors) where TPreProcessor : IPreProcessor
        {
            _preProcessors = preProcessors.Select(processor => (IPreProcessor)processor).ToHashSet();
        }
        public void SetPostProcessors<TPostProcessor>(IEnumerable<TPostProcessor> postProcessors) where TPostProcessor : IPostProcessor
        {
            _postProcessors = postProcessors.Select(processor => (IPostProcessor)processor).ToHashSet();
        }
        protected sealed override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            ExecutePreProcessorsAsync(args?.FirstOrDefault(), CancellationToken.None).Wait();
            var result = targetMethod?.Invoke(_decoration, args);
            ExecutePostProcessorsAsync(args?.FirstOrDefault(), result, CancellationToken.None).Wait();
            return result;
        }

        private async Task ExecutePreProcessorsAsync(object? args, CancellationToken cancellationToken)
        {
            foreach (var preProcessor in _preProcessors)
            {
                await preProcessor.ProcessAsync(args, cancellationToken);
            }
        }

        private async Task ExecutePostProcessorsAsync(object? args, object? result, CancellationToken cancellationToken)
        {
            foreach (var postProcessor in _postProcessors)
            {
                await postProcessor.ExecuteAsync(args, result, cancellationToken);
            }
        }
    }
}