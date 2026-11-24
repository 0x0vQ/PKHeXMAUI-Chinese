using CommunityToolkit.Maui.Storage;
using PKHeX.Core;

namespace PKHeXMAUI;

public partial class MiscCityForest : ContentPage
{
    private SAV5 SAV;
    public MiscCityForest(SAV5 sav)
	{
		InitializeComponent();
        SAV = sav;
	}
    private const string ForestCityBinFilter = "白色森林/黑色城市数据文件|*.fc5";
    private const string ForestCityBinPath = "{0}.fc5";

    private async void B_DumpFC_Click(object sender, EventArgs e)
    {
        if (SAV is not SAV5BW bw)
            return;
        var sfd = await FolderPicker.PickAsync("");
        if (sfd == null)
            return;
        var filename = string.Format(ForestCityBinPath, SAV.Version);

        var data = bw.Forest.ForestCity.Span;
        File.WriteAllBytes(sfd.Folder + filename, data);
    }

    private async void B_ImportFC_Click(object sender, EventArgs e)
    {
        if (SAV is not SAV5BW bw)
            return;

        var ofd = await FilePicker.PickAsync();
        if (ofd == null)
            return;

        var fi = new FileInfo(ofd.FileName);
        if (fi.Length != WhiteBlack5BW.ForestCitySize)
        {
            await DisplayAlertAsync("错误",$"文件大小不正确。当前大小：{fi.Length} 字节；预期大小：{WhiteBlack5BW.ForestCitySize} 字节。", "取消");
            return;
        }

        var data = File.ReadAllBytes(ofd.FileName);
        bw.SetData(bw.Forest.ForestCity.Span, data);
    }
}