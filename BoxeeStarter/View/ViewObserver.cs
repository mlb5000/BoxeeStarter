using BoxeeStarter.Model;
using BoxeeStarter.Presenter;

namespace BoxeeStarter.View
{
    public class ViewObserver
    {
        public static void Observe(ISettingsView view, IPortListener listener)
        {
            var presenter = new SettingsPresenter(view, listener);
            presenter.Initialize();
        }
    }
}