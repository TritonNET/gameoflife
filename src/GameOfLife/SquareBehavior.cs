namespace GameOfLife
{
    public sealed class SquareBehavior : Behavior<VisualElement>
    {
        protected override void OnAttachedTo(VisualElement v)
        {
            base.OnAttachedTo(v);
            v.SizeChanged += OnSizeChanged;
        }
        protected override void OnDetachingFrom(VisualElement v)
        {
            v.SizeChanged -= OnSizeChanged;
            base.OnDetachingFrom(v);
        }
        static void OnSizeChanged(object? sender, EventArgs e)
        {
            if (sender is VisualElement v && v.Width > 0)
            {
                // make height exactly equal to the current width -> perfect square
                v.HeightRequest = v.Width;
            }
        }
    }
}
