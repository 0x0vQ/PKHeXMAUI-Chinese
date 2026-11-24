using PKHeX.Core;

namespace PKHeXMAUI;

/// <summary>UI-only Simplified Chinese labels. Internal property names and stored values remain unchanged.</summary>
internal static class UiLocalization
{
    private static readonly Dictionary<string, string> Labels = new(StringComparer.Ordinal)
    {
        ["Misc"] = "杂项", ["Legality"] = "合法性", ["Encounter"] = "遭遇", ["ALM"] = "ALM 设置",
        ["StartupPage"] = "启动页面", ["United States"] = "美国", ["True"] = "是", ["False"] = "否", ["IgnoreLegalPopup"] = "忽略合法性提示",
        ["RememberLastSave"] = "记住上次使用的存档", ["DisplayLegalBallsOnly"] = "仅显示合法的精灵球",
        ["AllowIncompatibleConversion"] = "允许不兼容格式转换", ["SetUpdatePKM"] = "自动更新宝可梦数据",
        ["FilterUnavailableSpecies"] = "过滤当前版本无法获得的宝可梦", ["UsePkEditorAsCriteria"] = "使用宝可梦编辑器作为搜索条件",
        ["ShowExtraEncounterInfo"] = "显示额外遭遇信息", ["CheckWordFilter"] = "检查敏感词过滤",
        ["AllowGen1Tradeback"] = "允许第一世代交易回传", ["NicknamedTrade"] = "昵称宝可梦交易",
        ["NicknamedMysteryGift"] = "神秘礼物昵称", ["RNGFrameNotFound"] = "未找到 RNG 帧",
        ["Gen7TransferStarPID"] = "第七世代传送星形 PID", ["Gen8MemoryMissingHT"] = "第八世代缺少当前训练家记忆",
        ["Gen8TransferTrackerNotPresent"] = "缺少 HOME 追踪编号", ["NicknamedAnotherSpecies"] = "昵称与宝可梦种类不符",
        ["ZeroHeightWeight"] = "身高或体重为零", ["CurrentHandlerMismatch"] = "当前训练家不匹配",
        ["CheckActiveHandler"] = "检查当前训练家", ["DefaultOT"] = "默认原训练家名称",
        ["DefaultTID"] = "默认训练家 ID", ["DefaultSID"] = "默认秘密 ID",
        ["UseTrainerData"] = "使用训练家数据", ["TrainerFolderPath"] = "训练家文件夹路径",
        ["SetAllLegalRibbons"] = "自动设置全部合法奖章", ["SetBattleVersion"] = "设置对战版本",
        ["SetBallByColor"] = "根据颜色选择精灵球", ["EnableMemesForIllegalSets"] = "为不合法配置启用梗图",
        ["ForceLevel100For50"] = "等级 50 对战强制等级 100", ["LivingDexAllForms"] = "活体图鉴收录全部形态",
        ["LivingDexNativeOnly"] = "活体图鉴仅收录原生形态", ["LivingDexSetAlpha"] = "活体图鉴设为头目",
        ["LivingDexSetShiny"] = "活体图鉴设为闪光",
        ["Invalid"] = "不合法", ["Fishy"] = "可疑", ["Indeterminate"] = "无法判定", ["Valid"] = "合法",
        ["Normal"] = "一般", ["Fire"] = "火", ["Water"] = "水", ["Electric"] = "电", ["Grass"] = "草",
        ["Ice"] = "冰", ["Fighting"] = "格斗", ["Poison"] = "毒", ["Ground"] = "地面", ["Flying"] = "飞行",
        ["Psychic"] = "超能力", ["Bug"] = "虫", ["Rock"] = "岩石", ["Ghost"] = "幽灵", ["Dragon"] = "龙",
        ["Dark"] = "恶", ["Steel"] = "钢", ["Fairy"] = "妖精"
    };

    public static string Translate(string? value)
    {
        if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
        return Labels.TryGetValue(value, out var translated) ? translated : value;
    }

    public static string DisplayEnum(object? value)
    {
        if (value is null) return string.Empty;
        var name = value.ToString() ?? string.Empty;
        return Translate(name);
    }
}
