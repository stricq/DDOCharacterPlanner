using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace DdoCharacterPlanner.Controls
{

  public class Bitmap : FrameworkElement {

    #region Private Fields

    private Point pixelOffset;

    private readonly EventHandler                     sourceDownloaded;
    private readonly EventHandler<ExceptionEventArgs> sourceFailed;

    #endregion Private Fields

    #region Public Events

    public event EventHandler<ExceptionEventArgs> BitmapFailed;

    #endregion Public Events

    #region Constructor

    public Bitmap() {
      sourceDownloaded = onSourceDownloaded;
      sourceFailed     = onSourceFailed;

      LayoutUpdated   += onLayoutUpdated;
    }

    #endregion Constructor

    #region Dependency Properties

    #region Source Property

    public BitmapSource Source {
      get => (BitmapSource)GetValue(SourceProperty);
      set => SetValue(SourceProperty, value);
    }

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(BitmapSource), typeof(Bitmap), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure, onSourceChanged));

    private static void onSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
      if (!(d is Bitmap bitmap)) return;

      if (e.OldValue is BitmapSource oldValue && bitmap.sourceDownloaded != null && !oldValue.IsFrozen) {
        oldValue.DownloadCompleted -= bitmap.sourceDownloaded;
        oldValue.DownloadFailed -= bitmap.sourceFailed;
      }

      if (e.NewValue is BitmapSource newValue && !newValue.IsFrozen) {
        newValue.DownloadCompleted += bitmap.sourceDownloaded;
        newValue.DownloadFailed += bitmap.sourceFailed;
      }
    }

    #endregion Source Property

    #endregion Dependency Properties

    #region Event Handlers

    private void onSourceDownloaded(object sender, EventArgs e) {
      InvalidateMeasure();
      InvalidateVisual();
    }

    private void onSourceFailed(object sender, ExceptionEventArgs e) {
      Source = null; // setting a local value seems sketchy...

      BitmapFailed?.Invoke(this, e);
    }

    private void onLayoutUpdated(object sender, EventArgs e) {
      //
      // This event just means that layout happened somewhere.  However, this is
      // what we need since layout anywhere could affect our pixel positioning.
      //
      Point offset = getPixelOffset();

      if (!areClose(offset, pixelOffset)) {
        InvalidateVisual();
      }
    }

    #endregion Event Handlers

    #region Overrides

    protected override Size MeasureOverride(Size availableSize) {
      Size measureSize = new Size();

      BitmapSource bitmapSource = Source;

      if (bitmapSource == null) return measureSize;

      PresentationSource ps = PresentationSource.FromVisual(this);

      if (ps == null) return measureSize;
      //
      // ReSharper disable once PossibleNullReferenceException
      //
      Matrix fromDevice = ps.CompositionTarget.TransformFromDevice;

      Vector pixelSize = new Vector(bitmapSource.PixelWidth, bitmapSource.PixelHeight);

      Vector measureSizeV = fromDevice.Transform(pixelSize);

      measureSize = new Size(measureSizeV.X, measureSizeV.Y);

      return measureSize;
    }

    protected override void OnRender(DrawingContext dc) {
      BitmapSource bitmapSource = Source;

      if (bitmapSource == null) return;

      pixelOffset = getPixelOffset();
      //
      // Render the bitmap offset by the needed amount to align to pixels.
      //
      Size desiredSize = new Size(DesiredSize.Width - Margin.Left - Margin.Right, DesiredSize.Height - Margin.Top - Margin.Bottom);

      dc.DrawImage(bitmapSource, new Rect(pixelOffset, desiredSize));
    }

    #endregion Overrides

    #region Private Methods

    private Point getPixelOffset() {
      Point offset = new Point();

      PresentationSource ps = PresentationSource.FromVisual(this);

      if (ps == null) return offset;

      Visual rootVisual = ps.RootVisual;
      //
      // Transform (0,0) from this element up to pixels.
      //
      offset = TransformToAncestor(rootVisual).Transform(offset);
      offset = applyVisualTransform(offset, rootVisual, false);

      // ReSharper disable once PossibleNullReferenceException
      offset = ps.CompositionTarget.TransformToDevice.Transform(offset);
      //
      // Round the origin to the nearest whole pixel.
      //
      offset.X = Math.Round(offset.X);
      offset.Y = Math.Round(offset.Y);
      //
      // Transform the whole-pixel back to this element.
      //
      offset = ps.CompositionTarget.TransformFromDevice.Transform(offset);
      offset = applyVisualTransform(offset, rootVisual, true);
      offset = rootVisual.TransformToDescendant(this).Transform(offset);

      return offset;
    }

    private static Point applyVisualTransform(Point point, Visual v, bool inverse) {
      return tryApplyVisualTransform(point, v, inverse, true, out bool _);
    }

    private static Point tryApplyVisualTransform(Point point, Visual v, bool inverse, bool throwOnError, out bool success) {
      success = true;

      if (v == null) return point;

      Matrix visualTransform = getVisualTransform(v);

      if (inverse) {
        if (!throwOnError && !visualTransform.HasInverse) {
          success = false;

          return new Point(0, 0);
        }

        visualTransform.Invert();
      }

      point = visualTransform.Transform(point);

      return point;
    }

    private static Matrix getVisualTransform(Visual v) {
      if (v == null) return Matrix.Identity;

      Matrix m = Matrix.Identity;

      Transform transform = VisualTreeHelper.GetTransform(v);

      if (transform != null) {
        Matrix cm = transform.Value;

        m = Matrix.Multiply(m, cm);
      }

      Vector offset = VisualTreeHelper.GetOffset(v);

      m.Translate(offset.X, offset.Y);

      return m;
    }

    private static bool areClose(Point point1, Point point2) {
      return areClose(point1.X, point2.X) && areClose(point1.Y, point2.Y);
    }

    private static bool areClose(double value1, double value2) {
      double delta = value1 - value2;

      return (delta < 1.53E-06) && (delta > -1.53E-06);
    }

    #endregion Private Methods

  }

}
