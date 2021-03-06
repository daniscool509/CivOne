// CivOne
//
// To the extent possible under law, the person who associated CC0 with
// CivOne has waived all copyright and related or neighboring rights
// to CivOne.
//
// You should have received a copy of the CC0 legalcode along with this
// work. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.

using System.Linq;
using CivOne.Events;
using CivOne.Graphics;
using CivOne.Wonders;

namespace CivOne.Screens.Reports
{
	[Modal]
	internal class WorldWonders : BaseScreen
	{
		private struct CityWonders
		{
			public City City { get; set; }
			public IWonder Wonder { get; set; }
		}

		private bool _update = true;

		private int _page = 0;

		private readonly CityWonders[] _wonders;
		
		protected override bool HasUpdate(uint gameTick)
		{
			if (!_update) return false;

			this.FillRectangle(8, 32, 304, 160, 3);

			for (int i = (_page * 7); i < _wonders.Length && i < ((_page + 1) * 7); i++)
			{
				IWonder wonder = _wonders[i].Wonder;
				City city = _wonders[i].City;

				int xx = 8;
				int yy = 32 + (24 * (i % 7));
				int ww = 304;
				int hh = 16;

				byte colour = 12;
				if (city != null && city.Size > 0)
					colour = Common.ColourLight[city.Owner];
				this.FillRectangle(xx, yy, ww, hh, colour)
					.FillRectangle(xx + 1, yy + 1, ww - 2, hh - 2, 3)
					.AddLayer(wonder.SmallIcon, xx + 8, yy + 3)
					.DrawText(wonder.FormatWorldWonder(city), 0, 15, xx + 32, yy + 5);
			}

			_update = false;
			return true;
		}
		
		public override bool KeyDown(KeyboardEventArgs args)
		{
			if ((++_page * 7) > _wonders.Length)
				Destroy();
			else
				_update = true;
			return true;
		}
		
		public override bool MouseDown(ScreenEventArgs args)
		{
			if ((++_page * 7) > _wonders.Length)
				Destroy();
			else
				_update = true;
			return true;
		}
		
		public WorldWonders()
		{
			Palette = Common.DefaultPalette;

			_wonders = Game.BuiltWonders.OrderBy(w => w.Id).Select(w => new CityWonders()
			{
				Wonder = w,
				City = Game.GetCities().First(c => c.HasWonder(w))
			}).ToArray();
			
			this.Clear(3)
				.DrawText("The Wonders of the World", 0, 5, 100, 13)
				.DrawText("The Wonders of the World", 0, 15, 100, 12);
		}
	}
}