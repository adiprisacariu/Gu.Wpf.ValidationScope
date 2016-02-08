namespace Gu.Wpf.ValidationScope
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using JetBrains.Annotations;

    public abstract class Node : IErrorNode
    {
        private readonly Lazy<ObservableCollection<IErrorNode>> innerChildren = new Lazy<ObservableCollection<IErrorNode>>(() => new ObservableCollection<IErrorNode>());
        private ReadOnlyObservableCollection<IErrorNode> children;
        private bool hasErrors = true;

        protected Node(bool hasErrors)
        {
            this.hasErrors = hasErrors;
        }

        protected Node(IErrorNode child)
        {
            this.EditableChildren.Add(child);
            this.hasErrors = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyObservableCollection<IErrorNode> Children => this.children ?? (this.children = new ReadOnlyObservableCollection<IErrorNode>(this.EditableChildren));

        public virtual bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }

            protected set
            {
                if (value == this.hasErrors)
                {
                    return;
                }

                this.hasErrors = value;
                this.OnPropertyChanged();
                this.OnHasErrorsChanged();
            }
        }

        public abstract DependencyObject Source { get; }

        protected ObservableCollection<IErrorNode> EditableChildren => this.innerChildren.Value;

        protected internal abstract void RemoveChild(IErrorNode errorNode);

        protected internal virtual void AddChild(IErrorNode errorNode)
        {
            if (this.EditableChildren.Contains(errorNode))
            {
                return;
            }

            this.EditableChildren.Add(errorNode);
            this.HasErrors = true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected abstract void OnHasErrorsChanged();
    }
}