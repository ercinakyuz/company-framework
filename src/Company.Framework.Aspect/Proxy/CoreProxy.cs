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
            var cancellationToken = CancellationToken.None;
            ExecutePreProcessorsAsync(args?.FirstOrDefault(), cancellationToken).Wait(cancellationToken);
            var result = Next(targetMethod, args, cancellationToken).Result;
            ExecutePostProcessorsAsync(args?.FirstOrDefault(), result, cancellationToken).Wait(cancellationToken);
            return result;
        }

        private async Task<object?> Next(MethodInfo? targetMethod, object?[]? args, CancellationToken cancellationToken)
        {
            var targetMethodResult = targetMethod?.Invoke(_decoration, args);

            object? result;
            if (targetMethodResult is Task task)
            {
                await task;
                try
                {
                    result = ((dynamic)task).Result;
                }
                catch
                {
                    result = Task.CompletedTask;
                }
            }
            else
            {
                result = targetMethodResult!;
            }
            return result;
        }
        private async Task ExecutePreProcessorsAsync(object? args, CancellationToken cancellationToken)
        {
            foreach (var preProcessor in _preProcessors)
            {
                await preProcessor.ExecuteAsync(args, cancellationToken);
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