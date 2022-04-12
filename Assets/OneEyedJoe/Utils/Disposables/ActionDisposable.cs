using System;

namespace OneEyedJoe.Utils.Disposables
{
    public class ActionDisposable : IDisposable
    {
        private Action _osDispose;

        public ActionDisposable(Action onDispose)
        {
            _osDispose = onDispose;
        }
        
        public void Dispose()
        {
            _osDispose?.Invoke();
            _osDispose = null;
        }
    }
}