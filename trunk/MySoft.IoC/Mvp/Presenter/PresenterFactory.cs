using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using MySoft.IoC;
using MySoft.Core;

namespace MySoft.Mvp
{
    /// <summary>
    /// Presenter Factory
    /// </summary>
    public sealed class PresenterFactory
    {
        private CastleFactory container;

        private PresenterFactory()
        {
            container = CastleFactory.Create();
        }

        private static PresenterFactory singleton = null;

        /// <summary>
        /// Creates this singleton instance.
        /// </summary>
        /// <returns></returns>
        public static PresenterFactory Create()
        {
            if (singleton == null)
            {
                singleton = new PresenterFactory();
            }
            return singleton;
        }

        /// <summary>
        /// Gets the presenter.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        public IPresenterType GetPresenter<IPresenterType>(object view)
        {
            return GetPresenter<IPresenterType>(null, view);
        }

        /// <summary>
        /// Gets the presenter.
        /// </summary>
        /// <param name="presenterKey">The presenter key.</param>
        /// <param name="view">The view.</param>
        /// <returns></returns>
        public IPresenterType GetPresenter<IPresenterType>(string presenterKey, object view)
        {
            if (string.IsNullOrEmpty(presenterKey) ? container.ServiceContainer.Kernel.HasComponent(typeof(IPresenterType)) :
                container.ServiceContainer.Kernel.HasComponent(presenterKey))
            {
                IPresenterType _presenter = string.IsNullOrEmpty(presenterKey) ?
                    (IPresenterType)container.ServiceContainer.Kernel[typeof(IPresenterType)] :
                    (IPresenterType)container.ServiceContainer.Kernel[presenterKey];
                if (typeof(IPresenter).IsAssignableFrom(_presenter.GetType()))
                {
                    IPresenter presenter = (IPresenter)_presenter;
                    MethodInfo method = container.GetType().GetMethod("GetService", Type.EmptyTypes).MakeGenericMethod(presenter.TypeOfModel);
                    object model = DynamicCalls.GetMethodInvoker(method).Invoke(container, null);
                    presenter.BindView(view);
                    presenter.BindModel(model);
                    return _presenter;
                }
                else if (typeof(IPresenter2).IsAssignableFrom(_presenter.GetType()))
                {
                    IPresenter2 presenter = (IPresenter2)_presenter;
                    object[] models = new object[presenter.TypeOfModels.Length];
                    for (int i = 0; i < models.Length; i++)
                    {
                        MethodInfo method = container.GetType().GetMethod("GetService", Type.EmptyTypes).MakeGenericMethod(presenter.TypeOfModels[i]);
                        models[i] = DynamicCalls.GetMethodInvoker(method).Invoke(container, null);
                    }
                    presenter.BindView(view);
                    presenter.BindModels(models);
                    return _presenter;
                }
            }

            return default(IPresenterType);
        }

    }
}
