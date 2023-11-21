using UnityEngine;
using System.Collections;

namespace GrandDreams.Core
{
    public interface IObserver
    {

        string IDObserver { get; set; }

        void Notify(object data = null);

    }

    public interface ILanguageObserver : IObserver
    {

    }

    public interface ISubPanelObserver : IObserver
    {

    }

    public interface IRectTransformValueObserver : IObserver
    {

    }
}
