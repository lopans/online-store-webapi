using Base.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enums
{
    [UiEnum]
    public enum FlexSize
    {
        [UiEnumValue("XS small to large handset (< 600px)")]
        xs,
        [UiEnumValue("SM small to medium tablet (600px > < 960px)")]
        sm,
        [UiEnumValue("MD large tablet to laptop (960px > < 1264*)")]
        md,
        [UiEnumValue("LG desktop (1264px > < 1904px)")]
        lg,
        [UiEnumValue("XL 4k and ultra-wides (> 1904px)")]
        xl
    }
}
