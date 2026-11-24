
using PKHeX.Core;
using static PKHeXMAUI.MainPage;
namespace PKHeXMAUI;

public partial class MiscTab : TabbedPage
{
	public static MiscMainEditor? MME;
	public static MiscRecords? MRE;
	public static MiscPokeblock? MPB;
	public static MiscDecorations? MDE;
	public static MiscPaintings? MPE;
	public static MiscJoyful? MJE;
	public static MiscFerry? MFE;
	public static MiscBattleFrontier? MBF;
	public MiscTab()
	{
		InitializeComponent();
        misctab3.BarBackgroundColor = Color.FromArgb("303030");
        misctab3.BarTextColor = Colors.White;
        MME = new MiscMainEditor();
		MRE = new((SAV3)sav);
        misctab3.Children.Add(MME);
        if (((SAV3)sav).SmallBlock is ISaveBlock3SmallExpansion j)
        {
            MJE = new(j);
			misctab3.Children.Add(MJE);
        }
		if(sav is SAV3E e)
		{
			MFE = new(e);
			misctab3.Children.Add(MFE);
			MBF = new(e);
			misctab3.Children.Add(MBF);
		}
        misctab3.Children.Add(MRE);
        if (((SAV3)sav).LargeBlock is ISaveBlock3LargeHoenn hoenn)
        {
			MPB = new(hoenn);
			MDE = new(hoenn);
			MPE = new((SAV3)sav);
            misctab3.Children.Add(MPB);
            misctab3.Children.Add(MDE);
            misctab3.Children.Add(MPE);
        }
		misctab3.Children.Add(new cancelpage());
		misctab3.Children.Add(new Misc3Save());
	}
}
public partial class Misc3Save : ContentPage
{
	public Misc3Save()
	{
		this.Title = "保存";
		this.Content = new Label() { Text = "MAUI 框架存在一些问题。当前为存档页面。请先导航到其他页面，再选择你原本想打开的页面！" };
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		MiscTab.MPB?.Save();
		MiscTab.MJE?.SaveJoyful();
		Navigation.PopModalAsync();
	}
}