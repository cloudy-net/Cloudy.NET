using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class ListTrackerTests
    {
        [Fact]
        public void ListIndices()
        {
            var list1 = new List<object> { new object(), new object() };

            var listTracker = new ListTracker();

            Assert.Same(list1[0], listTracker.GetElement(list1, "0"));
            Assert.Same(list1[1], listTracker.GetElement(list1, "1"));
        }
    }
}
