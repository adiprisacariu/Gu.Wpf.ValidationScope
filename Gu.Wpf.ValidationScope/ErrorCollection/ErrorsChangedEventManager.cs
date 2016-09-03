﻿namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Windows;

    /// <summary>
    /// Manager for the ErrorCollection.Errors event.
    /// Inspired by: http://referencesource.microsoft.com/#WindowsBase/Base/System/Collections/Specialized/CollectionChangedEventManager.cs,7537b339109a7418
    /// </summary>
    internal class ErrorsChangedEventManager : WeakEventManager
    {
        private ErrorsChangedEventManager()
        {
        }

        // get the event manager for the current thread
        private static ErrorsChangedEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(ErrorsChangedEventManager);
                var manager = (ErrorsChangedEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new ErrorsChangedEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        /// <summary>Add a listener to the given source's event.</summary>
        public static void AddListener(ErrorCollection source, IWeakEventListener listener)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(listener, nameof(listener));
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>Remove a listener to the given source's event.</summary>
        public static void RemoveListener(ErrorCollection source, IWeakEventListener listener)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(listener, nameof(listener));
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <summary>Add a handler for the given source's event.</summary>
        public static void AddHandler(ErrorCollection source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(handler, nameof(handler));
            CurrentManager.ProtectedAddHandler(source, handler);
        }

        /// <summary>Remove a handler for the given source's event.</summary>
        public static void RemoveHandler(ErrorCollection source, EventHandler<ErrorsChangedEventArgs> handler)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(handler, nameof(handler));
            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        /// <summary>Return a new list to hold listeners to the event.</summary>
        protected override ListenerList NewListenerList()
        {
            return new ListenerList<ErrorsChangedEventArgs>();
        }

        /// <summary>Listen to the given source for the event.</summary>
        protected override void StartListening(object source)
        {
            ErrorCollection typedSource = (ErrorCollection)source;
            typedSource.ErrorsChanged += this.OnErrorsChanged;
        }

        /// <summary>Stop listening to the given source for the event.</summary>
        protected override void StopListening(object source)
        {
            ErrorCollection typedSource = (ErrorCollection)source;
            typedSource.ErrorsChanged -= this.OnErrorsChanged;
        }

        // event handler for CollectionChanged event
        private void OnErrorsChanged(object sender, ErrorsChangedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}