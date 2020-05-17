using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 进化史
{
    public class TestTrackNo : TestVirtual
    {
        public string GetTrack(string trackNo)
        {
            trackNo = "HM854756";
            if (this.IsRealTrackNo(trackNo))
            {
                trackNo = "";
            }
            return trackNo;
        }
        public override bool IsRealTrackNo(string trackNo)
        {
            return trackNo.StartsWith("HM");
        }
    }
}
