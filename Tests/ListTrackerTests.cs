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

        [Fact]
        public void AddElement()
        {
            var list = new List<object>();
            var key = "lorem";
            var element = new object();

            var listTracker = new ListTracker();

            listTracker.AddElement(list, key, element);

            Assert.Same(element, listTracker.GetElement(list, key));
        }

        [Fact]
        public void RemoveElement()
        {
            var list = new List<object>();
            var key = "lorem";
            var element = new object();

            var listTracker = new ListTracker();

            listTracker.AddElement(list, key, element);

            Assert.Same(element, listTracker.GetElement(list, key));

            listTracker.RemoveElement(list, key);

            Assert.Null(listTracker.GetElement(list, key));
        }
    }
}
