using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UILabel))]
public class LabelStandard : MonoBehaviour
{
    public enum Standard
    {
        None,
        win_w_18,
        win_wa_18,
        win_wb_18,
        win_b_18,
        win_o_18,
        win_r_18,
        win_g_18,

        win_w_20,
        win_wa_20,
        win_wb_20,
        win_b_20,
        win_o_20,
        win_r_20,
        win_g_20,

        win_w_22,
        win_wa_22,
        win_wb_22,
        win_b_22,
        win_o_22,
        win_r_22,
        win_g_22,

        win_w_24,
        win_wa_24,
        win_wb_24,
        win_b_24,
        win_o_24,
        win_r_24,
        win_g_24,

        win_w_30,
        win_wa_30,
        win_wb_30,
        win_b_30,
        win_o_30,
        win_r_30,
        win_g_30,

        Nav_select,
        Nav_off,
        SubNav_select,
        SubNav_off,

        win_y_25,
        win_y_20,

        win_En_18,
        win_En_50,
        win_En_30,
        win_En_30_w,
        win_Gradual_30,

        win_list_1st,
        win_list_2st,
        win_list_3st,
        win_list_pop,
        win_list_loss,
    }

    public Standard value = Standard.None;

    private static Color32 a = new Color32(171, 173, 185, 255);
    private static Color32 b = new Color32(125, 128, 137, 255);
    private static Color32 c = new Color32(255, 255, 255, 255);
    private static Color32 d = new Color32(102, 204, 255, 255);
    private static Color32 e = new Color32(204, 102, 0, 255);
    private static Color32 f = new Color32(255, 102, 102, 255);
    private static Color32 g = new Color32(159, 255, 159, 255);
    private static Color32 h = new Color32(217, 186, 104, 255);

    private struct Format
    {
        public Font font;
        public int fontSize;
        public Color32 color;

        public Format(Font font, int fontSize, Color32 color)
        {
            this.font = font;
            this.fontSize = fontSize;
            this.color = color;
        }
    }

    private static Dictionary<Standard, Format> mFormatDic = new Dictionary<Standard, Format>();

    static LabelStandard()
    {
        mFormatDic.Add(Standard.win_w_18, new Format(null, 18, a));
        mFormatDic.Add(Standard.win_wa_18, new Format(null, 18, b));
        mFormatDic.Add(Standard.win_wb_18, new Format(null, 18, c));
        mFormatDic.Add(Standard.win_b_18, new Format(null, 18, d));
        mFormatDic.Add(Standard.win_o_18, new Format(null, 18, e));
        mFormatDic.Add(Standard.win_r_18, new Format(null, 18, f));
        mFormatDic.Add(Standard.win_g_18, new Format(null, 18, g));

        mFormatDic.Add(Standard.win_w_20, new Format(null, 20, a));
        mFormatDic.Add(Standard.win_wa_20, new Format(null, 20, b));
        mFormatDic.Add(Standard.win_wb_20, new Format(null, 20, c));
        mFormatDic.Add(Standard.win_b_20, new Format(null, 20, d));
        mFormatDic.Add(Standard.win_o_20, new Format(null, 20, e));
        mFormatDic.Add(Standard.win_r_20, new Format(null, 20, f));
        mFormatDic.Add(Standard.win_g_20, new Format(null, 20, g));

        mFormatDic.Add(Standard.win_w_22, new Format(null, 22, a));
        mFormatDic.Add(Standard.win_wa_22, new Format(null, 22, b));
        mFormatDic.Add(Standard.win_wb_22, new Format(null, 22, c));
        mFormatDic.Add(Standard.win_b_22, new Format(null, 22, d));
        mFormatDic.Add(Standard.win_o_22, new Format(null, 22, e));
        mFormatDic.Add(Standard.win_r_22, new Format(null, 22, f));
        mFormatDic.Add(Standard.win_g_22, new Format(null, 22, g));

        mFormatDic.Add(Standard.win_w_24, new Format(null, 24, a));
        mFormatDic.Add(Standard.win_wa_24, new Format(null, 24, b));
        mFormatDic.Add(Standard.win_wb_24, new Format(null, 24, c));
        mFormatDic.Add(Standard.win_b_24, new Format(null, 24, d));
        mFormatDic.Add(Standard.win_o_24, new Format(null, 24, e));
        mFormatDic.Add(Standard.win_r_24, new Format(null, 24, f));
        mFormatDic.Add(Standard.win_g_24, new Format(null, 24, g));

        mFormatDic.Add(Standard.win_w_30, new Format(null, 30, a));
        mFormatDic.Add(Standard.win_wa_30, new Format(null, 30, b));
        mFormatDic.Add(Standard.win_wb_30, new Format(null, 30, c));
        mFormatDic.Add(Standard.win_b_30, new Format(null, 30, d));
        mFormatDic.Add(Standard.win_o_30, new Format(null, 30, e));
        mFormatDic.Add(Standard.win_r_30, new Format(null, 30, f));
        mFormatDic.Add(Standard.win_g_30, new Format(null, 30, g));

        mFormatDic.Add(Standard.Nav_select, new Format(null, 26, c));
        mFormatDic.Add(Standard.Nav_off, new Format(null, 26, a));
        mFormatDic.Add(Standard.SubNav_select, new Format(null, 24, d));
        mFormatDic.Add(Standard.SubNav_off, new Format(null, 24, b));

        mFormatDic.Add(Standard.win_y_25, new Format(null, 25, new Color32(255, 222, 90, 255)));
        mFormatDic.Add(Standard.win_y_20, new Format(null, 20, new Color32(255, 222, 90, 255)));

        mFormatDic.Add(Standard.win_En_18, new Format(null, 18, a));
        mFormatDic.Add(Standard.win_En_50, new Format(null, 50, c));
        mFormatDic.Add(Standard.win_En_30, new Format(null, 30, b));
        mFormatDic.Add(Standard.win_En_30_w, new Format(null, 30, c));
        mFormatDic.Add(Standard.win_Gradual_30, new Format(null, 30, c));

        mFormatDic.Add(Standard.win_list_1st, new Format(null, 22, h));
        mFormatDic.Add(Standard.win_list_2st, new Format(null, 20, c));
        mFormatDic.Add(Standard.win_list_3st, new Format(null, 20, c));
        mFormatDic.Add(Standard.win_list_pop, new Format(null, 20, c));
        mFormatDic.Add(Standard.win_list_loss, new Format(null, 20, b));
    }

    public static void Refresh(UILabel label, LabelStandard.Standard mode)
    {
        Format format = default(Format);
        if (mFormatDic.TryGetValue(mode, out format))
        {
            label.fontSize = format.fontSize;
            label.color = format.color;
        }

        if (mode == Standard.win_Gradual_30)
        {
            label.applyGradient = true;
            label.gradientTop = c;
            label.gradientBottom = new Color32(99, 119, 137, 255);
            
            label.effectStyle = UILabel.Effect.Shadow;
            label.effectColor = Color.black;
            label.effectDistance = new Vector2(1f, -2f);
        }

        label.MarkAsChanged();
    }

    public static Color GetFormatColor(LabelStandard.Standard mode)
    {
        Format format = default(Format);
        if (mFormatDic.TryGetValue(mode, out format))
        {
            return format.color;
        }
        else
        {
            return Color.white;
        }
    }
}