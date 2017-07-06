using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    /* リモートホストに送るタグ用 */
    public enum TagConstants
    {
        /* 確定タグ */
        ENTER,
        /* 削除タグ */
        DELETE,
        /* 変換タグ */
        CONV,
        /* コピータグ */
        COPY,
        /* カットタグ */
        CUT,
        /* ペーストタグ */
        PASTE,
        /* 切断タグ */
        END
    }

    public enum IntegerConstants
    {

    }

    /* enum TagConstantsの拡張クラス */
    public static class TagConstantsExpansion
    {
        public static string GetConstants(this TagConstants value)
        {
            string[] values = { "<ENTER>", "<DELETE>", "<CONV>", "<COPY>", "<CUT>", "<PASTE>", "<ENDCONN>" };
            return values[(int)value];
        }
    }

    public class A
    {
        public void B()
        {
            TagConstants.ENTER.GetConstants();
        }
    }
}

