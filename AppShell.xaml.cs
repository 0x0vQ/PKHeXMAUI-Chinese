using CommunityToolkit.Maui.Storage;
using PKHeX.Core;
using PKHeX.Core.AutoMod;
using System.Windows.Input;
using static PKHeXMAUI.MainPage;
using System;
using System.Diagnostics;

namespace PKHeXMAUI;

public partial class AppShell : Shell
{
	public AppShell(SaveFile sav)
	{
        AppSaveFile = sav;
        InitializeComponent();
        Shelltest = TheShell;
        TheShell.ItemTemplate = new FlyoutCollectionSelector();
        TheShell.MenuItemTemplate = new FlyoutCollectionSelector();
    }
	public static SaveFile? AppSaveFile { get; set; }
    public BoxManipulator manip = new BoxManipulatorMAUI();
    public static bool boxexpanded = false;
    public static bool pkexpanded = false;
    public static bool fileexpanded = false;
    public static bool dataexpanded = false;
    public static Shell? Shelltest;
    public async void DropdownExpansion(object? sender, EventArgs? e)
    {
        if (((string?)((ImageButton?)sender)?.CommandParameter) == "盒子/队伍")
        {
            if (!boxexpanded)
            {
                boxexpanded = true;
                SetFlyoutItemIsVisible(DeleteBoxes, true);
                SetFlyoutItemIsVisible(SortBoxes, true);
                SetFlyoutItemIsVisible(SortBoxesAdvanced, true);
                SetFlyoutItemIsVisible(ModifyBoxes, true);
            }
            else
            {
                SetFlyoutItemIsVisible(DeleteBoxes, false);
                SetFlyoutItemIsVisible(SortBoxes, false);
                SetFlyoutItemIsVisible(SortBoxesAdvanced, false);
                SetFlyoutItemIsVisible(ModifyBoxes, false);
                DeleteExpanded = true;
                DeleteClicked(sender, e);
                SortExpanded = true;
                SortClick(sender, e);
                SortAdvancedExpanded = true;
                SortBoxesAdvancedClicked(sender, e);
                ModifyExpanded = true;
                ModifyBoxesClicked(sender, e);
                boxexpanded = false;
            }
            return;
        }
        if (((string?)((ImageButton?)sender)?.CommandParameter) == "宝可梦编辑器")
        {
            if (!pkexpanded)
            {
                SetFlyoutItemIsVisible(thelegalizer, true);
                SetFlyoutItemIsVisible(impshow, true);
                SetFlyoutItemIsVisible(expshow, true);
                pkexpanded = true;
            }
            else
            {
                SetFlyoutItemIsVisible(thelegalizer, false);
                SetFlyoutItemIsVisible(impshow, false);
                SetFlyoutItemIsVisible(expshow, false);
                pkexpanded = false;
            }
            return;
        }
        if (((string?)((ImageButton?)sender)?.CommandParameter) == "File")
        {
            if (!fileexpanded)
            {
                SetFlyoutItemIsVisible(OpenFile,true);
                SetFlyoutItemIsVisible(SavePKM, true);
                SetFlyoutItemIsVisible(ExportSave, true);
                fileexpanded = true;
            }
            else
            {
                SetFlyoutItemIsVisible(OpenFile, false);
                SetFlyoutItemIsVisible(SavePKM, false);
                SetFlyoutItemIsVisible(ExportSave, false);
                fileexpanded = false;
            }
        }
        if (((string?)((ImageButton?)sender)?.CommandParameter) == "Data")
        {
            if (!dataexpanded)
            {
                SetFlyoutItemIsVisible(LoadBoxes,true);
                SetFlyoutItemIsVisible(DumpBox,true);
                SetFlyoutItemIsVisible(SaveBoxData, true);
                SetFlyoutItemIsVisible(MenuBatchEditor, true);
                dataexpanded = true;
            }
            else
            {
                SetFlyoutItemIsVisible(LoadBoxes, false);
                SetFlyoutItemIsVisible(DumpBox, false);
                SetFlyoutItemIsVisible(SaveBoxData, false);
                SetFlyoutItemIsVisible(MenuBatchEditor, false);
                dataexpanded = false;
            }
        }
    }
    public async void checkbox(object sender, EventArgs e)
    {
        if (TheShell.CurrentPage.GetType() != typeof(BoxTab))
        {
            try
            {
                ((BoxTab)TheShell.CurrentPage).boxview.SelectedItem = null;
            }
            catch(Exception) { }
        }
    }
    public bool SortExpanded = false;
    public bool DeleteExpanded = false;
    public bool SortAdvancedExpanded = false;
    public bool ModifyExpanded = false;
    private void DeleteClicked(object? sender, EventArgs? e)
    {
        if (DeleteExpanded)
        {
            SetFlyoutItemIsVisible(ClearBox, false);
            SetFlyoutItemIsVisible(ClearEggs, false);
            SetFlyoutItemIsVisible(ClearPast, false);
            SetFlyoutItemIsVisible(ClearForeign, false);
            SetFlyoutItemIsVisible(ClearUntrained, false);
            SetFlyoutItemIsVisible(ClearIllegal, false);
            SetFlyoutItemIsVisible(ClearClones, false);
            DeleteExpanded = false;
        }
        else
        {
            SetFlyoutItemIsVisible(ClearBox, true);
            SetFlyoutItemIsVisible(ClearEggs, true);
            SetFlyoutItemIsVisible(ClearPast, true);
            SetFlyoutItemIsVisible(ClearForeign, true);
            SetFlyoutItemIsVisible(ClearUntrained, true);
            SetFlyoutItemIsVisible(ClearIllegal, true);
            SetFlyoutItemIsVisible(ClearClones, true);
            DeleteExpanded = true;
        }
    }

    private async void ClearBoxClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Clear", "清空所有盒子", "清空当前盒子", BoxManipType.DeleteAll);
    }

    private async void ClearEggsClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Clear", "清除所有盒子中的蛋", "清除当前盒子中的蛋", BoxManipType.DeleteEggs);
    }

    private async void ClearPastClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Clear", "清除所有盒子中的旧世代宝可梦", "清除当前盒子中的旧世代宝可梦", BoxManipType.DeletePastGen);
    }

    private async void ClearForeignClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Clear", "清除所有盒子中的外来宝可梦", "清除当前盒子中的外来宝可梦", BoxManipType.DeleteForeign);
    }

    private async void ClearUntrainedClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Clear", "清除所有盒子中的未训练宝可梦", "清除当前盒子中的未训练宝可梦", BoxManipType.DeleteUntrained);
    }

    private async Task ManipulateBoxes(string title, string allBoxesMessage, string currentBoxMessage, BoxManipType manipType)
    {
        var displayTitle = title == "Clear" ? "清理盒子" : title == "Modify" ? "批量修改" : title;
        if (await DisplayAlertAsync(displayTitle, allBoxesMessage, "是", "否"))
        {
            manip.Execute(manipType, 0, true);
        }
        else if (await DisplayAlertAsync(displayTitle, currentBoxMessage, "是", "否"))
        {
            manip.Execute(manipType, BoxTab.CurrentBox, false);
        }
        manipType.TryGetManipCategoryName(out var cat);
        HideAllFlyoutItems(cat);
    }

    private void HideAllFlyoutItems(string? Menu)
    {
        if (Menu == "Delete")
        {
            SetFlyoutItemIsVisible(ClearBox, false);
            SetFlyoutItemIsVisible(ClearEggs, false);
            SetFlyoutItemIsVisible(ClearPast, false);
            SetFlyoutItemIsVisible(ClearForeign, false);
            SetFlyoutItemIsVisible(ClearUntrained, false);
            SetFlyoutItemIsVisible(ClearIllegal, false);
            SetFlyoutItemIsVisible(ClearClones, false);
            DeleteExpanded = false;
        }
        if (Menu == "Sort")
        {
            SetFlyoutItemIsVisible(SortSpecies, false);
            SetFlyoutItemIsVisible(SortSpeciesReverse, false);
            SetFlyoutItemIsVisible(SortLevellohi, false);
            SetFlyoutItemIsVisible(SortLevelhilo, false);
            SetFlyoutItemIsVisible(SortMetDate, false);
            SetFlyoutItemIsVisible(SortSpeciesName, false);
            SetFlyoutItemIsVisible(SortShiny, false);
            SetFlyoutItemIsVisible(SortRandom, false);
            SortExpanded = false;
        }
        if (Menu == "SortAdvanced")
        {
            SetFlyoutItemIsVisible(SortUsage, false);
            SetFlyoutItemIsVisible(SortIV, false);
            SetFlyoutItemIsVisible(SortEV, false);
            SetFlyoutItemIsVisible(SortOwnership, false);
            SetFlyoutItemIsVisible(SortType, false);
            if (sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortTera, false);
            SetFlyoutItemIsVisible(SortVersion, false);
            SetFlyoutItemIsVisible(SortBaseStat, false);
            if (sav.Version == GameVersion.PLA || sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortScale, false);
            SetFlyoutItemIsVisible(SortRibbonCount, false);
            SetFlyoutItemIsVisible(SortMarkCount, false);
            SetFlyoutItemIsVisible(SortLegal, false);
            SetFlyoutItemIsVisible(SortEncounter, false);
            SortAdvancedExpanded = false;
        }
        if (Menu == "Modify")
        {
            SetFlyoutItemIsVisible(HatchEggs, false);
            SetFlyoutItemIsVisible(MaxFriendship, false);
            SetFlyoutItemIsVisible(MaxLevel, false);
            SetFlyoutItemIsVisible(ResetMoves, false);
            SetFlyoutItemIsVisible(RandomizeMoves, false);
            SetFlyoutItemIsVisible(HyperTrain, false);
            SetFlyoutItemIsVisible(RemoveNicknames, false);
            SetFlyoutItemIsVisible(DeleteHeldItem, false);
            SetFlyoutItemIsVisible(Heal, false);
            ModifyExpanded = false;
        }
        Shell.Current?.FlyoutIsPresented = false;
    }
    private void SortClick(object? sender, EventArgs? e)
    {
        if (SortExpanded)
        {
            SetFlyoutItemIsVisible(SortSpecies, false);
            SetFlyoutItemIsVisible(SortSpeciesReverse, false);
            SetFlyoutItemIsVisible(SortLevellohi, false);
            SetFlyoutItemIsVisible(SortLevelhilo, false);
            SetFlyoutItemIsVisible(SortMetDate, false);
            SetFlyoutItemIsVisible(SortSpeciesName, false);
            SetFlyoutItemIsVisible(SortShiny, false);
            SetFlyoutItemIsVisible(SortRandom, false);
            SortExpanded = false;
        }
        else
        {
            SetFlyoutItemIsVisible(SortSpecies, true);
            SetFlyoutItemIsVisible(SortSpeciesReverse, true);
            SetFlyoutItemIsVisible(SortLevellohi, true);
            SetFlyoutItemIsVisible(SortLevelhilo, true);
            SetFlyoutItemIsVisible(SortMetDate, true);
            SetFlyoutItemIsVisible(SortSpeciesName, true);
            SetFlyoutItemIsVisible(SortShiny, true);
            SetFlyoutItemIsVisible(SortRandom, true);
            SortExpanded = true;
        }
    }
    private async void SortBySpecies(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按全国图鉴编号对所有盒子排序吗？", "要按全国图鉴编号对当前盒子排序吗？", BoxManipType.SortSpecies);
    }
    private async void SortSpeciesReverseClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按全国图鉴编号倒序排列所有盒子吗？", "要按全国图鉴编号倒序排列当前盒子吗？", BoxManipType.SortSpeciesReverse);
    }

    private async void SortLevellohiClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按等级从低到高排列所有盒子吗？", "要按等级从低到高排列当前盒子吗？", BoxManipType.SortLevel);
    }

    private async void SortLevelhiloClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按等级从高到低排列所有盒子吗？", "要按等级从高到低排列当前盒子吗？", BoxManipType.SortLevelReverse);
    }

    private async void SortMetDateClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按相遇日期排列所有盒子吗？", "要按相遇日期排列当前盒子吗？", BoxManipType.SortDate);
    }

    private async void SortSpeciesNameClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按名称排列所有盒子吗？", "要按名称排列当前盒子吗？", BoxManipType.SortName);
    }

    private async void SortShinyClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要按闪光状态排列所有盒子吗？", "要按闪光状态排列当前盒子吗？", BoxManipType.SortShiny);
    }

    private async void SortRandomClicked(object sender, EventArgs e)
    {
        await ManipulateBoxes("Sort", "要随机排列所有盒子吗？", "要随机排列当前盒子吗？", BoxManipType.SortRandom);
    }

    private void SortBoxesAdvancedClicked(object? sender, EventArgs? e)
    {
        if (SortAdvancedExpanded)
        {
            SetFlyoutItemIsVisible(SortUsage, false);
            SetFlyoutItemIsVisible(SortIV, false);
            SetFlyoutItemIsVisible(SortEV, false);
            SetFlyoutItemIsVisible(SortOwnership, false);
            SetFlyoutItemIsVisible(SortType, false);
            if (sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortTera, false);
            SetFlyoutItemIsVisible(SortVersion, false);
            SetFlyoutItemIsVisible(SortBaseStat, false);
            if (sav.Version == GameVersion.PLA || sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortScale, false);
            SetFlyoutItemIsVisible(SortRibbonCount, false);
            SetFlyoutItemIsVisible(SortMarkCount, false);
            SetFlyoutItemIsVisible(SortLegal, false);
            SetFlyoutItemIsVisible(SortEncounter, false);
            SortAdvancedExpanded = false;
        }
        else
        {
            SetFlyoutItemIsVisible(SortUsage, true);
            SetFlyoutItemIsVisible(SortIV, true);
            SetFlyoutItemIsVisible(SortEV, true);
            SetFlyoutItemIsVisible(SortOwnership, true);
            SetFlyoutItemIsVisible(SortType, true);
            SetFlyoutItemIsVisible(SortVersion, true);
            SetFlyoutItemIsVisible(SortBaseStat, true);
            if (sav.Version == GameVersion.PLA || sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortScale, true);
            if (sav.Version == GameVersion.VL || sav.Version == GameVersion.SL)
                SetFlyoutItemIsVisible(SortTera, true);
            SetFlyoutItemIsVisible(SortRibbonCount, true);
            SetFlyoutItemIsVisible(SortMarkCount, true);
            SetFlyoutItemIsVisible(SortLegal, true);
            SetFlyoutItemIsVisible(SortEncounter, true);
            SortAdvancedExpanded = true;
        }
    }

    private void SortUsageClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按使用情况排列所有盒子吗？", "要按使用情况排列当前盒子吗？", BoxManipType.SortUsage);
    }

    private void SortIVClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按个体值潜力排列所有盒子吗？", "要按个体值潜力排列当前盒子吗？", BoxManipType.SortPotential);
    }

    private void SortEVClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按努力值训练排列所有盒子吗？", "要按努力值训练排列当前盒子吗？", BoxManipType.SortTraining);
    }

    private void SortOwnershipClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按所有权排列所有盒子吗？", "要按所有权排列当前盒子吗？", BoxManipType.SortOwner);
    }

    private void SortTypeClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按属性排列所有盒子吗？", "要按属性排列当前盒子吗？", BoxManipType.SortType);
    }

    private void SortVersionClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按游戏版本排列所有盒子吗？", "要按游戏版本排列当前盒子吗？", BoxManipType.SortVersion);
    }

    private void SortBaseStatClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按种族值总和排列所有盒子吗？", "要按种族值总和排列当前盒子吗？", BoxManipType.SortBST);
    }

    private void SortRibbonClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按奖章数量排列所有盒子吗？", "要按奖章数量排列当前盒子吗？", BoxManipType.SortRibbons);
    }

    private void SortMarkClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按证明数量排列所有盒子吗？", "要按证明数量排列当前盒子吗？", BoxManipType.SortMarks);
    }

    private void SortLegalClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按合法性排列所有盒子吗？", "要按合法性排列当前盒子吗？", BoxManipType.SortLegal);
    }

    private void SortEncounterClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按遭遇类型排列所有盒子吗？", "要按遭遇类型排列当前盒子吗？", BoxManipType.SortEncounterType);
    }

    private void SortScaleClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按体型尺寸排列所有盒子吗？", "要按体型尺寸排列当前盒子吗？", BoxManipType.SortScale);
    }

    private void SortTeraClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("高级排序", "要按太晶属性排列所有盒子吗？", "要按太晶属性排列当前盒子吗？", BoxManipType.SortTypeTera);
    }

    private void ModifyBoxesClicked(object? sender, EventArgs? e)
    {
        if (ModifyExpanded)
        {
            SetFlyoutItemIsVisible(HatchEggs, false);
            SetFlyoutItemIsVisible(MaxFriendship, false);
            SetFlyoutItemIsVisible(MaxLevel, false);
            SetFlyoutItemIsVisible(ResetMoves, false);
            SetFlyoutItemIsVisible(RandomizeMoves, false);
            SetFlyoutItemIsVisible(HyperTrain, false);
            SetFlyoutItemIsVisible(RemoveNicknames, false);
            SetFlyoutItemIsVisible(DeleteHeldItem, false);
            SetFlyoutItemIsVisible(Heal, false);
            ModifyExpanded = false;
        }
        else
        {
            SetFlyoutItemIsVisible(HatchEggs, true);
            SetFlyoutItemIsVisible(MaxFriendship, true);
            SetFlyoutItemIsVisible(MaxLevel, true);
            SetFlyoutItemIsVisible(ResetMoves, true);
            SetFlyoutItemIsVisible(RandomizeMoves, true);
            SetFlyoutItemIsVisible(HyperTrain, true);
            SetFlyoutItemIsVisible(RemoveNicknames, true);
            SetFlyoutItemIsVisible(DeleteHeldItem, true);
            SetFlyoutItemIsVisible(Heal, true);
            ModifyExpanded = true;
        }
    }

    private void HatchEggsClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要孵化所有盒子中的蛋吗？", "只孵化当前盒子中的蛋吗？", BoxManipType.ModifyHatchEggs);
    }

    private void MaxFriendshipClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要将所有盒子中宝可梦的亲密度设为最大值吗？", "要将当前盒子中宝可梦的亲密度设为最大值吗？", BoxManipType.ModifyMaxFriendship);
    }

    private void MaxLevelClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要将所有盒子中宝可梦的等级设为最大值吗？", "要将当前盒子中宝可梦的等级设为最大值吗？", BoxManipType.ModifyMaxLevel);
    }

    private void ResetMovesClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要重置所有盒子中宝可梦的招式吗？", "要重置当前盒子中宝可梦的招式吗？", BoxManipType.ModifyResetMoves);
    }

    private void RandomizeMovesClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要随机化所有盒子中宝可梦的招式吗？", "要随机化当前盒子中宝可梦的招式吗？", BoxManipType.ModifyRandomMoves);
    }

    private void HyperTrainClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要对所有盒子中的宝可梦进行极限特训吗？", "要对当前盒子中的宝可梦进行极限特训吗？", BoxManipType.ModifyHyperTrain);
    }

    private void RemoveNicknamesClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要移除所有盒子中宝可梦的昵称吗？", "要移除当前盒子中宝可梦的昵称吗？", BoxManipType.ModifyRemoveNicknames);
    }

    private void DeleteHeldItemClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要移除所有盒子中宝可梦携带的道具吗？", "要移除当前盒子中宝可梦携带的道具吗？", BoxManipType.ModifyRemoveItem);
    }

    private void HealClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Modify", "要恢复所有盒子中宝可梦的状态和 PP 吗？", "要恢复当前盒子中宝可梦的状态和 PP 吗？", BoxManipType.ModifyHeal);
    }

    private void ClearNoHeldItemClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Clear", "要删除所有未携带道具的宝可梦吗？", "要删除当前盒子中未携带道具的宝可梦吗？", BoxManipType.DeleteItemless);
    }

    private void ClearIllegalClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Clear", "清除所有盒子中的不合法宝可梦", "清除当前盒子中的不合法宝可梦", BoxManipType.DeleteIllegal);
    }

    private void ClearClonesClicked(object sender, EventArgs e)
    {
        ManipulateBoxes("Clear", "清除所有盒子中的克隆宝可梦", "清除当前盒子中的克隆宝可梦", BoxManipType.DeleteClones);
    }

    private async void OpenPKMClicked(object sender, EventArgs e)
    {
        TheShell.FlyoutIsPresented = false;
        ((MainPage)PKPage).pk9picker_Clicked(sender, e);
    }
    private async void ExportSaveClicked(object sender, EventArgs e)
    {
        // Set box now that we're saving
        if (sav.HasBox)
            sav.CurrentBox = BoxTab.CurrentBox;
        var ext = sav.Metadata.GetSuggestedExtension();
        var flags = sav.Metadata.GetSuggestedFlags(ext);
        await using var LiveStream = new MemoryStream(sav.Write(flags).ToArray());
        var result = await FileSaver.Default.SaveAsync(sav.Metadata.FileName??"", LiveStream, CancellationToken.None);
        if (result.IsSuccessful)
            await DisplayAlertAsync("成功", $"存档已导出到：{result.FilePath}", "取消");
        else
            await DisplayAlertAsync("失败", $"存档导出失败：{result.Exception.Message}", "取消");
    }
    private async void SavePKMClicked(object sender, EventArgs e)
    {
        TheShell.FlyoutIsPresented = false;
        ((MainPage)PKPage).pk9saver_Clicked(sender, e);
    }

    private async void LegalizePKM(object sender, EventArgs e)
    {
        TheShell.FlyoutIsPresented = false;
        ((MainPage)PKPage).legalize(sender, e);
    }

    private async void ImpShowClicked(object sender, EventArgs e)
    {
        TheShell.FlyoutIsPresented = false;
        ((MainPage)PKPage).ImportShowdown(sender, e);
    }

    private async void ExpShowClicked(object sender, EventArgs e)
    {
        TheShell.FlyoutIsPresented = false;
        ((MainPage)PKPage).ExportShowdown(sender, e);
    }

    private async void LoadBoxesClicked(object sender, EventArgs e)
    {
        var folder = await FolderPicker.PickAsync(CancellationToken.None);
        if (folder.IsSuccessful)
        {
            foreach (var f in Directory.GetFiles(folder.Folder.Path))
            {
                PKM pkm = (PKM?)FileUtil.GetSupportedFile(f)??EntityBlank.GetBlank(sav.Generation);
                if (pkm.GetType() != sav.PKMType)
                {
                    var newpkm = EntityConverter.ConvertToType(pkm, sav.PKMType, out var result)??EntityBlank.GetBlank(sav.Generation);
                    if (result.IsSuccess || PSettings.AllowIncompatibleConversion)
                    {
                        sav.AdaptToSaveFile(newpkm);
                        sav.SetBoxSlotAtIndex(newpkm, sav.NextOpenBoxSlot());
                        continue;
                    }
                }
                sav.AdaptToSaveFile(pkm);
                sav.SetBoxSlotAtIndex(pkm, sav.NextOpenBoxSlot());
            }
            if (TheShell.CurrentPage is BoxTab tab)
                tab.fillbox();
        }
    }

    private async void DumpBoxClicked(object sender, EventArgs e)
    {
        if (await DisplayAlertAsync("导出","导出所有盒子？", "是", "取消"))
        {
            var result = await FolderPicker.PickAsync(CancellationToken.None);
            if(result.IsSuccessful)
                BoxExport.Export(sav, result.Folder.Path, BoxExportSettings.Default);
            return;
        }
        else if(await DisplayAlertAsync("导出","导出当前盒子？", "是", "取消"))
        {
            var result = await FolderPicker.PickAsync(CancellationToken.None);
            if (result.IsSuccessful)
                BoxExport.Export(sav,result.Folder.Path, BoxExportSettings.Default with { Scope = BoxExportScope.Current });
            return;
        }
    }

    private async void SaveBoxDataClicked(object sender, EventArgs e)
    {
        if (await DisplayAlertAsync("导出", "导出全部盒子？", "是", "否"))
        {
            await using MemoryStream boxstream = new(sav.GetPCBinary());
            await FileSaver.SaveAsync("pcdata.bin", boxstream);
            return;
        }
        if (await DisplayAlertAsync("导出", "导出当前盒子？", "是", "取消"))
        {
            await using MemoryStream Cboxstream = new(sav.GetBoxBinary(sav.CurrentBox));
            await FileSaver.SaveAsync($"盒子数据 {sav.CurrentBox}.bin", Cboxstream);
            return;
        }
    }

    private void OpenBatchEditorClick(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new BatchEditor());
    }
}
public class BoxManipulatorMAUI : BoxManipulator
{
    protected override SaveFile SAV => sav;
    protected override void FinishBoxManipulation(string message, bool all, int count) => Shell.Current.DisplayAlertAsync("完成", message + $"（{count}）", "取消");

    protected override bool CanManipulateRegion(int start, int end, string prompt, string fail)
    {
        bool canModify = base.CanManipulateRegion(start, end, prompt, fail);
        if (!canModify && !string.IsNullOrEmpty(fail))
            Shell.Current.DisplayAlertAsync("盒子", fail, "取消");
        return canModify;
    }
}
public class FlyoutCollectionSelector : DataTemplateSelector
{
    public DataTemplate FlyoutItemDataTemplate =  new(() =>
    {
        Grid grid = new() { Padding = 15 };
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        Label label = new() { TextColor = Colors.White, HorizontalOptions = LayoutOptions.Center };
        label.SetBinding(Label.TextProperty, "Title");
        grid.Add(label);
        Image icon = new() { HorizontalOptions = LayoutOptions.Start, HeightRequest=25, WidthRequest=25 } ;
        icon.SetBinding(Image.SourceProperty, "Icon");
        grid.Add(icon);
        ImageButton button = new()
        {
            BackgroundColor = Colors.White,
            HeightRequest = 25,
            WidthRequest = 25,
            Source = "dump.png"
        };
        button.SetBinding(ImageButton.CommandParameterProperty,"Title");
        button.Clicked += ((AppShell)AppShell.Current).DropdownExpansion;
        grid.Add(button, 1);
        Border border = new()
        {
            Stroke = Colors.White,
            BackgroundColor = Colors.Transparent,
            Content = grid
        };
        return border;
    });
    public DataTemplate MenuItemDataTemplate = new(() =>
    {
        Grid grid = new() { Padding = 15};
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        Label label = new() { TextColor = Colors.White , HorizontalOptions = LayoutOptions.Center};
        label.SetBinding(Label.TextProperty, "Title");
        grid.Add(label);
        Image icon = new() { HorizontalOptions = LayoutOptions.Start, HeightRequest = 25, WidthRequest = 25 };
        icon.SetBinding(Image.SourceProperty, "Icon");
        grid.Add(icon);
        Border border = new()
        {
            Stroke = Colors.White,
            BackgroundColor = Colors.Transparent,
            Content = grid
        };
        return border;
    });
    public DataTemplate MenuItemDataTemplate2 = new(() =>
    {
        Grid grid = new() { Padding = 15 };
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        Label label = new() { TextColor = Colors.White};
        label.SetBinding(Label.TextProperty, "Title");
        grid.Add(label);
        Image icon = new() { HorizontalOptions = LayoutOptions.Start, HeightRequest = 25, WidthRequest = 25 };
        icon.SetBinding(Image.SourceProperty, "Icon");
        grid.Add(icon);
        Border border = new()
        {
            Stroke = Colors.White,
            BackgroundColor = Colors.Transparent,
            Margin = new Thickness(20, 0, 20, 0),
            Content = grid
        };
        return border;
    });
    public DataTemplate MenuItemDropdownDataTemplate = new(() =>
    {
        Grid grid = new() { Padding = 15 };
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
        grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        Label label = new() { TextColor = Colors.White, HorizontalOptions = LayoutOptions.Center };
        label.SetBinding(Label.TextProperty, "Text");
        grid.Add(label);
        Image icon = new() { HorizontalOptions = LayoutOptions.Start, HeightRequest = 25, WidthRequest = 25 };
        icon.SetBinding(Image.SourceProperty, "Icon");
        grid.Add(icon);
        ImageButton button = new()
        {
            BackgroundColor = Colors.White,
            HeightRequest = 25,
            WidthRequest = 25,
            Source = "dump.png"
        };
        button.SetBinding(ImageButton.CommandParameterProperty, "Text");
        button.Clicked += ((AppShell)AppShell.Current).DropdownExpansion;
        grid.Add(button, 1);
        Border border = new()
        {
            Stroke = Colors.White,
            BackgroundColor = Colors.Transparent,
            Content = grid
        };
        return border;
    });
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is FlyoutItem e)
        {
            if (e.Title == "盒子/队伍" || e.Title == "宝可梦编辑器" || e.Title == "File")
                return FlyoutItemDataTemplate;
            else
                return MenuItemDataTemplate;
        }
        if(item is Tab t)
        {
            if (t.Title == "盒子/队伍" || t.Title == "宝可梦编辑器" || t.Title == "File")
                return FlyoutItemDataTemplate;
            else
                return MenuItemDataTemplate;
        }
        if (item.GetType().GetProperty("Title")?.GetValue(item) is string s)
        {
            if(s == "File"|| s == "Data")
            return MenuItemDropdownDataTemplate;
        }
        return MenuItemDataTemplate2;
    }
}
