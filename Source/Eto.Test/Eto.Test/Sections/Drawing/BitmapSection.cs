using Eto.Forms;
using Eto.Drawing;
using ImageView = Eto.Test.Sections.Drawing.DrawableImageView;
using System;

namespace Eto.Test.Sections.Drawing
{
	/// <summary>
	/// We use this class instead of ImageView to test showing the image using the graphics context only
	/// </summary>
	public class DrawableImageView : Drawable
	{
		Image image;
		public Image Image
		{
			get { return image; }
			set
			{
				image = value;
				if (image != null)
					MinimumSize = image.Size;
				if (Loaded)
					Invalidate();
			}
		}

		public override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (Image != null)
				e.Graphics.DrawImage(Image, PointF.Empty);
		}

		public DrawableImageView()
		{
		}

		public DrawableImageView(Generator generator)
			: base(generator)
		{
		}
	}

	public class BitmapSection : Scrollable
	{
		public BitmapSection()
		{
			var layout = new DynamicLayout();

			layout.AddRow(new Label { Text = "Load from Stream" }, LoadFromStream());

			layout.AddRow(
				new Label { Text = "Custom 32-bit" }, CreateCustom32(),
				new Label { Text = "Custom 32-bit alpha" }, CreateCustom32Alpha(),
				null
			);

			layout.AddRow(
				new Label { Text = "Clone" }, Cloning(),
				new Label { Text = "Clone rectangle" }, TableLayout.AutoSized(CloningRectangle(), centered: true),
				null);

			layout.AddRow(
				new Label { Text = "Clone using tiles" }, TableLayout.AutoSized(CloneTiles(), centered: true),
				null);

			layout.Add(null);

			Content = layout;
		}

		Control LoadFromStream()
		{
			var image = TestIcons.TestImage();

			return new DrawableImageView { Image = image };
		}

		Control CreateCustom32()
		{
			var image = new Bitmap(100, 100, PixelFormat.Format32bppRgb);

			// should always ensure .Dispose() is called when you are done with a Graphics object
			using (var graphics = new Graphics(image))
			{
				graphics.DrawLine(Pens.Blue(), Point.Empty, new Point(image.Size));
				graphics.DrawRectangle(Pens.Blue(), new Rectangle(image.Size - 1));
			}

			return new DrawableImageView { Image = image };
		}

		Control CreateCustom32Alpha()
		{
			var image = new Bitmap(100, 100, PixelFormat.Format32bppRgba);

			// should always ensure .Dispose() is called when you are done with a Graphics object
			using (var graphics = new Graphics(image))
			{
				graphics.DrawLine(Pens.Blue(), Point.Empty, new Point(image.Size));
				graphics.DrawRectangle(Pens.Black(), new Rectangle(image.Size - 1));
			}
			return new DrawableImageView { Image = image };
		}

		Control Cloning()
		{
			var image = TestIcons.TestImage();
			image = image.Clone();
			return new DrawableImageView { Image = image };
		}

		Control CloningRectangle()
		{
			var image = TestIcons.TestImage();
			image = image.Clone(new Rectangle(32, 32, 64, 64));
			return new DrawableImageView { Image = image };
		}

		Control CloneTiles()
		{
			// Creates a duplicate of the bitmap by cloning tiles of it
			// and drawing them in the same location in the duplicate.
			var image = TestIcons.TestImage();
			var bitmap = new Bitmap(new Size(image.Size), PixelFormat.Format32bppRgba);
			var tile = 64; // the test image is 128x128 so this produces 4 tiles.
			using (var g = new Graphics(bitmap))
			{
				for (var x = 0; x < image.Width; x += tile)
					for (var y = 0; y < image.Height; y += tile)
					{
						var clone = image.Clone(new Rectangle(x, y, tile, tile));
						g.DrawImage(clone, x, y);
					}
			}
			return new DrawableImageView { Image = bitmap };
		}
	}
}