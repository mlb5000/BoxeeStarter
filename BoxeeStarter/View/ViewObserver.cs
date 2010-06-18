using BoxeeStarter.Model;
using BoxeeStarter.Presenter;
using BoxeeStarter.Utilities.Registry;

namespace BoxeeStarter.View
{
    public class ViewObserver
    {
        public static void Observe(ISettingsView view, IPortListener listener)
        {
            var presenter = new SettingsPresenter(view, listener);
            presenter.RegistryHelper = new RegistryHelper(new WinRegistry());
            presenter.Initialize();
        }
    }
}