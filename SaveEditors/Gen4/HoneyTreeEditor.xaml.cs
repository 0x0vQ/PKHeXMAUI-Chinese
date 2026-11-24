using PKHeX.Core;

namespace PKHeXMAUI;

public partial class HoneyTreeEditor : ContentPage
{
    private readonly SAV4Sinnoh Origin;
    private readonly SAV4Sinnoh SAV;
    private readonly byte[] MunchlaxTrees;
    private int entry;
    private bool loading;
    private HoneyTreeValue? Tree;
    public HoneyTreeEditor(SAV4Sinnoh sav)
	{
		InitializeComponent();
        SAV = (SAV4Sinnoh)(Origin = sav).Clone();

        // Get Munchlax tree for this savegame in screen
        MunchlaxTrees = new byte[4];
        HoneyTreeUtil.CalculateMunchlaxTrees(SAV.ID32, MunchlaxTrees);
        var names = new object[] { "205 号道路（花苑镇一侧）", "205 号道路（百代市一侧）", "206 号道路", "207 号道路", "208 号道路", "209 号道路", "210 号道路（随意镇一侧）", "210 号道路（神和镇一侧）", "211 号道路", "212 号道路（家缘市一侧）", "212 号道路（湿原市一侧）", "213 号道路", "214 号道路", "215 号道路", "218 号道路", "221 号道路", "222 号道路", "谷间发电厂", "百代森林", "钢铁岛工厂", "花苑花田" };
        const string sep = "- ";
        CB_TreeList.ItemSource = names;
        L_Tree0.Text = string.Join(Environment.NewLine,
            sep + names[MunchlaxTrees[0]],
            sep + names[MunchlaxTrees[1]],
            sep + names[MunchlaxTrees[2]],
            sep + names[MunchlaxTrees[3]]);

        CB_TreeList.SelectedIndex = 0;
    }
    private ushort TreeSpecies => SAV.GetHoneyTreeSpecies((int)NUD_Group.Number, (int)NUD_Slot.Number);
    private void B_Catchable_Clicked(object sender, EventArgs e) => NUD_Time.Number = 1080;
    private async void ChangeGroupSlot(object sender, EventArgs e)
    {
        var species = TreeSpecies;
        L_Species.Text = GetLabelText(species);

        if (loading)
            return;

        if (species == (int)Species.Munchlax && !MunchlaxTrees.AsSpan().Contains((byte)CB_TreeList.SelectedIndex))
            await DisplayAlertAsync("警告","在这棵树上捕捉卡比兽会导致其与此存档的 TID16/SID16 组合不匹配，从而被判定为不合法。","确定");
    }
    private static string GetLabelText(ushort species)
    {
        var str = GameInfo.Strings;
        var arr = str.specieslist;
        if (species != (int)Species.Silcoon)
            return arr[species];

        // Silcoon/Cascoon
        var games = str.gamelist;
        return $"{arr[species + 0]} ({games[(int)GameVersion.D]})" + Environment.NewLine +
               $"{arr[species + 2]} ({games[(int)GameVersion.P]})";
    }
    private void B_Cancel_Clicked(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void ChangeTree(object sender, EventArgs e)
    {
        SaveTree();
        entry = CB_TreeList.SelectedIndex;
        ReadTree();
    }

    private void ReadTree()
    {
        loading = true;
        Tree = SAV.GetHoneyTree(entry);

        NUD_Time.Number = Math.Min(NUD_Time.MaxValue, Tree.Time);
        NUD_Shake.Number = Math.Min(NUD_Shake.MaxValue, Tree.Shake);
        NUD_Group.Number = Math.Min(NUD_Group.MaxValue, Tree.Group);
        NUD_Slot.Number = Math.Min(NUD_Slot.MaxValue, Tree.Slot);

        ChangeGroupSlot(this, EventArgs.Empty);
        loading = false;
    }

    private void SaveTree()
    {
        if (Tree is null)
            return;

        Tree.Time = (uint)NUD_Time.Number;
        Tree.Shake = (int)NUD_Shake.Number;
        Tree.Group = (int)NUD_Group.Number;
        Tree.Slot = (int)NUD_Slot.Number;

        SAV.SetHoneyTree(Tree, entry);
    }

    private void B_Save_Clicked(object sender, EventArgs e)
    {
        SaveTree();
        Origin.CopyChangesFrom(SAV);
        Navigation.PopModalAsync();
    }

}