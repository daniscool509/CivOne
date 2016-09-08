// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System;
using System.Drawing;
using System.Linq;
using CivOne.Advances;
using CivOne.Enums;
using CivOne.Events;
using CivOne.Interfaces;
using CivOne.IO;
using CivOne.GFX;
using CivOne.Templates;

namespace CivOne.Screens
{
	internal class Newspaper : BaseScreen
	{
		private bool _update = true;

		public event EventHandler Closed;

		public void Close()
		{
			if (Closed != null)
			{
				Closed(this, null);
			}
			Destroy();
		}
		
		public override bool HasUpdate(uint gameTick)
		{
			if (_update)
			{
				_update = false;
			}
			return true;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			Destroy();
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			Destroy();
			return true;
		}

		public Newspaper(params string[] message)
		{
			Cursor = MouseCursor.None;

			bool modernGovernment = Game.Instance.HumanPlayer.Advances.Any(a => a.Id == (int)Advance.Invention);
			Bitmap governmentPortrait = Icons.GovernmentPortrait(Government.Despotism, Advisor.Science, modernGovernment);
			Color[] palette = Resources.Instance.LoadPIC("SP257").Image.Palette.Entries;
			for (int i = 144; i < 256; i++)
			{
				palette[i] = governmentPortrait.Palette.Entries[i];
			}
			
			string newsflash = TextFile.Instance.GetGameText($"KING/NEWS{(char)Common.Random.Next((int)'A', (int)'O')}")[0];
			string shout = (Common.Random.Next(0, 2) == 0) ? "FLASH" : "EXTRA!";
			string date = $"January 1, {Common.YearString(Game.Instance.GameTurn)}";
			string name = "NONE";
			if (Game.Instance.HumanPlayer.Cities.Length > 0)
				name = Game.Instance.HumanPlayer.Cities[0].Name;
			switch (Common.Random.Next(0, 3))
			{
				case 0: name = $"The {name} Times"; break;
				case 1: name = $"The {name} Tribune"; break;
				case 2: name = $"{name} Weekly"; break;
			}

			_canvas = new Picture(320, 200, palette);
			_canvas.FillRectangle(15, 0, 0, 320, 100);
			for (int xx = 0; xx < _canvas.Image.Width; xx += Icons.Newspaper.Width)
			{
				AddLayer(Icons.Newspaper, xx, 100);
			}
			_canvas.DrawText("FLASH", 2, 5, 6, 3);
			_canvas.DrawText("FLASH", 2, 5, 272, 3);
			_canvas.DrawText(newsflash, 1, 5, 158, 3, TextAlign.Center);
			_canvas.DrawText(newsflash, 1, 5, 158, 3, TextAlign.Center);
			_canvas.DrawText(",-.", 4, 5, 8, 10);
			_canvas.DrawText(",-.", 4, 5, 268, 10);
			_canvas.DrawText(name, 4, 5, 158, 11, TextAlign.Center);
			_canvas.DrawText(date, 0, 5, 8, 28);
			_canvas.DrawText("10 cents", 0, 5, 272, 28);
			_canvas.FillRectangle(5, 1, 1, 318, 1);
			_canvas.FillRectangle(5, 1, 2, 1, 33);
			_canvas.FillRectangle(5, 318, 2, 1, 33);
			_canvas.FillRectangle(5, 0, 35, 320, 1);
			_canvas.FillRectangle(5, 0, 97, 320, 1);

			for (int i = 0; i < message.Length; i++)
			{
				_canvas.DrawText(message[i], 3, 5, 16, 40 + (i * 17));
			}
			
			Bitmap background = Resources.Instance.GetPart("SP299", 288, 120, 32, 16);
			Picture dialog = new Picture(152, 15);
			dialog.FillLayerTile(background);
			dialog.FillRectangle(0, 151, 0, 1, 15);
			dialog.AddBorder(15, 8, 0, 0, 151, 15);
			dialog.DrawText("Press any key to continue.", 0, 15, 4, 4);
			_canvas.FillRectangle(5, 80, 128, 153, 17);
			AddLayer(dialog, 81, 129);
		}
	}
}