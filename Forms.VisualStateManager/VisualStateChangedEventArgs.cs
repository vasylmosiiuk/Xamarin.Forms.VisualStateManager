using System;
using Xamarin.Forms;

namespace Forms.VisualStateManager
{
    public sealed class VisualStateChangedEventArgs : EventArgs
    {
        public VisualStateChangedEventArgs(VisualState oldState, VisualState newState, VisualElement control)
        {
            OldState = oldState;
            NewState = newState;
            Element = control;
        }

        public VisualState OldState { get; }

        public VisualState NewState { get; }

        public VisualElement Element { get; }
    }
}